using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using Npgsql;

class Program
{
    static void Main()
    {
        if (!File.Exists("./secrets"))
        {
            Console.WriteLine($"No secrets file found at {AppDomain.CurrentDomain.BaseDirectory}");
            Console.ReadKey();
            return;
        }
        var secrets = File.ReadAllLines("./secrets");

        string set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";

        string host = string.Empty,
            database = string.Empty,
            user = string.Empty,
            password = string.Empty,

            backupDirectory = string.Empty;

        foreach (var secret in secrets)
        {
            string key = secret.Split('=')[0];
            string value = secret.Split('=')[1];

            switch (key)
            {
                case "HOST":
                    host = value;
                    Console.WriteLine($"Host value = {host}");
                    break;
                case "DATABASE":
                    database = value;
                    Console.WriteLine($"Database value = {database}");
                    break;
                case "USER":
                    user = value;
                    Console.WriteLine($"User value = {user}");
                    break;
                case "PASSWORD":
                    password = value;
                    Console.WriteLine($"Password found");
                    break;
                case "BACKUP_DIRECTORY":
                    backupDirectory = value;
                    Console.WriteLine($"Backup Directory = {backupDirectory}");
                    break;
            }
        }

        if (host == string.Empty ||
            database == string.Empty ||
            user == string.Empty ||
            password == string.Empty ||
            backupDirectory == string.Empty)
        {
            Console.WriteLine("Not all values found, exiting");
            Console.ReadKey();
            return;
        }

        // Timestamp (for backup file naming)
        string timestamp = DateTime.Now.ToString("dd_MM_yyyy");

        // Filename for the backup file
        string backupFile = $"{backupDirectory}botcstat_{timestamp}.backup";

        Console.WriteLine("");
        Console.WriteLine("");

        if (File.Exists(backupFile))
        {
            Console.WriteLine($"File already exists at {backupFile}. Exiting");
            Console.ReadKey();
            return;
        }

        try
        {
            Console.WriteLine("Backuping...");

            var e = Execute(
            $"{set}PGPASSWORD={password}\n" +
                $"pg_dump -h {host} -U {user} -d {database} -Fc -f {backupFile}");

            e.Wait();



            Console.WriteLine("Backup completed successfully. Backup file is " + backupFile);
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine("Error during backup : " + ex.Message);
        }
        Console.ReadKey();

    }

    private static Task Execute(string dumpCommand)
    {
        return Task.Run(() =>
        {

            string batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
            try
            {
                string batchContent = "";
                batchContent += $"{dumpCommand}";

                File.WriteAllText(batFilePath, batchContent, Encoding.ASCII);

                ProcessStartInfo info = ProcessInfoByOS(batFilePath);

                using Process proc = Process.Start(info);

                proc.WaitForExit();
                var exit = proc.ExitCode;

                proc.Close();
            }
            catch (Exception e)
            {
                // Your exception handler here.
                Console.WriteLine("Exception " + e.Message);

            }
            finally
            {
                if (File.Exists(batFilePath)) File.Delete(batFilePath);
            }
        });
    }

    private static ProcessStartInfo ProcessInfoByOS(string batFilePath)
    {
        ProcessStartInfo info;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            info = new ProcessStartInfo(batFilePath)
            {
            };
        }
        else
        {
            info = new ProcessStartInfo("sh")
            {
                Arguments = $"{batFilePath}"
            };
        }

        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        info.RedirectStandardError = true;

        return info;
    }
}
