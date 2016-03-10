using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Kesoft.MySqlBackuper
{
    /// <summary>
    /// MySQL database utility.
    /// </summary>
    public class MySqlUtility
    {
        private const string mySql = "mysql.exe";
        private const string mySqlDump = "mysqldump.exe";
        private readonly string batFileName = Path.Combine(Environment.CurrentDirectory, "mysqldump.temp.bat");
        private readonly string dataSource;
        private readonly string database;
        private readonly string userId;
        private readonly string password;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSource">Data source.</param>
        /// <param name="database">Database name.</param>
        /// <param name="userId">User id.</param>
        /// <param name="password">Password.</param>
        public MySqlUtility(string dataSource, string database, string userId, string password)
        {
            this.dataSource = dataSource;
            this.database = database;
            this.userId = userId;
            this.password = password;
        }


        /// <summary>
        /// Backup entire database.
        /// </summary>
        /// <param name="fileName">SQL file.</param>
        public void BackupDatabase(string fileName)
        {
            var mysqldumpexe = Path.Combine(Environment.CurrentDirectory, mySqlDump);
            var command = string.Format("\"{0}\" {1} > \"{2}\" ", mysqldumpexe, GetConnectionString(), fileName);
            ExecuteCommand(command);
        }

        /// <summary>
        /// Backup dabase structure, not include data.
        /// </summary>
        /// <param name="fileName">SQL file.</param>
        public void BackupDatabaseStructure(string fileName)
        {
            var mysqldumpexe = Path.Combine(Environment.CurrentDirectory, mySqlDump);
            var command = string.Format("\"{0}\" --no-data {1} > \"{2}\" ", mysqldumpexe, GetConnectionString(), fileName);
            ExecuteCommand(command);
        }

        /// <summary>
        /// Backup some tables structure and data.
        /// </summary>
        /// <param name="tables"> Table names. </param>
        /// <param name="fileName">SQL file.</param>
        public void BackupTables(string[] tables, string fileName)
        {
            var tableLines = tables.Where(table => !string.IsNullOrEmpty(table)).Aggregate(string.Empty, (current, table) => current + (table + " "));
            var mysqldumpexe = Path.Combine(Environment.CurrentDirectory, mySqlDump);
            var command = string.Format("\"{0}\" {1} {2}> \"{3}\" ", mysqldumpexe, GetConnectionString(), tableLines, fileName);
            ExecuteCommand(command);
        }

        private string GetConnectionString()
        {
            return string.Format("-h{0} -P{1} -u{2} -p{3} {4}", dataSource, 3306, userId, password, database);
        }

        /// <summary>
        /// Backup one table structure and data.
        /// </summary>
        /// <param name="table">Table name.</param>
        /// <param name="fileName">SQL file.</param>
        public void BackupTable(string table, string fileName)
        {
            if (!string.IsNullOrEmpty(table))
            {
                BackupTables(new[] {table}, fileName);
            }
        }

        /// <summary>
        /// Restore database.
        /// </summary>
        /// <param name="fileName">SQL file.</param>
        public void RestoreDatabase(string fileName)
        {
            var mysqlexe = Path.Combine(Environment.CurrentDirectory, mySql);
            var command = string.Format("\"{0}\" {1} < \"{2}\" ", mysqlexe, GetConnectionString(), fileName);
            ExecuteCommand(command);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="arguments">command arguments.</param>
        private void ExecuteCommand(string arguments)
        {
            StreamWriter sw = null;
            try
            {
                sw = File.CreateText(batFileName);
                sw.WriteLine(arguments);
                sw.Close();

                var process = new Process {StartInfo = new ProcessStartInfo {FileName = batFileName, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true}};
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new Exception("Execute command error, maybe database is disconnected.");
            }
            finally
            {
                sw?.Close();
                if (File.Exists(batFileName)) File.Delete(batFileName);
            }
        }
    }
}