using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using Sys = Cosmos.System;
using System.Linq.Expressions;
using System.Net.NetworkInformation;

namespace LineOS
{
    public class Shell
    {
        public string ShellDir;
        public void Main() 
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                string[] args = input.Split(' ');

                if (args.Length == 0) continue;

                switch (args[0])
                {
                    case "touch":
                        if (args.Length > 1)
                            TouchCommand(args[1], ShellDir);
                        else
                            Console.WriteLine("touch: missing file name");
                        break;
                    case "del":
                        if (args.Length > 1)
                            DelCommand(args[1], ShellDir);
                        else
                            Console.WriteLine("del: missing file name");
                        break;
                    case "write":
                        if (args.Length > 2)
                            WriteCommand(args[1], input.Substring(args[0].Length + args[1].Length + 2), ShellDir);
                        else
                            Console.WriteLine("write: missing file name or content");
                        break;
                    case "show":
                        ShowCommand(ShellDir);
                        break;
                    case "read":
                        if (args.Length > 1)
                            ReadCommand(args[1], ShellDir);
                        else
                            Console.WriteLine("read: missing file name");
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "shutdown":
                        ShutdownCommand();
                        break;
                    case "reboot":
                        RebootCommand();
                        break;
                    case "help":
                        Console.WriteLine("");
                        Console.Write("It is ");

                        Console.ForegroundColor = ConsoleColor.Blue;

                        Console.Write("LineOS");

                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write("(in development). Created ");

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.Write("by LineTeam ");

                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("There is all command list:");

                        Console.WriteLine("help - help (There you can watch list of commands)");
                        Console.WriteLine("clear - clear terminal from text");
                        Console.WriteLine("touch {filename} - create a new empety file");
                        Console.WriteLine("del {filename} - delete file");
                        Console.WriteLine("write {filename} {text} - write text to the files");
                        Console.WriteLine("read {filename} - read files");
                        Console.WriteLine("show - show files and directories(catalogs or folders)");

                        Console.WriteLine("");

                        Console.WriteLine("shutdown - shutdown your computer");
                        Console.WriteLine("reboot - reboot your computer");


                        break;
                    default:
                        Console.WriteLine($"Command {args[0]} not found.");
                        break;
                }
            }
        }

        static void TouchCommand(string filename, string dir)
        {
            try
            {
                var file_stream = File.Create(dir+filename);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[OK] File create.");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void ShowCommand(string dir) 
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            var files_list = Directory.GetFiles(dir);
            var directory_list = Directory.GetDirectories(dir);

            try 
            {
                foreach (var file in files_list)
                {
                    Console.WriteLine(file);
                }
                foreach (var directory in directory_list)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(directory);
                }
            }
            catch(Exception e) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void ReadCommand(string filename, string dir)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(filename+":");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            try
            {
                Console.WriteLine(File.ReadAllText(dir+filename));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void WriteCommand(string filename, string content, string dir)
        {
            try
            {
                File.WriteAllText(dir+filename, content);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Content written to {filename}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void DelCommand(string filename, string dir)
        {
            while (true)
            {
                Console.WriteLine("Are you sure? Y/N");
                string input = Console.ReadLine();

                if (input.ToUpper() == "Y")
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        File.Delete(dir+ filename);
                        Console.WriteLine("File deleted successfully.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.ToString());
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (input.ToUpper() == "N")
                {
                    return; 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Sorry, try again.");
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void dir(string disk) 
        {
            ShellDir = disk;
        }

        static void ShutdownCommand() 
        {
            Cosmos.System.Power.Shutdown();
        }

        static void RebootCommand()
        {
            Cosmos.System.Power.Reboot();
        }

    }
}
