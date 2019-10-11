using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using translit.Properties;

namespace translit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            //https://stackoverflow.com/questions/42533036/kill-other-instances-if-program-is-running
            Mutex mutex = new Mutex(false, "Global\\" + appID);
            using (mutex)
            {
                try
                {
                    bool tryAgain = true;
                    while (tryAgain)
                    {
                        bool isSingleInstance = false;
                        try
                        {
                            isSingleInstance = mutex.WaitOne(0, false);
                            //Debug.WriteLine("result:" + result);
                        }
                        catch (AbandonedMutexException ex)
                        {
                            // No action required
                            isSingleInstance = true;
                        }
                        if (isSingleInstance)
                        {
                            // Run the application
                            tryAgain = false;
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(mainForm: new TransForm());
                        }
                        else
                        {
                            DialogResult user_reboot;
                            user_reboot = MessageBox.Show("Translit 已经开启，请检查任务栏图标。\n\n是否重启软件？", "Translit! " + Resources.TranslitCurrentVersion, MessageBoxButtons.YesNo);
                            if (user_reboot == DialogResult.Yes)
                            {
                                foreach (Process proc in Process.GetProcesses())
                                {
                                    if (proc.ProcessName.Equals(Process.GetCurrentProcess().ProcessName) && proc.Id != Process.GetCurrentProcess().Id)
                                    {
                                        proc.Kill();
                                        break;
                                    }
                                }
                                // Wait for process to close
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                finally
                {
                    if (mutex != null)
                    {
                        mutex.Close();
                        mutex = null;
                    }
                }

                //DialogResult result;
                //if (!mutex.WaitOne(0, false))
                //{
                //    result = MessageBox.Show("Translit 已经开启，请检查任务栏图标。\n\n是否重启软件？", "Translit! " + Resources.TranslitCurrentVersion, MessageBoxButtons.YesNo);
                //    if (result == DialogResult.Yes)
                //    {
                        
                //    }
                //    return;
                //}
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(defaultValue: false);
                //Application.Run(mainForm: new TransForm());
            }
        }
        private static string appID = "translit!";
    }
}
