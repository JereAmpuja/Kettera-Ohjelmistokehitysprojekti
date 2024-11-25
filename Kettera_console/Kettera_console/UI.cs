using System;
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
    public class UI
    {
        private DbConnection db;
        private Customer customer;
        private List<Customer> customers;
        private Trainer trainer;
        private List<Trainer> trainers;
        private GroupClass groupClass;
        private List<GroupClass> groupClasses;
        private CalendarEvent calendarEvent;
        private List<CalendarEvent> calendarEvents;

        private UI()
        {
            db = new DbConnection();
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
        public void ContinuePrompt()
        {
            Console.WriteLine("\nPaina ENTER jatkaaksesi.");
            Console.ReadLine();
            Console.Clear();
        }

        public void CustomerFunctions()
        {   
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Lisää asiakas");
                Console.WriteLine("2. Määritä asiakkaalle uusi valmentaja");
                Console.WriteLine("3. Määritä uusi jäsenyyden päättymispäivä");
                Console.WriteLine("4. Poista asiakas");
                Console.WriteLine("5. Näytä asiakkaat");
                Console.WriteLine("0. Palaa päävalikkoon");
                Console.Write("\nValitse toiminto: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        AssignNewPersonalTrainer();
                        break;
                    case "3":
                        AssignNewMembershipEnd();
                        break;
                    case "4":
                        Console.Clear();
                        DeleteCustomer();
                        break;
                    case "5":
                        Console.Clear();
                        PrintAllCustomers();
                        ContinuePrompt();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
            }
        }

        public void GroupClassFunctions()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Lisää ryhmäliikuntatunti");
                Console.WriteLine("2. Lisää asiakas ryhmäliikuntatunnille");
                Console.WriteLine("3. Poista ryhmäliikuntatunti");
                Console.WriteLine("4. Näytä ryhmäliikuntatunnit");
                Console.WriteLine("0. Palaa päävalikkoon");
                Console.Write("\nValitse toiminto: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Clear();
                        AddGroupClass();
                        break;
                    case "2":
                        Console.Clear();
                        AddCustomerToGroupClass();
                        break;
                    case "3":
                        //DeleteGroupClass();
                        break;
                    case "4":
                        PrintGroupClasses();
                        ContinuePrompt();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nVirheellinen syöte.");
                        ContinuePrompt();
                        break;
                }
            }
        }   

        public void CalendarSearch()
        {
            Console.Clear();
            Console.WriteLine("1. Näytä kaikki varaukset");
            Console.WriteLine("2. Näytä varaukset päivämäärän mukaan");
            Console.WriteLine("3. Näytä varaukset asiakkaan mukaan");
            Console.WriteLine("4. Näytä varaukset ryhmäliikuntatunnin mukaan");
            Console.WriteLine("0. Palaa päävalikkoon");
            Console.Write("\nValitse toiminto: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    PrintAllCalendarEvents();
                    ContinuePrompt();
                    break;
                case "2":
                    PrintCalendarEventsByDate();
                    break;
                case "3":
                    PrintCalendarEventsByCustomer();
                    break;
                case "4":
                    PrintCalendarEventsByTrainer();
                    break;
                case "5":
                    PrintCalendarEventsByGroupClass();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\nVirheellinen syöte.");
                    ContinuePrompt();
                    break;
            }
        }

        private void PrintCalendarEventsByDate()
        {
            Console.Clear();
            Console.Write("Syötä päivämäärä muodossa PP.KK.VVVV: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset päivämäärälle:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date.Date == date.Date)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
            ContinuePrompt();
        }

        private void PrintCalendarEventsByCustomer()
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jonka varaukset haluat nähdä.\n");
            customer = RequestCustomer();

            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset asiakkaalle:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == customer.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
            ContinuePrompt();
        }

        private void PrintCalendarEventsByTrainer()
        {
            Console.Clear();
            Console.WriteLine("Valitse valmentaja jonka varaukset haluat nähdä.\n");
            trainer = RequestTrainer();

            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset valmentajalle:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].trainerID == trainer.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
            ContinuePrompt();
        }
        private void PrintCalendarEventsByGroupClass()
        {
            Console.Clear();
            Console.WriteLine("Valitse ryhmäliikuntatunti jonka varaukset haluat nähdä.\n");
            groupClass = RequestGroupClass();

            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset ryhmäliikuntatunnille:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].classID == groupClass.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
            ContinuePrompt();
        }

        //Metodeja, joilla näytetään tietoa:

        //Metodi joka tulostaa asiakkaat ja palauttaa valitun asiakkaan oliona. Tarkistaa myös että arvo löytyy.
        public Customer RequestCustomer() 
        {
            customers = db.GetAllCustomers();

            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{customers[i].ID + ":"} {customers[i].Name}");
            }
            Console.Write("\nSyötä asiakkaan ID tai Nimi: ");

            string response = Console.ReadLine();
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].ID.ToString() == response)
                {
                    return customers[i];
                }
                else if (customers[i].Name == response)
                {
                    return customers[i];
                }
                else if (i == customers.Count - 1)
                {
                    Console.WriteLine("\nAsiakasta ei löytynyt.");
                    ContinuePrompt();
                }
            }

            ContinuePrompt();
            return null; ;

        }
        //Metodi joka tulostaa valmentajat ja palauttaa valitun valmentajan oliona. Tarkistaa myös että arvo löytyy.
        public Trainer RequestTrainer()
        {
            trainers = db.GetAllTrainers();

            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine($"{trainers[i].ID + ":"} {trainers[i].Name}");
            }
            Console.Write("\nSyötä valmentajan ID tai Nimi: ");
         
                string response = Console.ReadLine();
                for (int i = 0; i < trainers.Count; i++)
                {
                    if (trainers[i].ID.ToString() == response)
                    {
                        return trainers[i];
                    }
                    else if (trainers[i].Name == response)
                    {
                        return trainers[i];
                    }
                    else if (i == trainers.Count - 1)
                    {
                        Console.WriteLine("\nValmentajaa ei löytynyt.");
                        ContinuePrompt();
                    }
                }
            ContinuePrompt();
            return null;
            
        }
        //Metodi joka tulostaa ryhmäliikuntatunnit ja palauttaa valitun ryhmäliikuntatunnin oliona. Tarkistaa myös että arvo löytyy.
        public GroupClass RequestGroupClass()
        {
            PrintGroupClasses();
            Console.Write("\nSyötä ryhmäliikuntatunnin ID: ");

            int groupClassID = Convert.ToInt16(Console.ReadLine());
            for (int i = 0; i < groupClasses.Count; i++)
            {
                if (groupClasses[i].ID == groupClassID)
                {
                    return groupClasses[i];
                }
                else if (i == groupClasses.Count - 1)
                {
                    Console.WriteLine("\nRyhmäliikuntatuntia ei löytynyt.");
                }
            }
           
            ContinuePrompt();
            return null;

        }

        public void PrintAllCustomers()
        {
            customers = db.GetAllCustomers();
            Console.WriteLine("Asiakkaat:\n");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine(customers[i].ToString());
            }
        }
        public void PrintAllTrainers()
        {
            trainers = db.GetAllTrainers();
            Console.WriteLine("Valmentajat:\n");
            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine(trainers[i].ToString());
            }

        }

        public void PrintGroupClasses()
        {
            groupClasses = db.GetGroupClasses();

            Console.WriteLine("Ryhmäliikuntatunnit:");
            for (int i = 0; i < groupClasses.Count; i++)
            {
                Console.WriteLine(groupClasses[i].ToString());
            }
        }

        public void PrintAllCalendarEvents()
        {
            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        }

        public void AddGroupClass()
        {
            Console.WriteLine("Määritä valmentaja. Saatavilla olevat valmentajat:");
            trainer = RequestTrainer();
            Console.Write("\nSyötä päivämäärä ja aika muodossa PP.KK.VVVV HH:MM: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            Console.Write("\nSyötä kävijäraja: ");
            int visitorLimit = Convert.ToInt16(Console.ReadLine());

            string[] fields = { "trainer_ref", "dateandtime", "visitor_limit", "visitor_count" };
            string[] values = { trainer.ID.ToString(), date.ToString("yyyy-MM-dd HH:mm"), visitorLimit.ToString(), "0" };

            db.ExecuteInsertInto("group_class", fields, values);

            Console.WriteLine("\nRyhmäliikuntatunti lisätty tietokantaan onnistuneesti.");
            ContinuePrompt();
        }

        public void AddCustomer()
        {
            Console.Clear();

            Console.Write("Asiakkaan nimi muodossa ETUNIMI SUKUNIMI: ");
            string name = Console.ReadLine();
            Console.Write("\nSyntymäpäivä muodossa PP.KK.VVVV: ");
            DateTime birthDay = Convert.ToDateTime(Console.ReadLine());
            Console.Write("\nJäsenyyden päättymispäivä muodossa PP.KK.VVVV: ");
            DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine();
            //Kysytään valmentaja metodia käyttäen.
            Console.WriteLine("\nMääritä valmentaja. Saatavilla olevat valmentajat:\n");
            trainer = RequestTrainer();
            int personalTrainerID = trainer.ID;
            Console.Write("\nRyhmä ja valmentaja käynnit: ");
            int groupVisits = Convert.ToInt16(Console.ReadLine());

            string[] fields = { "customer_name", "birthday", "trainer_ref", "gym_visits", "group_pt_visits", "membership_end" };
            string[] values = { name, birthDay.ToString("yyyy-MM-dd"), personalTrainerID.ToString(), "0", groupVisits.ToString(), membershipEndDay.ToString("yyyy-MM-dd") };

            db.ExecuteInsertInto("customer", fields, values);

            Console.WriteLine("\nAsiakas lisätty tietokantaan onnistuneesti!");
            ContinuePrompt();
        }

        public void DeleteCustomer()
        {
            Console.WriteLine("Valitse asiakas jonka haluat poistaa.\n");
            customer = RequestCustomer();

            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteDelete("customer", keyfield, keyValue);

            Console.WriteLine("\nAsiakas poistettu onnistuneesti.");
            ContinuePrompt();
        }

        public void AssignNewPersonalTrainer()
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jolle haluat määrittää uuden valmentajan.\n");
            customer = RequestCustomer();

            Console.WriteLine("\nValitse valmentaja asiakkaalle.\n");
            trainer = RequestTrainer();
            
            string[] fields = { "trainer_ref" };
            string[] values = { trainer.ID.ToString() };
            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteUpdate("customer", fields, values, keyfield, keyValue);

            Console.WriteLine("\nValmentaja määritetty onnistuneesti!");
            ContinuePrompt();
        }

        public void AssignNewMembershipEnd()
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jolle haluat määrittää uuden jäsenyyden päättymispäivän.\n");
            customer = RequestCustomer();

            Console.Write("\nUusi jäsenyyden päättymispäivä muodossa PP.KK.VVVV: ");
            DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());

            string[] fields = { "membership_end" };
            string[] values = { membershipEndDay.ToString("yyyy-MM-dd") };
            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteUpdate("customer", fields, values, keyfield, keyValue);
        }
        public void AddCustomerToGroupClass()
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jonka haluat lisätä ryhmäliikuntatunnille.\n");
            customer = RequestCustomer();
            customer.GymVisits++;
            if (customer.GroupVisits == 0)
            {
                Console.WriteLine("Asiakkaalla ei ole enää ryhmäliikunta/pt kertoja jäljellä.");
                ContinuePrompt();
                return;
            }


            Console.WriteLine("\nValitse ryhmäliikuntatunti jolle haluat lisätä asiakkaan.\n");
            groupClass = RequestGroupClass();

            string[] fields = { "customer_ref", "class_ref" };
            string[] values = { customer.ID.ToString(), groupClass.ID.ToString() };

            db.ExecuteInsertInto("group_class_reservation", fields, values);

            customer.GroupVisits--;
            Console.WriteLine("\nAsiakas lisätty ryhmäliikuntatunnille onnistuneesti. Jäljellä olevat ryhmäliikunta/pt kerrat: " + customer.GroupVisits);
            ContinuePrompt();
            

            string[] fields2 = { "group_pt_visits" };
            string[] values2 = { customer.GroupVisits.ToString() };
            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteUpdate("customer", fields2, values2, keyfield, keyValue);
        }

        public void DeleteGroupClass()
        {
            Console.Clear();
            Console.WriteLine("Valitse ryhmäliikuntatunti jonka haluat poistaa.\n");
            groupClass = RequestGroupClass();

            string keyfield = "class_id";
            string keyValue = groupClass.ID.ToString();

            db.ExecuteDelete("group_class", keyfield, keyValue);

            Console.WriteLine("\nRyhmäliikuntatunti poistettu onnistuneesti.");
            ContinuePrompt();
        }

    }

    
}
