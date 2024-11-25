using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Data.OleDb;
using System.Xml;
using System.Windows.Input;
using System.Xml.Linq;
using System.Net.NetworkInformation;

namespace Kettera_console
{     
    internal class Program
    {
        static void Main(string[] args)
        {
            UI ui = UI.Instance;
            bool running = true;
            while (running)
            {
                try
                {
                    Console.WriteLine("1. Lisää ja hallinnoi asiakkaita.");
                    Console.WriteLine("2. Lisää ja hallinnoi ryhmäliikuntatunteja");
                    Console.WriteLine("3. Hallinnoi valmentajien tunteja");
                    Console.WriteLine("4. Näytä asiakkaat & valmentajat");
                    Console.WriteLine("5. Näytä ryhmäliikuntatunnit");
                    Console.WriteLine("6. Näytä asiakkaiden varaukset");
                    Console.WriteLine("0. Lopeta");
                    Console.Write("\nValitse toiminto: ");
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            ui.CustomerFunctions();
                            break;
                        case "2":
                            Console.Clear();
                            ui.GroupClassFunctions();
                            break;
                        case "3":
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            ui.PrintAllCalendarEvents();
                            Console.ReadKey();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Virheellinen syöte. Paina ENTER jatkaaksesi.");
                            Console.ReadLine();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Virhe: " + ex.Message);
                    Console.WriteLine("Paina ENTER jatkaaksesi.");
                    Console.ReadLine();
                }
                Console.Clear();
            }
        }      
    }
}
