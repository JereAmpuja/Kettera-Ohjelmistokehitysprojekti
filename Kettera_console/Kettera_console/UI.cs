using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
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

        public void continuePrompt()
        {
            Console.WriteLine("\nPaina ENTER jatkaaksesi.");
            Console.ReadLine();
            Console.Clear();
        }

        //Metodi joka tulostaa asiakkaat ja palauttaa valitun asiakkaan oliona. Tarkistaa myös että arvo löytyy.
        public Customer RequestCustomer() 
        {
            customers = db.GetAllCustomers();

            Console.WriteLine($"{"ID",-3} {"Nimi",-20}");
            Console.WriteLine(new string('-', 25));
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{customers[i].ID + ":",-3} {customers[i].Name,-20}");
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
                    continuePrompt();
                }
            }

            continuePrompt();
            return null; ;

        }
        //Metodi joka tulostaa valmentajat ja palauttaa valitun valmentajan oliona. Tarkistaa myös että arvo löytyy.
        public Trainer RequestTrainer()
        {
            trainers = db.GetAllTrainers();

            Console.WriteLine($"{"ID",-3} {"Nimi",-20}");
            Console.WriteLine(new string('-', 25));
            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine($"{trainers[i].ID + ":",-3} {trainers[i].Name, -20}");
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
                        continuePrompt();
                    }
                }
            continuePrompt();
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
           
            continuePrompt();
            return null;

        }

        public void PrintAllCustomers()
        {
            customers = db.GetAllCustomers();
            Console.WriteLine("Asiakkaat:\n");
            Console.WriteLine($"{"ID",-3} {"Nimi",-20}{"Syntymäpäivä",-20}{"Jäsenyyden päättymispäivä",-29} {"Valmentaja",-20}{"Käyntejä salilla",-20}{"Käyntejä ryhmäliikunnassa",-20}");
            Console.WriteLine(new string('-', 139));
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine(customers[i].ToString());
            }


        }
        public void PrintAllTrainers()
        {
            trainers = db.GetAllTrainers();
            Console.WriteLine("Valmentajat:\n");
            Console.WriteLine($"{"ID",-3} {"Nimi",-20}");
            Console.WriteLine(new string('-', 20));
            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine(trainers[i].ToString());
            }

        }

        public void PrintGroupClasses()
        {
            groupClasses = db.GetGroupClasses();

            Console.WriteLine("Ryhmäliikuntatunnit:");
            Console.WriteLine($"\n{"ID",-3} {"Valmentaja",-19} {"Päivämäärä ja aika",-20}");
            Console.WriteLine(new string('-', 75));
            for (int i = 0; i < groupClasses.Count; i++)
            {
                Console.WriteLine(groupClasses[i].ToString());
            }
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
            continuePrompt();
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
            continuePrompt();
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
    }
}
