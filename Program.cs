using System;
using System.Threading;
using System.Windows.Forms;

namespace GraphApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var thread2 = new Thread(() =>
            {
                var form2 = new UserForm("User 2 — Office B");
                form2.Location = new System.Drawing.Point(980, 80);
                Application.Run(form2);
            });
            thread2.SetApartmentState(ApartmentState.STA);
            thread2.Start();

            var form1 = new UserForm("User 1 — Office A");
            form1.Location = new System.Drawing.Point(10, 80);
            Application.Run(form1);
        }
    }
}
