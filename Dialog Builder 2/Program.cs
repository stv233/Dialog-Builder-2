using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Array.IndexOf(args, "-noupdt") != -1)
            {
                RemoveFromArgs(ref args, "-noupdt");
            }
            else
            {
                new frСheckingForUpdates().ShowDialog();
            }

            if (Array.IndexOf(args, "-reset") != -1)
            {
                RemoveFromArgs(ref args, "-reset");
                System.IO.File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Stv233\\Dialog builder\\usersett");
            }

            Application.Run(new frMain(args));
        }

        static void RemoveFromArgs(ref string[] args, string target)
        {
            for (var i = Array.IndexOf(args,target); i < args.Length - 1; i++)
            {
                args[i] = args[i + 1];
            }
            Array.Resize<string>(ref args, args.Length - 1);
        }
    }
}
