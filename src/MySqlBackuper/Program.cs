using Topshelf;

namespace Kesoft.MySqlBackuper
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(a =>
            {
                a.Service<Backuper>(x =>
                {
                    x.ConstructUsing(c => new Backuper());
                    x.WhenStarted(s => s.Start());
                    x.WhenStopped(s => s.Stop());
                });
                a.SetDescription("MySQL Backup Service");
                a.SetDisplayName("MySQL Backuper");
                a.SetServiceName("mysqlbackuper");
                a.RunAsLocalSystem();
            }
                );
        }
    }
}