using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace translit
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appID))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Translit 已经开启，请检查任务栏图标。", "Translit!");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(defaultValue: false);
                Application.Run(mainForm: new TransForm());
            }
        }
        private static string appID = "translit!";
    }
}
