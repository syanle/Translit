using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using translit.Properties;

namespace translit
{
    public partial class TransForm : Form
    {
        KeyboardHook hook = new KeyboardHook();

        #region fields

        private IntPtr _clipboardViewerNext;

        string s = string.Format(Resources.SogouTranslaterUrl, 0);
        Boolean alwaysOnTop = false;

        #endregion

        #region WinApi 32 functions definition

        [DllImport(dllName: "User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport(dllName: "user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion

        //shadow start
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        //shadow end


        #region Constructors
        public TransForm()
        {
            try
            {
                CheckUpdate();
            }
            catch
            {
                //pass
            }
            InitializeComponent();

            // register the event that is fired after the key press.
            hook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            // hook.RegisterHotKey(translit.ModifierKeys.Control | translit.ModifierKeys.Alt, Keys.F12);
            hook.RegisterHotKey(translit.ModifierKeys.Alt, Keys.F1);

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            HookMouseMove(this.Controls);
            //sogouWordDefWebBrowser.DocumentCompleted += sogouWordDefWebBrowser_DocumentCompleted;

            m_aeroEnabled = false;
            _clipboardViewerNext = SetClipboardViewer(hWndNewViewer: this.Handle);
            //WindowState = FormWindowState.Minimized;
            Startupballon(CheckMessage());

            this.components = new System.ComponentModel.Container();


            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            hiddenNotifyIcon.ContextMenuStrip = this.iconContextMenu;

            // Handle the DoubleClick event to activate the form.
            hiddenNotifyIcon.DoubleClick += new System.EventHandler(this.hiddenNotifyIcon_DoubleClick);

            InitSettings();

            //refresh?
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        bool PronunciationActivated = Settings.Default.PronunciationActivated;
        bool Chinese2EnglishEnabled = Settings.Default.Chinese2EnglishEnabled;
        string PronunciationAccent = Settings.Default.PronunciationAccent;

        private void InitSettings()
        {
            this.iPnonToolStripMenuItem.Checked = PronunciationActivated;
            this.iCeonToolStripMenuItem.Checked = Chinese2EnglishEnabled;
        }

        private void hiddenNotifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            TranslitActivated = !TranslitActivated;
            this.iCbonToolStripMenuItem.Checked = !this.iCbonToolStripMenuItem.Checked;
            if (TranslitActivated) { hiddenNotifyIcon.Icon = Resources.translit; }
            else { hiddenNotifyIcon.Icon = Resources.translit_bw; }
            string iconStateMsg = "复制翻译已" + (TranslitActivated ? "开启" : "关闭");
            Startupballon(iconStateMsg);
            this.hiddenNotifyIcon.Text = ("Translit! " + Resources.TranslitCurrentVersion + "\n" + iconStateMsg + "\n开关快捷键：Alt+F1");
        }

        public void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            //Set test data
            //SendKeys.Send("^c");
            hiddenNotifyIcon_DoubleClick((object)sender, (EventArgs)e);
            miniClose_MouseClick((object)sender, null);
        }

        #endregion

        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        string lastOriginalText = "";
        public long lastPostTime = 0;
        const int wmDrawclipboard = 0x308;
        //actually 16
        private const int tbMarginTopBottom = 18;
        private const int titlePanelHeightMinus1 = 29;
        //private static ConfigIO PreferenceConfigs = new ConfigIO();

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //TO DO: text selection trigger
                //https://stackoverflow.com/questions/51557059/how-to-get-selected-text-of-currently-focused-window
                case wmDrawclipboard:
                    if (Clipboard.ContainsText() && TranslitActivated && !initialRun && !cpInTranslit && tbClipboardText.SelectedText == String.Empty && CheckTextValid(Clipboard.GetText()))
                    {
                        long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                        if (currentTime - lastPostTime > 1000)
                        {
                            lastPostTime = currentTime;
                            //get rid of BOM and Byte order mark: sogou misreconize English as Chinese
                            string clipboardText = Clipboard.GetText().Trim(new char[] { '\uFEFF', '\u200B' });
                            Console.WriteLine(System.Text.RegularExpressions.Regex.Escape(clipboardText));

                            if (alwaysOnTop && !tbEditing)
                            {
                                try
                                {
                                    if (!lastOriginalText.EndsWith("-")) lastOriginalText = lastOriginalText.Trim() + " ";
                                    if (lastOriginalText.Trim().EndsWith(".")) lastOriginalText = lastOriginalText.Trim() + "\r\n";
                                    tbClipboardText.Text = GetTranslatedTextAsync(lastOriginalText + clipboardText);
                                }
                                catch (WebException)
                                {
                                    tbClipboardText.Text = "网络连接失败，请检查您的网络设置";
                                    lastOriginalText = "";
                                    lastPrettifiedQueryText = "";
                                }

                                tbClipboardText.SelectionStart = tbClipboardText.TextLength;
                                lastOriginalText += clipboardText;
                                tbClipboardText.ScrollToCaret();
                            }
                            else if (!alwaysOnTop)
                            {
                                if (clipboardText == lastOriginalText)
                                {
                                    this.Show();
                                    WindowState = FormWindowState.Normal;
                                }
                                else
                                {
                                    try
                                    {
                                        Console.WriteLine("try to get sogou");
                                        tbClipboardText.Text = GetTranslatedTextAsync(clipboardText);
                                        lastOriginalText = clipboardText;
                                        //this.wordDefWebBrowser.Navigate("https://fanyi.sogou.com/#auto/zh-CHS/test");
                                    }
                                    catch (WebException)
                                    {
                                        tbClipboardText.Text = "网络连接失败，请检查您的网络设置";
                                        lastOriginalText = "";
                                        lastPrettifiedQueryText = "";
                                    }
                                }
                            }

                            if (!tbEditing)
                            {
                                WidthResizer();
                                HeightResizer();
                                //scroll to top: dirty fix the weird bug of first line missing when pinned
                                tbClipboardText.SelectionStart = 0;
                                tbClipboardText.ScrollToCaret();
                            }
                        }
                        else
                        {
                            //pass
                        }
                    }
                    else
                    {
                        base.WndProc(m: ref m);
                    }
                    break;

                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    else
                    {
                        base.WndProc(m: ref m);
                    }
                    break;

                default:
                    base.WndProc(m: ref m);
                    break;
            }
        }

        //void sogouWordDefWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    sogouWordDefWebBrowser.DocumentCompleted -= sogouWordDefWebBrowser_DocumentCompleted;
        //    sogouWordDefWebBrowser.DocumentText = sogouWordDefWebBrowser.Document.GetElementById("trans-rows-3").OuterHtml;
        //}

        private Boolean CheckTextValid(String cpText)
        {

            if (Chinese2EnglishEnabled)
            {
                var r = new Regex(@"[a-zA-Z\u4e00-\u9fa5\x3130-\x318F\xAC00-\xD7A3\u0800-\u4e00]");
                if (!r.IsMatch(cpText)) return false;
            }
            else
            {
                var r1 = new Regex(@"[a-zA-Z]");
                if (!r1.IsMatch(cpText)) return false;
                var pureEngText = Regex.Matches(cpText, @"[a-zA-Z]", RegexOptions.Multiline)
                     .Cast<Match>().Select(c => c.Value)
                     .Aggregate((a, b) => a + "" + b);
                if (pureEngText.Length * 2 < cpText.Length) return false;
            }

            List<Regex> rxs = new List<Regex>(4);
            //URL
            rxs.Add(new Regex(@"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~\(\)#?&//=]*)"));
            //Email
            rxs.Add(new Regex(@"^.+@.+$"));

            foreach (var rx in rxs)
            {
                Boolean match = rx.IsMatch(cpText);
                if (match)
                {
                    return false;
                }
            }
            return true;
        }

        private void WidthResizer()
        {
            int textLength;
            Graphics g = this.CreateGraphics();
            SizeF sizeF = g.MeasureString(tbClipboardText.Text, tbClipboardText.Font);
            g.Dispose();

            textLength = tbClipboardText.TextLength;
            if (textLength < 35)
            {
                this.Width = (int)Math.Ceiling(sizeF.Width * 1.06 + 8);
            }
            else if (textLength >= 35 && textLength < 200)
            {
                this.Width = 400;
            }
            else if (textLength >= 200 && textLength < 300)
            {
                this.Width = 450;
            }
            else if (textLength >= 300 && textLength < 400)
            {
                this.Width = 500;
            }
            else if (textLength >= 400 && textLength < 500)
            {
                this.Width = 550;
            }
            else
            {
                int tempWidth;
                tempWidth = textLength / 8 * 7;
                if (tempWidth > 600)
                {
                    this.Width = 600;
                }
                else
                {
                    this.Width = tempWidth;
                }
            }

            //textLength >= 35
            int numberOfLines = SendMessage(tbClipboardText.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);
            if (numberOfLines == 1)
            {
                Console.WriteLine("only one line");
                this.Width = (int)Math.Ceiling(sizeF.Width + 20);
            }

            Console.WriteLine(tbEditing);
            if (this.Width < 150 && !tbEditing)
            {
                this.miniEditNQuery.Visible = false;
            }
            else
            {
                this.miniEditNQuery.Visible = true;
            }
        }

        private void HeightResizer()
        {
            int numberOfLines = SendMessage(tbClipboardText.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);

            //restrict to 2/3 of the screen height
            if (titlePanel.Visible)
            {
                this.MaximumSize = new System.Drawing.Size(801, System.Windows.Forms.SystemInformation.VirtualScreen.Height * 2 / 3 + tbMarginTopBottom + titlePanelHeightMinus1);
                this.Height = Convert.ToInt32(tbClipboardText.Font.Height * 1.3) * numberOfLines + tbMarginTopBottom + titlePanelHeightMinus1;
            }
            else
            {
                this.MaximumSize = new System.Drawing.Size(801, System.Windows.Forms.SystemInformation.VirtualScreen.Height * 2 / 3 + tbMarginTopBottom);
                this.Height = Convert.ToInt32(tbClipboardText.Font.Height * 1.3) * numberOfLines + tbMarginTopBottom;
            }


            if (this.Height >= this.MaximumSize.Height)
            {
                tbClipboardText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            }
            else
            {
                tbClipboardText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            }

        }

        private string SogouPhoneticUrl = "";
        private string sogouSecret;
        string prettyText;
        private string lastPrettifiedQueryText = "";
        private string lastTranslatedText = "";
        private string GetTranslatedTextAsync(string sourceText)
        {
            //string rtfData;
            //rtfData = Clipboard.GetText(TextDataFormat.UnicodeText);
            //Console.WriteLine("rtfdata" + rtfData);

            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0";

                prettifyQueryText(sourceText);
                //do not repeat quering
                if (prettyText == lastPrettifiedQueryText)
                {
                    return lastTranslatedText;
                }
                lastPrettifiedQueryText = prettyText;

                var values = new NameValueCollection();
                values["from"] = "auto";
                values["to"] = "zh-CHS";
                values["client"] = "pc";
                values["fr"] = "browser_pc";
                values["useDetect"] = "on";
                values["useDetectResult"] = "on";
                values["needQc"] = "1";
                values["pid"] = "sogou-dict-vr";
                values["uuid"] = Guid.NewGuid().ToString();
                values["oxford"] = "on";
                values["isReturnSugg"] = "on";
                values["text"] = prettyText;
                values["s"] = CreateMD5("auto" + "zh-CHS" + prettyText + sogouSecret);

                var response = client.UploadValues(Resources.SogouTranslaterUrl, "POST", values);

                var responseString = Encoding.UTF8.GetString(response);
                JObject json = JObject.Parse(responseString);

                bool PronunciationValid = true;
                int wordsCount = sourceText.Trim().Split().Length;
                var PronunciationWords = sourceText.Trim().Split();
                if (wordsCount == 1 && PronunciationWords.All(s => s.Length <= 14))
                {
                    PronunciationValid = true;
                }
                else if (wordsCount > 1 && PronunciationWords.All(s => s.Length <= 16) && PronunciationWords.All(s => s.All(c => Char.IsLetter(c))))
                {
                    PronunciationValid = true;
                }
                else
                {
                    PronunciationValid = false;
                }

                if (PronunciationActivated && PronunciationValid)
                {

                    try
                    {
                        if (wordsCount == 1)
                        {
                            string PronunciationAccent = Settings.Default.PronunciationAccent;
                            string phoneticUrl;
                            var phoneticObj = json["data"]["common_dict"]["oxford"]["dict"].First()["content"].First()["phonetic"];
                            if (PronunciationAccent == "US")
                            {
                                phoneticUrl = (string)phoneticObj.Last()["filename"];
                            }
                            else if(PronunciationAccent == "RD")
                            {
                                var randgen = new Random();
                                phoneticUrl = (string)phoneticObj[randgen.Next(phoneticObj.Count())]["filename"];
                            }else
                            {
                                phoneticUrl = (string)phoneticObj.First()["filename"];
                            }
                            
                            SogouPhoneticUrl = "http:" + phoneticUrl;
                        }
                        else if (wordsCount <= 5 && wordsCount > 1)
                        {
                            Regex rgx = new Regex("[^a-zA-Z -]");
                            SogouPhoneticUrl = Resources.SogouAudioUrl + rgx.Replace(sourceText, " ");
                        }
                        Debug.WriteLine(SogouPhoneticUrl);
                    }
                    catch
                    {
                        // Do Nothing
                    }
                    //no audio fix for plural, participle etc.
                    finally
                    {
                        if (wordsCount <= 5 && wordsCount >= 1)
                        {
                            if (string.IsNullOrEmpty(SogouPhoneticUrl))
                            {
                                Regex rgx = new Regex("[^a-zA-Z -]");
                                SogouPhoneticUrl = Resources.SogouAudioUrl + rgx.Replace(sourceText, " ");
                            }
                        }
                    }
                }

                string translatedText = (string)json["data"]["translate"]["dit"];
                translatedText = translatedText.Trim();
                lastTranslatedText = translatedText;
                return translatedText;
            }
        }


        private void prettifyQueryText(string sourceText)
        {
            if (!tbEditing && !isEditedQuering)
            {
                prettyText = sourceText.Trim().Replace("\\s+", " ").Trim();
                // remove the quotation brakets
                // caution with \W for it'll match brackets also 
                // visualization https://www.debuggex.com/r/PKoiAw59R9z75pHa
                prettyText = Regex.Replace(prettyText, @"(?<!\b(to|of|at|in|by)[\s\n\r]+?|\.[\s\n\r]+?|^)[\[|\(][\d\W]*\d+\s?[\]|\)]|(?<=[,.)])((\d{1,2}(\W\d{1,2})*)(?=\s\w))", "");
                string[] sourceTextLines = prettyText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);


                double textLengthThreshold = Quartiles(sourceTextLines).Item1;

                string[] prettyTextLines = new String[sourceTextLines.Length];
                prettyText = "";
                Console.WriteLine($"item1:{Quartiles(sourceTextLines).Item1},item2:{Quartiles(sourceTextLines).Item2},item3:{Quartiles(sourceTextLines).Item3}");
                foreach (var item in sourceTextLines.Select((value, i) => new { i, value }))
                {
                    //Console.WriteLine($"{item.value.Substring(0, 8)}:{item.value.Length}");
                    //Console.WriteLine(System.Text.RegularExpressions.Regex.Escape(item.value));
                    //string trimedLine = item.value.TrimEnd((char[])"/n/r".ToCharArray());
                    //Console.WriteLine(System.Text.RegularExpressions.Regex.Escape(item.value));
                    if ((item.value.EndsWith(".") || item.value.EndsWith("。")) && item.i + 1 != sourceTextLines.Length && (item.value.Length < textLengthThreshold || item.value.Length > 150 ) || (sourceTextLines.Length > 1 && item.value.Length < textLengthThreshold / 2))
                    {
                        prettyText += item.value + "\r\n";
                    }
                    else
                    {
                        prettyText += item.value + " ";
                    }
                }
                prettyText = prettyText.Replace("\\s+", " ");
            }

            if (isEditedQuering)
            {
                prettyText = sourceText;
            }
        }

        private static Tuple<double, double, double> Quartiles(String[] asVal)
        {
            int[] afVal = asVal.Select(i => i.Length).ToArray();
            Array.Sort(afVal);
            int iSize = afVal.Length;
            int iMid = iSize / 2; //this is the mid from a zero based index, eg mid of 7 = 3;

            double fQ1 = 0;
            double fQ2 = 0;
            double fQ3 = 0;

            if (iSize % 2 == 0)
            {
                //================ EVEN NUMBER OF POINTS: =====================
                //even between low and high point
                fQ2 = (afVal[iMid - 1] + afVal[iMid]) / 2;

                int iMidMid = iMid / 2;

                //easy split 
                if (iMid % 2 == 0)
                {
                    fQ1 = (afVal[iMidMid - 1] + afVal[iMidMid]) / 2;
                    fQ3 = (afVal[iMid + iMidMid - 1] + afVal[iMid + iMidMid]) / 2;
                }
                else
                {
                    fQ1 = afVal[iMidMid];
                    fQ3 = afVal[iMidMid + iMid];
                }
            }
            else if (iSize == 1)
            {
                //================= special case, sorry ================
                fQ1 = afVal[0];
                fQ2 = afVal[0];
                fQ3 = afVal[0];
            }
            else
            {
                //odd number so the median is just the midpoint in the array.
                fQ2 = afVal[iMid];

                if ((iSize - 1) % 4 == 0)
                {
                    //======================(4n-1) POINTS =========================
                    int n = (iSize - 1) / 4;
                    fQ1 = (afVal[n - 1] * .25) + (afVal[n] * .75);
                    fQ3 = (afVal[3 * n] * .75) + (afVal[3 * n + 1] * .25);
                }
                else if ((iSize - 3) % 4 == 0)
                {
                    //======================(4n-3) POINTS =========================
                    int n = (iSize - 3) / 4;

                    fQ1 = (afVal[n] * .75) + (afVal[n + 1] * .25);
                    fQ3 = (afVal[3 * n + 1] * .25) + (afVal[3 * n + 2] * .75);
                }
            }

            Console.WriteLine(fQ1.ToString() + "-" + fQ2.ToString() + "-" + fQ3.ToString());

            return new Tuple<double, double, double>(fQ1, fQ2, fQ3);
        }

        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        private string tMessage = "复制翻译已开启\n双击我关闭";
        private void Startupballon(string tmsg)
        {
            this.hiddenNotifyIcon.BalloonTipText = tmsg;
            this.hiddenNotifyIcon.BalloonTipTitle = "Translit!";
            this.hiddenNotifyIcon.ShowBalloonTip(1000);
            this.hiddenNotifyIcon.Text = ("Translit! " + Resources.TranslitCurrentVersion + "\n开关快捷键：Alt+F1");
        }


        private string UserID(string secret)
        {
            String macAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(macAddress);
            System.Security.Cryptography.HMACMD5 cryptographer = new System.Security.Cryptography.HMACMD5(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private void CheckUpdate()
        {
            String userID;
            try
            {
                userID = UserID(Resources.Secret);
            }
            catch
            {
                userID = "ANONYMOUS";
            }

            //different network interfaces mapping to different userIDs
            //string recordedUserID = PreferenceConfigs.ReadSetting("userID") ?? "";
            //if (!recordedUserID.Contains(userID))
            //{
            //    PreferenceConfigs.WriteSetting("userID", (string.IsNullOrEmpty(recordedUserID) ? "" : (recordedUserID+",")) + userID);
            //}

            Settings.Default.userID =  Settings.Default.userID ?? new StringCollection();
            if (!Settings.Default.userID.Contains(userID))
            {
                Settings.Default.userID.Add(userID);
                Settings.Default.Save();
            }

            var request = WebRequest.Create($"{Resources.TranslitCheckUpdateUrl}?userid={UserID(Resources.Secret)}&version={Resources.TranslitCurrentVersion}");
            var response = request.GetResponse();

            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    responseString = reader.ReadToEnd().Trim();
                    if (responseString == "1")
                    {
                        var site = new ProcessStartInfo($"{Resources.TranslitAboutUrl}?userid={UserID(Resources.Secret)}&version={Resources.TranslitCurrentVersion}");
                        Process.Start(site);
                        tMessage = CheckMessage();
                    }
                }
            }

            try
            {
                sogouSecret = CheckSalt();
                Settings.Default.SogouSecret = sogouSecret;
                Settings.Default.Save();
            }
            catch
            {
                sogouSecret = Settings.Default.SogouSecret;
            }
        }

        private string CheckSalt()
        {
            var request = WebRequest.Create($"{Resources.SogouSaltUrl}?userid={UserID(Resources.Secret)}&version={Resources.TranslitCurrentVersion}");
            var response = request.GetResponse();

            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    responseString = reader.ReadToEnd().Trim();
                }
            }
            if (!response.ResponseUri.Host.Contains("translit"))
            {
                throw new WebException("没有有效的网络连接");
            }
            return responseString;
        }

        private string CheckMessage()
        {

            var request = WebRequest.Create(Resources.TranslitMessageUrl + UserID(Resources.Secret) + "&version=" + Resources.TranslitCurrentVersion);
            var response = request.GetResponse();

            if (!response.ResponseUri.Host.Contains("translit"))
            {
                return "没有有效的网络连接";
            }

            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    responseString = reader.ReadToEnd().Trim();
                }
            }
            return responseString;
        }

        private void TransForm_Deactivate(object sender, EventArgs e)
        {
            //this.ActiveControl = textBoxPanel;
            wordsLengthUpperLimit.Visible = false;
            tbContextMenu.Hide();
            tbClipboardText.Select(0, 0);
            if (!alwaysOnTop)
            {
                if (titlePanel.Visible) this.Top += titlePanelHeightMinus1;
                titlePanel.Visible = false;
                tableLayoutPanel.RowStyles[0].Height = 0;
                tableLayoutPanel.RowStyles[1].Height = 2;
                this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                //shrink to prevent flash
                //this.Width = 30;
                //this.Height = 30;
                WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void TransForm_VisibleChanged(object sender, EventArgs e)
        {
            if (tbEditing)
            {
                miniEditNQuery.Image = Resources.mini_edit;
                tbClipboardText.BackColor = System.Drawing.Color.White;
                tbClipboardText.ReadOnly = true;
                //this.ActiveControl = titlePanel;

                //drift away if not switch to false
                //tbEditing phase switching should be in front of tbClipboardText changing
                tbEditing = false;
                tbClipboardText.Text = tbBeforeEdText;
                //WidthResizer();
                edToolStripMenuItem.Visible = true;
                edExitToolStripMenuItem.Visible = false;
            }
            //todo
        }

        private Boolean initialRun = true;
        private void TransForm_Load(object sender, EventArgs e)
        {
            //this.Height = 500;
            //this.MinimumSize = new System.Drawing.Size(62, this.Height);
            titlePanel.Visible = false;
            tableLayoutPanel.RowStyles[0].Height = 0;
            tableLayoutPanel.RowStyles[1].Height = 2;
            this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            this.Top = SystemInformation.VirtualScreen.Height / 10;
            this.Left = SystemInformation.VirtualScreen.Width / 20;
            WindowState = FormWindowState.Minimized;
            this.Hide();
            initialRun = false;
            this.iconContextMenu.Renderer = new MyRenderer();
            this.tbContextMenu.Renderer = new MyRenderer();
        }

        public Boolean ready2move = false;
        public Point mouseDownPoint;
        private void titlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            //this.ActiveControl = titlePanel;
            if (e.Button == MouseButtons.Left)
            {
                titlePanel.Cursor = Cursors.SizeAll;
            }
        }

        private void titlePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.15;
                    toolTip.SetToolTip(titlePanel, "透明度：" + (1.0-this.Opacity).ToString("P0"));
                }              
            }
            else
            {
                if (this.Opacity > 0.2)
                {
                    this.Opacity -= 0.08;
                    toolTip.SetToolTip(titlePanel, "透明度：" + (1.0 - this.Opacity).ToString("P0"));
                }
            }
        }

        private void titlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!ready2move)
            {
                mouseDownPoint = e.Location;
            }

            if (e.Button == MouseButtons.Left)
            {
                ready2move = true;
                //label1.Text = String.Format("{0} :: {1}", e.X - mouseDownPoint.X, e.Y - mouseDownPoint.Y);
                this.Left = e.X + this.Left - mouseDownPoint.X;
                this.Top = e.Y + this.Top - mouseDownPoint.Y;

                titlePanel.Cursor = Cursors.SizeAll;
            }
            else
            {
                ready2move = false;
                titlePanel.Cursor = Cursors.Arrow;
            }

        }


        private void titlePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (titlePanel.Visible) this.Top += titlePanelHeightMinus1;
                titlePanel.Visible = false;
                wordsLengthUpperLimit.Visible = false;

                miniClose.Image = Resources.mini_close;
                miniPin.Image = Resources.mini_sprig;
                this.TopMost = false;
                alwaysOnTop = false;
                miniHide.Visible = false;
                tableLayoutPanel.RowStyles[0].Height = 0;
                tableLayoutPanel.RowStyles[1].Height = 2;
                this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                //shrink to prevent flash
                //this.Width = 30;
                //this.Height = 30;
                WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void titlePanel_MouseEnter(object sender, EventArgs e)
        {
            if (!tbEditing) titlePanel.Focus();
        }

        private void titlePanel_MouseLeave(object sender, EventArgs e)
        {

        }

        private void titlePanel_VisibleChanged(object sender, EventArgs e)
        {
            if (titlePanel.Visible)
            {
                this.MaximumSize = new System.Drawing.Size(801, System.Windows.Forms.SystemInformation.VirtualScreen.Height * 2 / 3 + tbMarginTopBottom + titlePanelHeightMinus1);
            }
            else
            {
                this.MaximumSize = new System.Drawing.Size(801, System.Windows.Forms.SystemInformation.VirtualScreen.Height * 2 / 3 + tbMarginTopBottom);
            }
        }

        private void tbClipboardText_MouseMove(object sender, MouseEventArgs e)
        {
            if (!ready2move)
            {
                mouseDownPoint = e.Location;
            }

            if (e.Button == MouseButtons.Middle)
            {
                ready2move = true;
                this.Left = e.X + this.Left - mouseDownPoint.X;
                this.Top = e.Y + this.Top - mouseDownPoint.Y;
                Console.WriteLine($"this.Left:{this.Left}, e.X:{e.X}, mouseDownPoint.X:{mouseDownPoint.X}");

                tbClipboardText.Cursor = Cursors.SizeAll;
            }
            else if (e.Button == MouseButtons.Left || tbEditing)
            {
                tbClipboardText.Cursor = Cursors.IBeam;
            }
            else
            {
                ready2move = false;
                tbClipboardText.Cursor = Cursors.Arrow;
            }
        }

        private void tbClipboardText_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                tbClipboardText.Cursor = Cursors.SizeAll;
            }else if (e.Button == MouseButtons.Left)
            {
                tbClipboardText.Cursor = Cursors.IBeam;
            }else if (e.Button == MouseButtons.Right)
            {
                if (tbClipboardText.SelectionLength > 0)
                {
                    cpToolStripMenuItem.Visible = true;
                }
                else
                {
                    cpToolStripMenuItem.Visible = false;
                }

                //prettyText is NULL in inital
                if (String.IsNullOrEmpty(prettyText))
                {
                    toolStripSeparator4.Visible = false;
                    edToolStripMenuItem.Visible = false;
                }
                else
                {
                    if (tbEditing)
                    {
                        edToolStripMenuItem.Visible = false;
                        cpSrcToolStripMenuItem.Visible = true;
                        cpAllToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        edToolStripMenuItem.Visible = true;
                        cpSrcToolStripMenuItem.Visible = false;
                        cpAllToolStripMenuItem.Visible = true;
                    }
                }
            }
        }


        private void tbClipboardText_MouseUp(object sender, MouseEventArgs e)
        {
            tbClipboardText.Cursor = Cursors.Arrow;
        }

        private Boolean tbClipboardTextSelected()
        {
            return (tbClipboardText.SelectedText != null && tbClipboardText.SelectedText != "");
        }

        private void tbClipboardText_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = tbClipboardText;

            if (!titlePanel.Visible)
            {
                this.SuspendLayout();
                //should before the height change for the maximum is rely on the tilepanel visible or not
                titlePanel.Visible = true;
                this.Top -= titlePanelHeightMinus1;
                this.Height += titlePanelHeightMinus1;
                tableLayoutPanel.RowStyles[0].Height = 30;
                tableLayoutPanel.RowStyles[1].Height = 1;
                this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
                this.ResumeLayout();
            }
        }


        private void tbClipboardText_MouseLeave(object sender, EventArgs e)
        {
            tbClipboardText.SelectionLength = 0;
            this.ActiveControl = titlePanel;
        }

        //private void tbClipboardText_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    if (e.Delta > 0)
        //    {
        //        if (this.Opacity < 1)
        //        {
        //            this.Opacity += 0.15;
        //            toolTip.SetToolTip(tbClipboardText, "透明度：" + (1.0 - this.Opacity).ToString("P0"));
        //        }
        //    }
        //    else
        //    {
        //        if (this.Opacity > 0.2)
        //        {
        //            this.Opacity -= 0.08;
        //            toolTip.SetToolTip(tbClipboardText, "透明度：" + (1.0 - this.Opacity).ToString("P0"));
        //        }
        //    }
        //}

        //a tricky way to move the blinking curser and retain the black color
        private void tbClipboardText_EnabledChanged(object sender, EventArgs e)
        {
            //if (SogouPhoneticUrl.Length > 0)
            //{
            //    try
            //    {
            //        var wplayer = new WMPLib.WindowsMediaPlayer();
            //        wplayer.URL = SogouPhoneticUrl;
            //        SogouPhoneticUrl = "";
            //        wplayer.controls.play();
            //    }
            //    catch
            //    {
            //        //pass
            //    }
            //}
        }


        private Boolean tbEditing = false;
        private void tbClipboardText_TextChanged(object sender, EventArgs e)
        {
            //tbClipboardText.SelectionIndent = (int)(sizeF.Width / 10 * 2);
            tbIsHide = false;
            this.Opacity = 1;
            if (!tbEditing)
            {
                //this.ActiveControl = textBoxPanel;

                if (prettyText.Length > 4000)
                {
                    wordsLengthUpperLimit.Text = $"({prettyText.Length} / 5000)";
                    wordsLengthUpperLimit.Visible = true;
                }
                WidthResizer();
                HeightResizer();

                //autoscroll after scrollbar shows up
                //if (alwaysOnTop)
                //{
                //    if (tbClipboardText.ScrollBars != RichTextBoxScrollBars.None)
                //    {
                //        tbClipboardText.SelectionStart = tbClipboardText.Text.Length;
                //        tbClipboardText.ScrollToCaret();
                //    }
                //}
                this.Show();
            }
            else if (tbEditing && tbClipboardText.TextLength > 4000)
            {
                wordsLengthUpperLimit.Text = $"({tbClipboardText.TextLength} / 5000)";
                wordsLengthUpperLimit.Visible = true;
            }

            WindowState = FormWindowState.Normal;
        }

        private void tbClipboardText_SizeChanged(object sender, EventArgs e)
        {
        }


        private void miniEditNQuery_MouseEnter(object sender, EventArgs e)
        {
            if (tbEditing)
            {
                miniEditNQuery.Image = Resources.mini_query_hover;
            }
            else
            {
                miniEditNQuery.Image = Resources.mini_edit_hover;
            }
        }

        private void miniEditNQuery_MouseHover(object sender, EventArgs e)
        {
            if (!isEditedQuering && !isQueryFinishedFeeding)
            {
                if (tbEditing)
                {
                    toolTip.SetToolTip(miniEditNQuery, "提交翻译");
                }
                else
                {
                    toolTip.SetToolTip(miniEditNQuery, "编辑原文");
                }
            }
        }


        private void miniEditNQuery_MouseLeave(object sender, EventArgs e)
        {
            if (!isQueryFinishedFeeding && !isEditedQuering)
            {
                if (tbEditing)
                {
                    miniEditNQuery.Image = Resources.mini_query;
                }
                else
                {
                    miniEditNQuery.Image = Resources.mini_edit;
                }
            }
        }

        private Boolean isQueryFinishedFeeding = false;
        private Boolean isEditedQuering;
        private async void miniEditNQuery_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbClipboardText.ScrollBars != RichTextBoxScrollBars.None)
            {
                //scroll to top
                tbClipboardText.SelectionStart = 0;
                tbClipboardText.ScrollToCaret();
            }
            if (tbEditing)
            {
                isEditedQuering = true;
                _ = Task.Factory.StartNew(() =>
                {
                    List<Image> queringLoadingImages = new List<Image>
                    {
                        Resources.mini_query_a,
                        Resources.mini_query_b,
                        Resources.mini_query_c,
                        Resources.mini_query
                    };

                    int index = 0;
                    while (isEditedQuering)
                    {
                        miniEditNQuery.Image = queringLoadingImages[index];
                        Thread.Sleep(100);
                        index += 1;
                        if (index == 3) index = 0;

                        //miniEditNQuery.Image = Resources.mini_query_b;
                        //Thread.Sleep(100);

                        //miniEditNQuery.Image = Resources.mini_query_c;
                        //Thread.Sleep(100);

                        //miniEditNQuery.Image = Resources.mini_query;
                        //Thread.Sleep(100);
                    }
                });

                lastOriginalText = tbClipboardText.Text;

                //miniEditNQuery.Image = Resources.mini_edit;
                tbClipboardText.BackColor = System.Drawing.Color.White;
                tbClipboardText.ReadOnly = true;
                //this.ActiveControl = titlePanel;
                //WidthResizer();
                edToolStripMenuItem.Visible = true;
                edExitToolStripMenuItem.Visible = false;
                
                //tbEditing phase switching should be in front of tbClipboardText changing
                tbEditing = false;
                if (!String.IsNullOrEmpty(tbClipboardText.Text) && !String.IsNullOrWhiteSpace(tbClipboardText.Text))
                {   
                    try
                    {
                        string s = tbClipboardText.Text;
                        tbClipboardText.Text = await Task.Run(() => GetTranslatedTextAsync(s));
                    }
                    catch (WebException)
                    {
                        tbClipboardText.Text = "网络连接失败，请检查您的网络设置";
                    }
                }
                else
                {
                    tbClipboardText.Text = tbBeforeEdText;
                }
                isEditedQuering = false;

                await Task.Factory.StartNew(() =>
                {
                    isQueryFinishedFeeding = true;
                    miniEditNQuery.Image = Resources.mini_ok_a;
                    Thread.Sleep(100);
                    miniEditNQuery.Image = Resources.mini_ok_b;
                    Thread.Sleep(50);
                    miniEditNQuery.Image = Resources.mini_ok_c;
                    Thread.Sleep(50);
                    miniEditNQuery.Image = Resources.mini_ok;
                    Thread.Sleep(800);
                    miniEditNQuery.Image = Resources.mini_ok_y;
                    Thread.Sleep(50);
                    miniEditNQuery.Image = Resources.mini_ok_z;
                    Thread.Sleep(50);
                    if (tbEditing)
                    {
                        miniEditNQuery.Image = Resources.mini_query;
                    }
                    else
                    {
                        miniEditNQuery.Image = Resources.mini_edit;
                    }
                    isQueryFinishedFeeding = false;
                });
            }
            else
            {
                tbBeforeEdText = tbClipboardText.Text;
                miniEditNQuery.Image = Resources.mini_query;
                tbClipboardText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
                tbClipboardText.ReadOnly = false;
                //this.ActiveControl = tbClipboardText;
                //WidthResizer();
                edToolStripMenuItem.Visible = false;
                edExitToolStripMenuItem.Visible = true;

                //tbEditing phase switching should be in front of tbClipboardText changing for
                //tbClipboardText_textchange event using tbeding to check if need to resize,
                //so once miniEditNquery is clicked the tbeding is always true and textchange says no need to resize
                tbEditing = true;
                tbClipboardText.Text = prettyText;
                //I prefer not to resize the width when simply swich between eding and not
                HeightResizer();
            }
        }

        

        private Boolean tbIsHide = false;
        private void miniHide_MouseHover(object sender, EventArgs e)
        {
            if (tbIsHide)
            {
                toolTip.SetToolTip(miniHide, "展开");
            }
            else
            {
                toolTip.SetToolTip(miniHide, "收起");
            }
        }


        private void miniHide_MouseLeave(object sender, EventArgs e)
        {
            if (tbIsHide)
            {
                miniHide.Image = Resources.mini_down;
            }
            else
            {
                miniHide.Image = Resources.mini_up;
            }
        }

        private void miniHide_MouseEnter(object sender, EventArgs e)
        {
            if (tbIsHide)
            {
                miniHide.Image = Resources.mini_down_hover;
            }
            else
            {
                miniHide.Image = Resources.mini_up_hover;
            }
        }

        private void miniHide_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbIsHide)
            {
                miniHide.Image = Resources.mini_up;
                HeightResizer();
                tbIsHide = false;
            }
            else
            {
                miniHide.Image = Resources.mini_down;
                this.Height = titlePanelHeightMinus1 + 2;
                tbIsHide = true;
            }
        }

        private void miniPin_MouseHover(object sender, EventArgs e)
        {  
            if (!alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_hover;
                toolTip.SetToolTip(miniPin, "点击置顶（多选翻译）");
            }
            else if (alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_down_hover;
                if (tbEditing)
                {
                    toolTip.SetToolTip(miniPin, "点击取消置顶\n编辑中，多选翻译已禁用");
                }
                else
                {
                    toolTip.SetToolTip(miniPin, "点击取消置顶\n多选翻译已开启");
                }
            }
        }

        private void miniPin_MouseLeave(object sender, EventArgs e)
        {
            if (!alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig;
            }else if (alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_down;
            }
        }

        private void miniPin_MouseEnter(object sender, EventArgs e)
        {
            if (!alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_hover;
            }
            else if (alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_down_hover;
            }
        }

        private void miniPin_MouseClick(object sender, MouseEventArgs e)
        {
            if (!alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_down;
                this.TopMost = true;
                alwaysOnTop = true;
                miniHide.Visible = true;
                miniHide.Image = Resources.mini_up;
            }
            else if (alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig;
                this.TopMost = false;
                alwaysOnTop = false;
                miniHide.Visible = false;
            }
        }

        private void miniPin_MouseDown(object sender, MouseEventArgs e)
        {
            //this.ActiveControl = miniPin;
            miniPin.Image = Resources.mini_sprig_down;
        }

        private void miniPin_MouseUp(object sender, MouseEventArgs e)
        {
            if (!alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_hover;
            }
            else if (alwaysOnTop)
            {
                miniPin.Image = Resources.mini_sprig_down_hover;
            }
        }

        private void miniClose_MouseEnter(object sender, EventArgs e)
        {
            miniClose.Image = Resources.mini_close_hover;
        }

        private void miniClose_MouseLeave(object sender, EventArgs e)
        {
            miniClose.Image = Resources.mini_close;
        }

        private void miniClose_MouseHover(object sender, EventArgs e)
        {
            miniClose.Image = Resources.mini_close_hover;
            toolTip.SetToolTip(miniClose, "关闭");
        }

        private void miniClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (titlePanel.Visible) this.Top += titlePanelHeightMinus1;
            titlePanel.Visible = false;
            wordsLengthUpperLimit.Visible = false;

            miniClose.Image = Resources.mini_close;
            miniPin.Image = Resources.mini_sprig;
            this.TopMost = false;
            alwaysOnTop = false;
            miniHide.Visible = false;
            tableLayoutPanel.RowStyles[0].Height = 0;
            tableLayoutPanel.RowStyles[1].Height = 2;
            this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            //shrink to prevent flash
            //this.Width = 30;
            //this.Height = 30;
            WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void miniClose_MouseDown(object sender, MouseEventArgs e)
        {
            //this.ActiveControl = miniClose;
        }

        private void LogoPicture_MouseDown(object sender, MouseEventArgs e)
        {
            //this.ActiveControl = LogoPicture;
        }

        private void tbClipboardText_MouseClick(object sender, MouseEventArgs e)
        {

        }

        //private Size originalSize;
        //private void textBoxPanel_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.X > textBoxPanel.Width - 7 && e.Y > textBoxPanel.Height - 7)
        //    {
        //        textBoxPanel.Cursor = Cursors.SizeNWSE;
        //    }
        //    else if (e.X > textBoxPanel.Width - 3 && e.Y <= textBoxPanel.Height - 7)
        //    {
        //        textBoxPanel.Cursor = Cursors.SizeWE;
        //    }
        //    else if (e.X < textBoxPanel.Width - 7 && e.Y > textBoxPanel.Height - 3 && e.X > 7)
        //    {
        //        textBoxPanel.Cursor = Cursors.SizeNS;
        //    }
        //    else if (e.X < 3 && e.Y < textBoxPanel.Height - 7)
        //    {
        //        textBoxPanel.Cursor = Cursors.SizeWE;
        //    }
        //    else if (e.X <= 7 && e.Y >= textBoxPanel.Height - 7)
        //    {
        //        textBoxPanel.Cursor = Cursors.SizeNESW;
        //    }
        //    else
        //    {
        //        textBoxPanel.Cursor = Cursors.Arrow;
        //    }

        //    if (!ready2move)
        //    {
        //        mouseDownPoint = e.Location;
        //        originalSize = this.Size;
        //    }

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        ready2move = true;
        //        if (e.X >= textBoxPanel.Width - 8 && e.Y >= textBoxPanel.Height - 8)
        //        {
        //            this.Height = textBoxPanel.Top + e.Y;
        //            this.Width = textBoxPanel.Left + e.X;
        //        }
        //        else if (e.X > textBoxPanel.Width - 8 && e.Y < textBoxPanel.Height - 8)
        //        {
        //            this.Width = textBoxPanel.Left + e.X;
        //        }
        //        else if (e.X < textBoxPanel.Width - 8 && e.Y > textBoxPanel.Height - 8 && e.X > 8)
        //        {
        //            this.Height = textBoxPanel.Top + e.Y;
        //        }
        //        else if (e.X < 8 && e.Y < textBoxPanel.Height - 8)
        //        {
        //            if (this.Width > this.MinimumSize.Width || e.X < textBoxPanel.Left)
        //            {
        //                this.Width = textBoxPanel.Right - e.X;
        //                this.Left -= textBoxPanel.Left - e.X;
        //            }
        //        }
        //        else if (e.X < 8 && e.Y >= textBoxPanel.Height - 8)
        //        {
        //            if (this.Width > this.MinimumSize.Width || e.X < textBoxPanel.Left)
        //            {
        //                this.Width = textBoxPanel.Right - e.X;
        //                this.Left -= textBoxPanel.Left - e.X;
        //                this.Height = textBoxPanel.Top + e.Y;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ready2move = false;
        //    }
        //}

        private Size originalSize;
        private Boolean ready2Resize;
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control ctl = sender as Control;
            Point loc = this.PointToClient(ctl.PointToScreen(e.Location));

            //Console.WriteLine($"loc.x:{loc.X},loc.y:{loc.Y},this.left:{this.Left},this.top:{this.Top},this.width:{this.Width},this.height:{this.Height},e.x:{e.X},e.y:{e.Y}");

            if (loc.X > this.Width - 7 && loc.Y > this.Height - 7)
            {
                this.Cursor = Cursors.SizeNWSE;
            }
            else if (loc.X > this.Width - 3 && loc.Y <= this.Height - 7)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (loc.X < this.Width - 7 && loc.Y > this.Height - 3 && loc.X > 7)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else if (loc.X < 3 && loc.Y < this.Height - 7)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (loc.X <= 7 && loc.Y >= this.Height - 7)
            {
                this.Cursor = Cursors.SizeNESW;
            }
            else
            {
                if(!ready2move) this.Cursor = Cursors.Arrow;
            }

            
            if (e.Button == MouseButtons.Left)
            {
                ready2Resize = true;
                //the commented way is too lag
                //if (loc.X >= this.Width - 8 && loc.Y >= this.Height - 8)
                if (resizingOrientation == 4)
                {
                    this.Height = loc.Y;
                    this.Width = loc.X;
                }
                else if (resizingOrientation == 5)
                {
                    this.Width = loc.X;
                }
                else if (resizingOrientation == 3)
                {
                    this.Height = loc.Y;
                }
                else if (resizingOrientation == 1)
                {
                    if (this.Width > this.MinimumSize.Width || loc.X < 0)
                    {
                        this.Width -= loc.X;
                        this.Left += loc.X;
                    }
                }
                else if (resizingOrientation == 2)
                {
                    if (this.Width > this.MinimumSize.Width || loc.X < 0)
                    {
                        this.Width -= loc.X;
                        this.Left += loc.X;
                        this.Height = loc.Y;
                    }
                }

                if (this.Width < 150 && !tbEditing)
                {
                    this.miniEditNQuery.Visible = false;
                }
                else
                {
                    this.miniEditNQuery.Visible = true;
                }

                this.ResumeLayout();
            }
            else
            {
                ready2Resize = false;
            }
        }

        private int resizingOrientation;
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Control ctl = sender as Control;
            Point loc = this.PointToClient(ctl.PointToScreen(e.Location));

            //orientation 
            //  |1 0 5|
            //  |1 0 5|
            //  |2 3 4|
            if (loc.X < 3 && loc.Y < this.Height - 7)
            {
                this.Cursor = Cursors.SizeWE;
                resizingOrientation = 1;
            }
            else if (loc.X <= 7 && loc.Y >= this.Height - 7)
            {
                this.Cursor = Cursors.SizeNESW;
                resizingOrientation = 2;
            }
            else if (loc.X < this.Width - 7 && loc.Y > this.Height - 3 && loc.X > 7)
            {
                this.Cursor = Cursors.SizeNS;
                resizingOrientation = 3;
            }
            else if (loc.X > this.Width - 7 && loc.Y > this.Height - 7)
            {
                this.Cursor = Cursors.SizeNWSE;
                resizingOrientation = 4;
            }
            else if (loc.X > this.Width - 3 && loc.Y <= this.Height - 7)
            {
                this.Cursor = Cursors.SizeWE;
                resizingOrientation = 5;
            }
            else
            {
                resizingOrientation = 0;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            resizingOrientation = 0;
        }

        private string tbBeforeEdText;
        private void edToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbBeforeEdText = tbClipboardText.Text;

            miniEditNQuery.Image = Resources.mini_query;
            tbClipboardText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            tbClipboardText.ReadOnly = false;
            //this.ActiveControl = tbClipboardText;
            
            //WidthResizer();
            edToolStripMenuItem.Visible = false;
            edExitToolStripMenuItem.Visible = true;

            //tbEditing phase switching should be in front of tbClipboardText changing
            tbEditing = true;
            tbClipboardText.Text = prettyText;
        }

        private void edExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            miniEditNQuery.Image = Resources.mini_edit;
            tbClipboardText.BackColor = System.Drawing.Color.White;
            tbClipboardText.ReadOnly = true;
            //this.ActiveControl = titlePanel;

            //WidthResizer();
            edToolStripMenuItem.Visible = true;
            edExitToolStripMenuItem.Visible = false;

            //tbEditing phase switching should be in front of tbClipboardText changing
            tbEditing = false;
            tbClipboardText.Text = tbBeforeEdText;
        }

        private Boolean cpInTranslit = false;
        private void cpAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cpInTranslit = true;
            Clipboard.SetText(tbClipboardText.Text);
            cpInTranslit = false;
        }

        private void cpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tbClipboardText.SelectionLength > 0)
            {
                cpInTranslit = true;
                Clipboard.SetText(tbClipboardText.SelectedText);
                cpInTranslit = false;
            }
        }

        private void tbClipboardText_VisibleChanged(object sender, EventArgs e)
        {
            if (SogouPhoneticUrl.Length > 0)
            {
                try
                {
                    var wplayer = new WMPLib.WindowsMediaPlayer();
                    wplayer.URL = SogouPhoneticUrl;
                    SogouPhoneticUrl = "";
                    wplayer.controls.play();
                }
                catch
                {
                    //pass
                }
            }
        }

        private bool TranslitActivated = true;
        private void iCbonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.iCbonToolStripMenuItem.Checked = !this.iCbonToolStripMenuItem.Checked;
            TranslitActivated = !TranslitActivated;
            if (TranslitActivated) { hiddenNotifyIcon.Icon = Resources.translit; }
            else { hiddenNotifyIcon.Icon = Resources.translit_bw; }
        }

        //private bool PronunciationActivated = Convert.ToBoolean(PreferenceConfigs.ReadSetting("PronunciationActivated"));
        //private string PronunciationAccent = PreferenceConfigs.ReadSetting("PronunciationAccent") ?? PreferenceConfigs.WriteSetting("PronunciationAccent", "US");

        private void iPnonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.iPnonToolStripMenuItem.Checked = !this.iPnonToolStripMenuItem.Checked;
            PronunciationActivated = !PronunciationActivated;
            Settings.Default.PronunciationActivated = PronunciationActivated;
            Settings.Default.Save();
        }


        //private bool Chinese2EnglishEnabled = Convert.ToBoolean(PreferenceConfigs.ReadSetting("Chinese2EnglishEnabled"));
        private void iCeonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.iCeonToolStripMenuItem.Checked = !this.iCeonToolStripMenuItem.Checked;
            Chinese2EnglishEnabled = !Chinese2EnglishEnabled;
            Settings.Default.Chinese2EnglishEnabled = Chinese2EnglishEnabled;
            Settings.Default.Save();
        }

        private void iShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.ActiveControl = textBoxPanel;
            this.Show();
            WindowState = FormWindowState.Normal;
            //var numberOfLines = SendMessage(tbClipboardText.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);
            //this.Height = Convert.ToInt32(tbClipboardText.Font.Height * 1.25) * numberOfLines + 16;
            WidthResizer();
            HeightResizer();
        }

        private void iAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void iExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        //flat contentMenuStripRender
        public class MyColorTable : ProfessionalColorTable
        {
            public override Color MenuItemBorder
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color MenuItemSelected
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Color.White; }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return Color.White; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return Color.White; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return Color.White; }
            }
        }

        public class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer()
                : base(new MyColorTable())
            {
            }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var r = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
                r.Inflate(-2, -6);
                e.Graphics.DrawLines(Pens.Black, new Point[]{
                    new Point(r.Left, r.Top),
                    new Point(r.Right, r.Top + r.Height / 2),
                    new Point(r.Left, r.Top + r.Height)});
            }

            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var r = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                r.Inflate(-4, -6);
                e.Graphics.DrawLines(Pens.Black, new Point[]{
                    new Point(r.Left, r.Bottom - r.Height /2),
                    new Point(r.Left + r.Width /3,  r.Bottom),
                    new Point(r.Right, r.Top)});
            }
        }
        //flat contentMenuStripRender end


        //little bit dirtty to be transparent
        private void HookMouseMove(Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                ctl.MouseEnter += OnMouseEnter;
                ctl.MouseLeave += OnMouseLeave;
                ctl.MouseMove += OnMouseMove;
                ctl.MouseDown += OnMouseDown;
                ctl.MouseUp += OnMouseUp;
                HookMouseMove(ctl.Controls);
            }
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            //enter whatever miniclose or titlepanel
            if (tbIsHide) this.Opacity = 1;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            try
            {
                string objname = ((Panel)sender).Name;
                if (objname == "titlePanel" || objname == "splitPanel")
                {
                    if (tbIsHide) this.Opacity = 0.5;
                }
            }
            catch { }
        }


        //private void button1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    // Call the nextline whenever you want to execute your code
        //    //sogouWordDefWebBrowser.Document.InvokeScript("applyChanges");

        //    HtmlElement headElement = wordDefWebBrowser.Document.GetElementsByTagName("head")[0];
        //    HtmlElement scriptElement = wordDefWebBrowser.Document.CreateElement("script");
        //    IHTMLScriptElement domScriptElement = (IHTMLScriptElement)scriptElement.DomElement;
        //    //$('head').append('<link rel=""stylesheet"" href=""//translit.tk/share/blue_stylesheet.css"" type=""text/css""/>');
        //    domScriptElement.text = @"function applyChanges()
        //                            { 
        //                                $('link[rel=stylesheet]')[0].disabled=true;

        //                                $('#header-fixed').remove(); 
        //                                $('#goTop').remove(); 
        //                                $('.btn-tips').remove(); 
        //                                $('body >').hide(); 
        //                                $('#trans-rows-3').show().appendTo('body'); 
        //                                $('.J-dict-usual').css({'width':'100%','border-style':'none','box-shadow':'none'}); 
        //                                var $defTitle = $('.for-oxford').clone().attr('id', 'simple-def-title');
        //                                $('.box-to-text').prepend($defTitle);
        //                                $('#simple-def-title span').text('单词释义');
        //                                $('.to-text-title').remove();
        //                                $('.box-to-text').css({'padding':'0'}); 
        //                                $('.for-oxford').css({'padding-top':'0'});
        //                                $('.J-dict-detail-content').css({'width':'100%'});
        //                            }";
        //    headElement.AppendChild(scriptElement);

        //    string css = Resources.blue_stylesheet;

        //    HTMLDocument CurrentDocument = (HTMLDocument)wordDefWebBrowser.Document.DomDocument;
        //    IHTMLStyleSheet styleSheet = CurrentDocument.createStyleSheet("", 0);
        //    styleSheet.cssText = css;

        //    wordDefWebBrowser.Document.InvokeScript("applyChanges");
        //}
    }

    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

    //FIXME: privilige issues  
    //public class ConfigIO
    //{
    //    public string ReadSetting(string key)
    //    {
    //        try
    //        {
    //            var appSettings = ConfigurationManager.AppSettings;
    //            string result = appSettings[key];
    //            return result;
    //        }
    //        catch (ConfigurationErrorsException)
    //        {
    //            return null;
    //        }
    //    }

    //    public string WriteSetting(string key, string value)
    //    {
    //        try
    //        {
    //            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    //            var settings = configFile.AppSettings.Settings;
    //            if (settings[key] == null)
    //            {
    //                settings.Add(key, value);
    //            }
    //            else
    //            {
    //                settings[key].Value = value;
    //            }
    //            configFile.Save(ConfigurationSaveMode.Modified);
    //            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
    //            return value;
    //        }
    //        catch (ConfigurationErrorsException)
    //        {
    //            return null;
    //        }
    //    }
    //}

}