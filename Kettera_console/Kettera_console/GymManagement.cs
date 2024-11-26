using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
    internal class GymManagement
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

        public GymManagement()
        {
            db = new DbConnection();
        }

        public void PrintCalendarEventsByDate()
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
        }

        public void PrintCalendarEventsByCustomer()
        {
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
        }

        public void PrintCalendarEventsByTrainer()
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
        }
        public void PrintCalendarEventsByGroupClass()
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
        }

        public void RequestCalendarEventPrint()
        {

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
                }
            }
            return null; ;

        }
        //Metodi joka tulostaa valmentajat ja palauttaa valitun valmentajan oliona. Tarkistaa myös että arvo löytyy.
        public Trainer RequestTrainer()
        {
            trainers = db.GetAllTrainers();

            PrintAllTrainers();
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
                }
            }
            return null;

        }
        //Metodi joka tulostaa ryhmäliikuntatunnit ja palauttaa valitun ryhmäliikuntatunnin oliona. Tarkistaa myös että arvo löytyy.
        public GroupClass RequestGroupClass()
        {
            PrintAllGroupClasses();
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
            return null;

        }
        //Tulostaa kaikki ryhmäliikuntatunnit.
        public void PrintAllCustomers()
        {
            customers = db.GetAllCustomers();
            Console.WriteLine("Asiakkaat:\n");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine(customers[i].ToString());
            }
        }
        //Tulostaa kaikki ryhmäliikuntatunnit.
        public void PrintAllTrainers()
        {
            trainers = db.GetAllTrainers();
            Console.WriteLine("Valmentajat:\n");
            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine(trainers[i].ToString());
            }
        }
        //Tulostaa kaikki ryhmäliikuntatunnit.
        public void PrintAllGroupClasses()
        {
            groupClasses = db.GetGroupClasses();

            Console.WriteLine("Ryhmäliikuntatunnit:");
            for (int i = 0; i < groupClasses.Count; i++)
            {
                Console.WriteLine(groupClasses[i].ToString());
            }
        }
        //Tulostaa kaikki varaukset.
        public void PrintAllCalendarEvents()
        {
            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        }

        //Metodit, jolla luodaan uusi ryhmäliikuntatunti
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
        }

        public void DeleteCustomer()
        {
            Console.WriteLine("Valitse asiakas jonka haluat poistaa.\n");
            customer = RequestCustomer();

            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteDelete("customer", keyfield, keyValue);

            Console.WriteLine("\nAsiakas poistettu onnistuneesti.");
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
        //Metodi joka lisää asiakkaan ryhmäliikuntatunnille. Kutsuu metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
        public void AddCustomerToGroupClass()
        {
            Console.WriteLine("Valitse asiakas jonka haluat lisätä ryhmäliikuntatunnille.\n");
            customer = RequestCustomer(); //Kutsutaan metodia joka palauttaa valitun asiakkaan oliona.
            //ilmoitetaan asiakkaalle jos hänellä ei ole enää ryhmäliikunta/pt kertoja jäljellä.
            if (customer.GroupVisits == 0)
            {
                Console.WriteLine("Asiakkaalla ei ole enää ryhmäliikunta/pt kertoja jäljellä.");
                return;
            }
            Console.WriteLine("\nValitse ryhmäliikuntatunti jolle haluat lisätä asiakkaan.\n");
            groupClass = RequestGroupClass(); //Kutsutaan metodia joka palauttaa valitun ryhmäliikuntatunnin oliona.

            string[] fields = { "customer_ref", "class_ref" };
            string[] values = { customer.ID.ToString(), groupClass.ID.ToString() };

            db.ExecuteInsertInto("group_class_reservation", fields, values);

            IncreaseDecreaseGroupPtVisits(customer.ID, 1, 1);
            Console.WriteLine("\nAsiakas lisätty ryhmäliikuntatunnille onnistuneesti. Jäljellä olevat ryhmäliikunta/pt kerrat: " + customer.GroupVisits);

        }

        //Metodi joka poistaa ryhmäliikuntatunnin. Kutsuu metodia joka poistaa varaukset.
        public void DeleteGroupClass()
        {
            Console.Clear();
            Console.WriteLine("Valitse ryhmäliikuntatunti jonka haluat poistaa.\n");
            //Kutsutaan metodia joka palauttaa valitun ryhmäliikuntatunnin oliona.
            groupClass = RequestGroupClass(); 
            //Poistetaan ryhmäliikuntatunti tietokannasta.
            string keyfield = "class_id";
            string keyValue = groupClass.ID.ToString();
            db.ExecuteDelete("group_class", keyfield, keyValue);
            //Kutsutaan metodia joka poistaa kyseisen ryhmäliikuntatunnin varaukset.
            DeleteGroupClassReservationsByID(Convert.ToInt16(keyValue));
      
            Console.WriteLine("\nRyhmäliikuntatunti poistettu onnistuneesti.");
        }
        //Metodi, joka poistaa ryhmäliikuntatunnin varaukset ID:n perusteella. Kutsuu metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
        public void DeleteGroupClassReservationsByID(int groupClassID)
        {
            calendarEvents = db.GetAllCalendarEvents(); //haetaan kaikki varaukset.

            for (int i = 0; i < calendarEvents.Count ; i++) //Looppi joka tarkistaa kaikki varaukset, sekä kutsuu metodia joka poistaa varauksen.
            {
                if (calendarEvents[i].classID == groupClassID)
                {
                    db.ExecuteDelete("group_class_reservation", "class_ref", groupClassID.ToString());
                    //Kutsutaan metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
                    IncreaseDecreaseGroupPtVisits(calendarEvents[i].customerID, 1, 0);
                }
            }
        }

        //Metodi joka vähentää tai lisää asiakkaan ryhmäliikuntakertoja.
        public void IncreaseDecreaseGroupPtVisits(int customerID, int increaseValue, int incrDecr)
        {
            customer = db.GetCustomerByID(customerID.ToString()); //Haetaan asiakas tietokannasta.
            //Tarkistetaan onko kyseessä lisäys vai vähennys.
            if (incrDecr == 1) 
            {
                customer.GroupVisits += increaseValue;
            }
            else if (incrDecr == 0)
            {
                customer.GroupVisits -= increaseValue;
            }
            string[] fields = { "group_pt_visits" };
            string[] values = { customer.GroupVisits.ToString() };

            //Päivitetään tieto tietokantaan.
            db.ExecuteUpdate("customer", fields, values, "customer_id", customer.ID.ToString());
        }

        public void IncreaseDecreaseGymVisits(int customerID, int increaseValue, int incrDecr)
        {
            customer = db.GetCustomerByID(customerID.ToString());
            if (incrDecr == 1)
            {
                customer.GymVisits += increaseValue;
            }
            else if (incrDecr == 0)
            {
                customer.GymVisits -= increaseValue;
            }
            string[] fields = { "gym_visits" };
            string[] values = { customer.GymVisits.ToString() };

            db.ExecuteUpdate("customer", fields, values, "customer_id", customer.ID.ToString());
        }


    }
}
