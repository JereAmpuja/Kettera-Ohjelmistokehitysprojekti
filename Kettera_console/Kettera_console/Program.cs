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
                    Console.WriteLine("1. Lisää uusi asiakas");
                    Console.WriteLine("2. Lisää ryhmäliikuntatunti");
                    Console.WriteLine("3. Lisää varaus");
                    Console.WriteLine("4. Poista varaus");
                    Console.WriteLine("5. Poista asiakas");
                    Console.WriteLine("6. Poista ryhmäliikuntatunti");
                    Console.WriteLine("7. Näytä asiakkaat & valmentajat");
                    Console.WriteLine("8. Näytä ryhmäliikuntatunnit");
                    Console.WriteLine("9. Näytä varaukset");
                    Console.WriteLine("0. Lopeta");
                    Console.Write("\nValitse toiminto: ");
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            ui.AddCustomer();
                            break;
                        case "2":
                            ui.AssignNewPersonalTrainer();
                            break;
                        case "3":
                            ui.AssignNewMembershipEnd();
                            break;
                        case "4":
                            ui.RequestGroupClass();
                            ui.continuePrompt();
                            break;
                        case "5":

                            break;
                        case "6":

                            break;
                        case "7":
                            Console.Clear();
                            ui.PrintAllTrainers();
                            Console.WriteLine();
                            ui.PrintAllCustomers();
                            ui.continuePrompt();
                            break;
                        case "8":
                            ui.PrintGroupClasses();
                            ui.continuePrompt();
                            break;
                        case "9":

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
