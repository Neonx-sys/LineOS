using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.Core.IOGroup;
using Cosmos.HAL;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;

namespace LineOS
{
    public class Kernel : Sys.Kernel
    {
        private Shell shell;
        public string disk;

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.WriteLine("Booting...");

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[In Progress] File system initialization...");

                var fs = new CosmosVFS();
                VFSManager.RegisterVFS(fs);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[OK] File system initialization end.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] File system initialization failed: {ex.Message}");
            }
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[In Progress] Connecting to the shell...");

                shell = new Shell();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[OK] Connecting to the shell end.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Connecting to the shell failed: {ex.Message}");
            }

            try
            {
                var volumes = VFSManager.GetVolumes();
                if (volumes != null && volumes.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Available disks:");

                    for (int i = 0; i < volumes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {volumes[i].mName}");
                    }

                    Console.ForegroundColor = ConsoleColor.White;

                    while (true)
                    {
                        Console.Write("What disk you want to use? (Write disk number) > ");
                        string input = Console.ReadLine();

                        if (int.TryParse(input, out int diskNumber))
                        {
                            if (diskNumber >= 1 && diskNumber <= volumes.Count)
                            {
                                disk = volumes[diskNumber - 1].mName; 
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"You selected: {disk}");
                                break; 
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Wrong input. Please enter a valid disk number.");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Disks not found! Using free space!");
                    disk = "";
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Initializing network...");

            var device = NetworkDevice.GetDeviceByName("eth0");
            if (device == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No network device found!");
                Console.WriteLine("Network initialized with errors.");
                return;
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Network initialized.");
            }

            NetworkDevice nic = device;
            IPConfig.Enable(nic, new Address(192, 168, 0, 69), new Address(255, 255, 255, 0), new Address(192, 168, 0, 1));

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("");
            Console.Write("Welcome to the ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("LineOS!");

            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Type 'help' for help ");

        }

        protected override void Run()
        {
            shell.Dir(disk);
            shell.Main();
        }
    }
}

