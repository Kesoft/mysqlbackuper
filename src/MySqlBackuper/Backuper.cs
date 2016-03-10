using System;
using System.IO;
using System.Threading;
using Kesoft.MySqlBackuper.Properties;
using NLog;

namespace Kesoft.MySqlBackuper
{
    public class Backuper
    {
        private Timer timer;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            var tables = Settings.Default.Tables.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var period = Convert.ToInt32(Settings.Default.IntervalHours*60*60*1000);
            timer = new Timer(TimerCallback, tables, 2, period);
            log.Debug("Backuper started, interval: {0}", TimeSpan.FromMilliseconds(period));
        }

        private void TimerCallback(object state)
        {
            try
            {
                var tables = state as string[];
                var file = $"{Settings.Default.BackupPath}{Settings.Default.Database}-backup-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.sql";
                var dir = Path.GetDirectoryName(file);
                if (dir == null) throw new Exception("Path is null");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var mysql = new MySqlUtility(Settings.Default.DataSource, Settings.Default.Database, Settings.Default.UserId, Settings.Default.Password);
                log.Debug("Start to backup {0}...", file);
                mysql.BackupTables(tables, file);
                log.Debug("Mysqldump db to file {0}", file);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void Stop()
        {
            timer.Dispose();
            log.Debug("Backuper stopped.");
        }
    }
}