using System;
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
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using translit.Properties;

namespace translit
{
    public partial class TransForm : Form
    {
        #region fields

        private IntPtr _clipboardViewerNext;

        string s = string.Format(Resources.SogouTranslaterUrl, 0);

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;

        #endregion

        #region WinApi 32 functions definition

        [DllImport(dllName: "User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport(dllName: "user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion

        #region Constructors

        public TransForm()
        {
            CheckUpdate();
            InitializeComponent();
            _clipboardViewerNext = SetClipboardViewer(hWndNewViewer: this.Handle);
            //WindowState = FormWindowState.Minimized;
            Startupballon();

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem4.Checked = true;

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem2 });
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem3 });
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem4 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize menuItem2
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "&Show";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            // Initialize menuItem3
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "&About";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);

            // Initialize menuItem4
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "&Popup activated";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            this.menuItem4.Checked = true;

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            hiddenNotifyIcon.ContextMenu = this.contextMenu1;

            // Handle the DoubleClick event to activate the form.
            hiddenNotifyIcon.DoubleClick += new System.EventHandler(this.hiddenNotifyIcon_DoubleClick);
        }

        private void hiddenNotifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void menuItem2_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        
        private void menuItem3_Click(object Sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private bool TranslitActivated = true;
        private void menuItem4_Click(object Sender, EventArgs e)
        {
            this.menuItem4.Checked = !this.menuItem4.Checked;
            TranslitActivated = !TranslitActivated;
        }

        #endregion

        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        public long lastPostTime = 0;
        protected override void WndProc(ref Message m)
        {
            const int wmDrawclipboard = 0x308;
            switch (m.Msg)
            {
                case wmDrawclipboard:
                    if (Clipboard.ContainsText() && TranslitActivated)
                    {
                        long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        
                        if (currentTime - lastPostTime > 1000)
                        {
                            lastPostTime = currentTime;
                            tbClipboardText.Text = GetTranslatedText(Clipboard.GetText());
                            tbClipboardText.Enabled = false;
                        }

                        this.Show();

                        var testLength = tbClipboardText.Text.Length;
                        if (testLength < 10)
                        {
                            this.Width = (testLength/2 + 5) * 20;
                        }
                        else if (testLength >= 10 && testLength < 20)
                        {
                            this.Width = 300;
                        }
                        else if (testLength >= 20 && testLength < 200)
                        {
                            this.Width = 400;
                        }
                        else if (testLength >= 200 && testLength < 300)
                        {
                            this.Width = 400;
                        }
                        else if(testLength >= 300 && testLength < 500)
                        {
                            this.Width = 450;
                        }
                        else
                        {
                            this.Width = testLength / 4 * 3;
                        }

                        var numberOfLines = SendMessage(tbClipboardText.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0) + 1;
                        this.Height = (tbClipboardText.Font.Height + 1) * numberOfLines;

                        WindowState = FormWindowState.Normal;
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

        private string GetTranslatedText(string sourceText)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["from"] = "auto";
                values["to"] = "zh-CHS";
                values["client"] = "pc";
                values["fr"] = "browser_pc";
                values["useDetect"] = "on";
                values["useDetectResult"] = "on";
                values["uuid"] = Guid.NewGuid().ToString();
                values["oxford"] = "on";
                values["isReturnSugg"] = "on";
                values["text"] = sourceText.Replace("\n", " ");

                var response = client.UploadValues(Resources.SogouTranslaterUrl, values);

                var responseString = Encoding.UTF8.GetString(response);
                JObject json = JObject.Parse(responseString);
                return (string)json["translate"]["dit"];
            }
        }


        private void Startupballon()
        {
            this.hiddenNotifyIcon.BalloonTipText = "Copy Anywhere to Translit!";
            this.hiddenNotifyIcon.BalloonTipTitle = "Translit!";
            this.hiddenNotifyIcon.ShowBalloonTip(1000);
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

            var request = WebRequest.Create(Resources.TranslitCheckUpdateUrl + UserID(Resources.Secret) + Resources.TranslitCurrentVersion);
            var response = request.GetResponse();

            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    responseString = reader.ReadToEnd().Trim();
                    if (responseString == "1")
                    {
                        var site = new ProcessStartInfo(Resources.TranslitAboutUrl);
                        Process.Start(site);
                    }
                }
            }
        }

        private void btnRollUp_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void TransForm_Deactivate(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void TransForm_Load(object sender, EventArgs e)
        {
            //this.Height = 500;
            this.MinimumSize = new System.Drawing.Size(62, this.Height);
            WindowState = FormWindowState.Minimized;
            this.Hide();
        }


        public Boolean ready2move = false;
        public Point mouseDownPoint;
        private void tbClipboardText_MouseMove(object sender, MouseEventArgs e)
        {
            tbClipboardText.Cursor = Cursors.IBeam;
            if (!ready2move)
            {
                mouseDownPoint = e.Location;
            }

            if (e.Button == MouseButtons.Middle)
            {
                ready2move = true;
                //label1.Text = String.Format("{0} :: {1}", e.X - mouseDownPoint.X, e.Y - mouseDownPoint.Y);
                this.Left = e.X + this.Left - mouseDownPoint.X;
                this.Top = e.Y + this.Top - mouseDownPoint.Y;

                tbClipboardText.Cursor = Cursors.SizeAll;
            }
            else
            {
                ready2move = false;
            }
        }

        //a tricky way to move the blinking curser and retain the black color
        private void tbClipboardText_Resize(object sender, EventArgs e)
        {
            tbClipboardText.Enabled = true;
        }
    }
}