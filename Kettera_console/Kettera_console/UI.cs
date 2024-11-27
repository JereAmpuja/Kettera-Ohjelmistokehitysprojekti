﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kettera_console
{
    public class UI //Singleton implementointi. Tuskin on tarpeellinen, mutta tehty kuitenkin koska osataan! :D
    {
        private GymManagement gm;
        private UI()
        {
            gm = new GymManagement();
        }
        private static UI instance = null;
        public static UI Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UI();
                }
                return instance;
            }
        }
        //Metodeja, joilla ohjataan ohjelman suoritusta:
        private void ContinuePrompt()
        {
            Console.WriteLine("\nPaina ENTER jatkaaksesi.");
            Console.ReadLine();
            Console.Clear();
        }

        public void Run()
        { 
            while(true)
            {
                try
                {
                Console.Clear();
                char value = MainMenu();
                switch(value)
                {
                    case '1':
                        Console.Clear();
                        CustomerMenu();
                        break;
                    case '2':
                        Console.Clear();
                        TrainerMenu(); 
                        break;
                    case '3':
                        Console.Clear();
                        GroupClassMenu();
                        break;
                    case '4':
                        Console.Clear();
                        break;
                    case '5':
                        Console.Clear();
                        break;
                    case '6':
                        Console.Clear();
                        gm.IncreaseGymVisitsByOne(); //GymManagement luokan metodi.
                        ContinuePrompt();
                        break;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ContinuePrompt();
                }
                
            }
        }
        private char MainMenu()
        {
            string text;
            text = "1: Hallitse asiakkaita.\n";
            text += "2: Hallitse valmentajia.\n";
            text += "3: Hallitse ryhmäliikuntatunteja.\n";
            text += "4: Hallitse varauksia.\n";
            text += "5: Hallitse kalentereja.\n";
            text += "6: Merkkaa salikäynti.\n";
            text += "\nValitse toiminto: ";
            Console.Write(text);
            return Console.ReadKey().KeyChar;
        }

        private void CustomerMenu()
        {
            while (true)
            {
                string text;
                text = "1: Lisää asiakas.\n";
                text += "2: Poista asiakas.\n";
                text += "3: Näytä asiakkaiden tiedot.\n";
                text += "4: Muokkaa asiakkaan tietoja.\n";
                text += "5: Lisää ryhmäliikunta- ja valmentaja käyntikertoja.\n";
                text += "0: Palaa päävalikkoon.\n";
                Console.WriteLine(text);
                char value = Console.ReadKey().KeyChar;

                switch (value)
                {
                    case '1':
                        Console.Clear();
                        gm.AddCustomer();
                        ContinuePrompt();
                        break;
                    case '2':
                        Console.Clear();
                        gm.DeleteCustomer();
                        ContinuePrompt();
                        break;
                    case '3':
                        Console.Clear();
                        gm.PrintAllCustomers();
                        ContinuePrompt();
                        break;
                    case '4':
                        Console.Clear();
                        gm.EditCustomer();
                        ContinuePrompt();
                        break;
                    case '5':
                        Console.Clear();
                        gm.AddGroupPtVisits();
                        ContinuePrompt();
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
            }
        }

        private void TrainerMenu()
        {
            while (true)
            {
                string text;
                text = "1: Lisää valmentaja.\n";
                text += "2: Poista valmentaja.\n";
                text += "3: Näytä valmentajien tiedot.\n";
                text += "4: Muokkaa valmentajan tietoja (vain nimi).\n";
                text += "0: Palaa päävalikkoon.\n";
                Console.WriteLine(text);
                char value = Console.ReadKey().KeyChar;

                switch (value)
                {
                    case '1':
                        Console.Clear();
                        gm.AddTrainer();
                        ContinuePrompt();
                        break;
                    case '2':
                        Console.Clear();
                        gm.DeleteTrainer();
                        ContinuePrompt();
                        break;
                    case '3':
                        Console.Clear();
                        gm.PrintAllTrainers();
                        ContinuePrompt();
                        break;
                    case '4':
                        Console.Clear();
                        gm.EditTrainer();
                        ContinuePrompt();
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
            }
        }

        private void GroupClassMenu()
        { 
            while(true) 
            { 
            string text;
                text = "1: Lisää ryhmäliikuntatunti.\n";
                text += "2: Poista ryhmäliikuntatunti.\n";
                text += "3: Näytä ryhmäliikuntatunnit.\n";
                text += "4: Muokkaa ryhmäliikuntatuntia.\n";
                text += "0: Palaa päävalikkoon.\n";
                Console.WriteLine(text);
                char value = Console.ReadKey().KeyChar;
                switch (value) 
                {
                    case '1':
                        Console.Clear();
                        gm.AddGroupClass();
                        ContinuePrompt();
                        break;
                    case '2':
                        Console.Clear();
                        gm.DeleteGroupClass();
                        ContinuePrompt();
                        break;
                    case '3':
                        Console.Clear();
                        gm.PrintAllGroupClasses();
                        ContinuePrompt();
                        break;
                        case '4':
                        Console.Clear();
                        gm.EditGroupClass();
                        ContinuePrompt();                   
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                        

                }
            
            }
        }
        private void ReservationMenu()
        {
            while(true)
            {
                string text;    
                text = "1: Lisää asiakas ryhmäliikuntatunnille.\n";
                text += "2: Poista asiakas ryhmäliikuntatunnilta.\n";
                text += "3: Näytä varaukset.\n";
                text += "4: Muokkaa varausta.\n";
                text += "0: Palaa päävalikkoon.\n";
                Console.WriteLine(text);
                char value = Console.ReadKey().KeyChar;
                switch (value)
                {
                    case '1':
                        Console.Clear();
                        gm.AddReservation();
                        ContinuePrompt();
                        break;
                    case '2':
                        Console.Clear();
                        gm.DeleteReservation();
                        ContinuePrompt();
                        break;
                    case '3':
                        Console.Clear();
                        gm.PrintAllReservations();
                        ContinuePrompt();
                        break;
                    case '4':
                        Console.Clear();
                        gm.EditReservation();
                        ContinuePrompt();
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
            }
        }
    }   
}
