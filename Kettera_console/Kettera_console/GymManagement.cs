using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
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

        //Kalenterin tulostusmetodeja.
        public void PrintGcCalendarEventsByDate() //Tulostaa varaukset tiettynä päivänä. 0 käyttöä toistaiseksi.
        {
            Console.Clear();
            Console.Write("Syötä päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());

            calendarEvents = db.GetAllCalendarEvents();

            Console.WriteLine("Varaukset " + date + ".\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date.Date == date.Date)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
        }
        public Customer PrintGcCalendarEventsByCustomer() //Tulostaa asiakkaan varaukset ja palauttaa asiakkaan oliona.
        {
            customer = RequestCustomer();

            calendarEvents = db.GetAllCalendarEvents();
            int y = 0;
            Console.Clear();
            Console.WriteLine("Varaukset asiakkaalle " + customer.Name + ".\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == customer.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                    y++;
                }
            }
            if (y == 0)
            {
                Console.WriteLine("Asiakkaalla ei ole varauksia.");
                return null;
            }
            else
                return customer;
        }
        public void PrintGcCalendarEventsByTrainer() //Tulostaa varaukset valmentajan mukaan. 0 käyttöä toistaiseksi.
        {
            Console.Clear();
            Console.WriteLine("Valitse valmentaja jonka varaukset haluat nähdä.\n");
            trainer = RequestTrainer();

            calendarEvents = db.GetAllCalendarEvents();
            int y = 0;
            Console.WriteLine("Varaukset valmentajalle " + trainer.Name + ".\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].trainerID == trainer.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                    y++;
                }
            }
            if (y == 0)
            {
                Console.WriteLine("Valmentajalla ei ole varauksia.");
            }
        }
        public void PrintGcCalendarEventsByGroupClass() //Tulostaa varaukset ryhmäliikuntatunnin mukaan. 0 käyttöä toistaiseksi.
        {
            Console.Clear();
            Console.WriteLine("Valitse ryhmäliikuntatunti jonka varaukset haluat nähdä.\n");
            groupClass = RequestGroupClass();

            calendarEvents = db.GetAllCalendarEvents();
            int y = 0;
            Console.WriteLine("Varaukset ryhmäliikuntatunnille " + groupClass.ID + ".\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].classID == groupClass.ID)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                    y++;
                }
            }
            if (y == 0)
            {
                Console.WriteLine("Ryhmäliikuntatunnilla ei ole varauksia.");
            }
        }


        //Metodit, jotka kysyy ja palauttaa tietyn olion. Tarkistaa myös että arvo löytyy.
        public Customer RequestCustomer()//Metodi joka tulostaa asiakkaat ja palauttaa valitun asiakkaan oliona. Tarkistaa myös että arvo löytyy.
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
        public Trainer RequestTrainer()//Metodi joka tulostaa valmentajat ja palauttaa valitun valmentajan oliona. Tarkistaa myös että arvo löytyy.
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
        public GroupClass RequestGroupClass()//Metodi joka tulostaa ryhmäliikuntatunnit ja palauttaa valitun ryhmäliikuntatunnin oliona. Tarkistaa myös että arvo löytyy.
        {
            if (!PrintOnlyUpComingGc())
            {
                return null;
            }
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
        private GroupClass GetGroupClassByID(int groupClassID)//Metodi joka palauttaa ryhmäliikuntatunnin oliona ID:n perusteella.
        {
            groupClasses = db.GetGroupClasses();
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
        private CalendarEvent RequestPtReservation()
        {
            calendarEvents = db.GetPtReservations();
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
            Console.Write("\nSyötä varauksen ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].ID == id)
                {
                    return calendarEvents[i];
                }
                else if (i == calendarEvents.Count - 1)
                {
                    Console.WriteLine("\nVarausta ei löytynyt.");
                }
            }
            return null;
        }
        private CalendarEvent RequestGcReservation()
        {
            calendarEvents = db.GetAllCalendarEvents();
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
            Console.Write("\nSyötä varauksen ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].ID == id)
                {
                    return calendarEvents[i];
                }
                else if (i == calendarEvents.Count - 1)
                {
                    Console.WriteLine("\nVarausta ei löytynyt.");
                }
            }
            return null;
        }

        //Metodit, jotka tulostaa olioita eri puitteiden alla.
        public void PrintAllCustomers()//Tulostaa kaikki ryhmäliikuntatunnit.
        {
            customers = db.GetAllCustomers();
            Console.WriteLine("Asiakkaat:\n");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine(customers[i].ToString());
            }
        }
        public void PrintAllTrainers() //Tulostaa kaikki valmentajat.
        {
            trainers = db.GetAllTrainers();
            Console.WriteLine("Valmentajat:\n");
            for (int i = 0; i < trainers.Count; i++)
            {
                Console.WriteLine(trainers[i].ToString());
            }
        }
        public void PrintAllGroupClasses() //Tulostaa kaikki ryhmäliikuntatunnit.
        {
            groupClasses = db.GetGroupClasses();
            if (groupClasses.Count < 1)
            {
                Console.WriteLine("Ei ryhmäliikuntatunteja.");
                return;
            }
            Console.WriteLine("Ryhmäliikuntatunnit:");
            for (int i = 0; i < groupClasses.Count; i++)
            {
                Console.WriteLine(groupClasses[i].ToString());
            }
        }
        public void PrintAllGcCalendarEvents()   //Tulostaa kaikki varaukset.
        {
            calendarEvents = db.GetAllCalendarEvents();

            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei varauksia.");
                return;
            }

            Console.WriteLine("Varaukset:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        }
        public bool PrintOnlyUpComingGc()//Tulostaa vain tulevat ryhmäliikunta tunnit. Eli EI TULOSTA menneitä tunteja.
        {
            List<GroupClass> tempGcs = db.GetGroupClasses();
            if (tempGcs.Count < 1)
            {
                Console.WriteLine("Ei ryhmäliikuntatunteja.");
                return false;
            }
            groupClasses = new List<GroupClass>();
            for (int i = 0; i < tempGcs.Count; i++)
            {
                if (tempGcs[i].DateAndTime > DateTime.Now)
                    groupClasses.Add(tempGcs[i]);
            }
            for (int i = 0; i < groupClasses.Count; i++)
            {
                Console.WriteLine(groupClasses[i].ToString());
            }
            return true;
        }


        //Ryhmäliikuntatunteihin liittyvät metodit. Add, edit, delete jne.
        public void AddGroupClass()//Metodit, jolla luodaan uusi ryhmäliikuntatunti  
        {
            Console.WriteLine("Määritä valmentaja.");
            trainer = RequestTrainer();
            if (trainer == null)
                return;
            Console.Write("\nSyötä päivämäärä ja kellonaika muodossa PP-KK-VVVV HH:MM: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            if (date < DateTime.Now)
            {
                Console.WriteLine("Ryhmäliikuntatunnin aika ei voi olla pienempi kuin tämä päivä.");
                return;
            }
            else if (date.Hour < 12 || date.Hour > 21)
            {
                Console.WriteLine("Ryhmäliikuntatunnin aika voi olla vain välillä 12:00 - 21:00.");
                return;
            }
            Console.Write("\nSyötä kävijäraja: ");
            int visitorLimit = Convert.ToInt16(Console.ReadLine());

            string[] fields = { "trainer_ref", "dateandtime", "visitor_limit", "visitor_count" };
            string[] values = { trainer.ID.ToString(), date.ToString("yyyy-MM-dd HH:mm"), visitorLimit.ToString(), "0" };

            db.ExecuteInsertInto("group_class", fields, values);

            Console.WriteLine("\nRyhmäliikuntatunti lisätty tietokantaan onnistuneesti.");
        }
        public void EditGroupClass()//Muokkaa ryhmäliikuntatuntia
        {
            Console.WriteLine("Muokattavissa olevat ryhmäliikuntatunnit.\n");
            groupClass = RequestGroupClass();
            if (groupClass == null)
                return;
            bool run = true;
            int counter = 0;
            while (run)
            {
                Console.Clear();
                Console.WriteLine(groupClass.ToString());
                Console.WriteLine("\nMuokkaa ryhmäliikuntatuntia " + groupClass.ID + "\n");
                Console.WriteLine("1: Valmentaja\n2: Päivämäärä ja aika\n3: Kävijäraja\n0: Poistu\n");
                Console.Write("Syötä valinta: ");
                int value = Convert.ToInt16(Console.ReadLine());

                switch (value)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + groupClass.TrainerID + ". " + groupClass.TrainerName);
                        Console.WriteLine("Määritä uusi valmentaja.\n");
                        trainer = RequestTrainer();
                        if (trainer == null)
                            break;
                        groupClass.TrainerID = trainer.ID;
                        groupClass.TrainerName = trainer.Name;
                        counter++;
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + groupClass.DateAndTime.ToString("dd-MM-yyyy HH:mm"));
                        Console.Write("Syötä uusi päivämäärä ja aika muodossa PP-KK-VVVV HH:MM: ");
                        groupClass.DateAndTime = Convert.ToDateTime(Console.ReadLine());
                        if (groupClass.DateAndTime < DateTime.Now)
                        {
                            Console.WriteLine("Päivämäärä ei voi olla pienempi kuin tämä päivä.");
                            break;
                        }
                        counter++;
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + groupClass.VisitorLimit);
                        Console.Write("Syötä uusi kävijäraja: ");
                        int newValue = Convert.ToInt16(Console.ReadLine());
                        if (newValue >= groupClass.VisitorCount)
                        {
                            groupClass.VisitorLimit = newValue;
                        }
                        else if (newValue < groupClass.VisitorCount)
                        {
                            Console.WriteLine("Kävijäraja ei voi olla pienempi kuin nykyinen kävijämäärä.");
                            break;
                        }
                        counter++;
                        break;
                    case 0:
                        if (counter == 0)
                        {
                            return;
                        }
                        else if (counter > 0)
                        {
                            run = false;
                            break;
                        }
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine(groupClass.ToString());

            Console.Write("\nHaluatko tallentaa muutokset? K/E: ");
            string response = Console.ReadLine();
            if (response == "K" || response == "k")
            {
                string[] fields = { "trainer_ref", "dateandtime", "visitor_limit", "visitor_count" };
                string[] values = { groupClass.TrainerID.ToString(), groupClass.DateAndTime.ToString("yyyy-MM-dd HH:mm"), groupClass.VisitorLimit.ToString(), groupClass.VisitorCount.ToString() };
                string keyfield = "class_id";
                string keyValue = groupClass.ID.ToString();

                db.ExecuteUpdate("group_class", fields, values, keyfield, keyValue);
                Console.Clear();
                Console.WriteLine("\nMuutokset tallennettu onnistuneesti.");
            }
            else
            {
                Console.WriteLine("\nMuutokset hylätty.");
            }
        }
        public void AddCustomerToGroupClass() //Metodi joka lisää asiakkaan ryhmäliikuntatunnille. Kutsuu metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
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
            if (groupClass == null)
                return;
            if (groupClass.VisitorCount == groupClass.VisitorLimit)
            {
                Console.WriteLine("Ryhmäliikuntatunnilla ei ole enää tilaa.");
                return;
            }

            string[] fields = { "customer_ref", "class_ref" };
            string[] values = { customer.ID.ToString(), groupClass.ID.ToString() };

            db.ExecuteInsertInto("group_class_reservation", fields, values);

            IncreaseDecreaseGroupPtVisits(customer.ID, 1, 0);
            IncreaseDecreaseVisitorCount(groupClass.ID, 1, 1);
            Console.WriteLine("\nAsiakas lisätty ryhmäliikuntatunnille onnistuneesti. Jäljellä olevat ryhmäliikunta/pt kerrat: " + customer.GroupVisits);
        }
        public void RemoveCustomerFromGroupClass()  //Metodi, jolla poistetaan asiakas ryhmäliikuntatunnilta.
        {
            Console.WriteLine("Valitse asiakas kenet haluat poistaa ryhmäliikuntatunnilta.");

            customer = PrintGcCalendarEventsByCustomer();
            if (customer == null)
                return;
            Console.Write("Poistettavan varauksen ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            DeleteGroupClassReservationsByReservationID(id);
        }
        public void RemoveCustomerFromAllGroupClassesByID(int custID) //Poistaa asiakkaan kaikilta ryhmäliikuntatunneilta parametrina tulevan ID:n mukaan.
        {
            calendarEvents = db.GetAllCalendarEvents();
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == custID)
                {
                    DeleteGroupClassReservationsByReservationID(calendarEvents[i].ID);
                }
            }
        }
        public void DeleteGroupClass() //Metodi joka poistaa ryhmäliikuntatunnin. Kutsuu metodia joka poistaa varaukset.
        {
            Console.Clear();
            Console.WriteLine("Valitse ryhmäliikuntatunti jonka haluat poistaa.\n");
            //Kutsutaan metodia joka palauttaa valitun ryhmäliikuntatunnin oliona.
            groupClass = RequestGroupClass();
            if (groupClass == null)
            {
                return;
            }
            //Poistetaan ryhmäliikuntatunti tietokannasta.
            string keyfield = "class_id";
            string keyValue = groupClass.ID.ToString();
            //Kutsutaan metodia joka poistaa kyseisen ryhmäliikuntatunnin varaukset.
            DeleteGroupClassReservationsByGcID(Convert.ToInt16(keyValue));
            db.ExecuteDelete("group_class", keyfield, keyValue);
            Console.WriteLine("\nRyhmäliikuntatunti poistettu onnistuneesti.");
        }
        public void DeleteGroupClassByTrainerID(int trainerID)//Poistaa kaikki ryhmäliikuntatunnit valmentajan ID:n perusteella. Käytetään jos valmentaja poistetaan.
        {
            groupClasses = db.GetGroupClasses();
            for (int i = 0; i < groupClasses.Count; i++)
            {
                if (groupClasses[i].TrainerID == trainerID)
                {
                    DeleteGroupClassReservationsByGcID(groupClasses[i].ID);
                    string keyfield = "class_id";
                    string keyValue = groupClasses[i].ID.ToString();
                    db.ExecuteDelete("group_class", keyfield, keyValue);
                }
            }
        }

        //Valmentajan metodit.
        public void AddTrainer()//Lisää valmentajan tietokantaan.  
        {
            Console.Write("Valmentajan nimi: ");
            string name = Console.ReadLine();

            string[] fields = { "trainer_name" };
            string[] values = { name };

            db.ExecuteInsertInto("trainer", fields, values);

            Console.WriteLine("\nValmentaja lisätty tietokantaan onnistuneesti!");
        }
        public void DeleteTrainer()//Poistaa valmentajan tietokannasta.
        {
            Console.WriteLine("Valitse valmentaja jonka haluat poistaa.\n");
            trainer = RequestTrainer();

            string keyfield = "trainer_id";
            string keyValue = trainer.ID.ToString();

            DeleteGroupClassByTrainerID(trainer.ID);

            db.ExecuteDelete("trainer", keyfield, keyValue);

            Console.WriteLine("\nValmentaja poistettu onnistuneesti. Huomioi, että ryhmäliikuntatunnit, jossa valmentaja oli ovat poistettu myös.");
        }
        public void EditTrainer()//Muokkaa Valmentajan tietoja. (Vain nimi..)
        {
            Console.WriteLine("Valitse valmentaja jonka tietoja haluat muokata.\n");
            trainer = RequestTrainer();
            if (trainer == null)
                return;
            Console.Write("\nSyötä uusi nimi: ");
            string name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine(trainer.ToString());

            Console.Write("\nHaluatko tallentaa muutokset? K/E: ");
            string response = Console.ReadLine();
            if (response == "K" || response == "k")
            {
                string[] fields = { "trainer_name" };
                string[] values = { name };
                string keyfield = "trainer_id";
                string keyValue = trainer.ID.ToString();
                db.ExecuteUpdate("trainer", fields, values, keyfield, keyValue);

                Console.WriteLine("\nMuutokset tallennettu onnistuneesti.");
            }
            else
            {
                Console.WriteLine("\nMuutokset hylätty.");
                return;
            }

        }
        public void TrainerCustomerVisitCount() //Tulostaa valmentajan uniikit asiakkaat ja käynnit valitulta aikaväliltä.
        {
            trainers = db.GetAllTrainers();
            calendarEvents = db.GetPtReservations();
            Console.WriteLine("Miltä aikaväliltä haluat nähdä valmentajien asiakasmäärät?\n");
            Console.Write("Syötä alkupäivämäärä muodossa PP-KK-VVVV: ");
            DateTime startDate = Convert.ToDateTime(Console.ReadLine());
            Console.Write("\nSyötä loppupäivämäärä muodossa PP-KK-VVVV: ");
            DateTime endDate = Convert.ToDateTime(Console.ReadLine());

            for (int i = 0; i < trainers.Count; i++)
            {
                int counter = 0;
                HashSet<int> customerIDs = new HashSet<int>();

                for (int j = 0; j < calendarEvents.Count; j++)
                {
                    if (calendarEvents[j].trainerID == trainers[i].ID && calendarEvents[j].Date >= startDate && calendarEvents[j].Date <= endDate)
                    {
                        counter++;
                        customerIDs.Add(calendarEvents[j].customerID);
                    }
                }
                Console.WriteLine("Valmentaja: " + trainers[i].Name + ":");
                Console.WriteLine("Asiakkaita: " + customerIDs.Count);
                Console.WriteLine("Käyntejä: " + counter + "\n");
            }
        }

        //Asiakkaan metodit.

        public void AddCustomer()
        {
            Console.Write("Asiakkaan nimi muodossa ETUNIMI SUKUNIMI: ");
            string name = Console.ReadLine();
            Console.Write("\nSyntymäpäivä muodossa PP-KK-VVVV: ");
            DateTime birthDay = Convert.ToDateTime(Console.ReadLine());
            Console.Write("\nJäsenyyden päättymispäivä muodossa PP-KK-VVVV: ");
            DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());
            if (membershipEndDay < DateTime.Now)
            {
                Console.WriteLine("Jäsenyyden päättymispäivä ei voi olla pienempi kuin tämä päivä.");
                return;
            }
            Console.WriteLine();
            //Kysytään valmentaja metodia käyttäen.
            Console.WriteLine("Määritä valmentaja. Saatavilla olevat valmentajat:\n");
            trainer = RequestTrainer();
            int personalTrainerID = trainer.ID;
            Console.Write("\nRyhmä ja valmentaja käynnit: ");
            int groupVisits = Convert.ToInt16(Console.ReadLine());

            string[] fields = { "customer_name", "birthday", "trainer_ref", "gym_visits", "group_pt_visits", "membership_end" };
            string[] values = { name, birthDay.ToString("yyyy-MM-dd"), personalTrainerID.ToString(), "0", groupVisits.ToString(), membershipEndDay.ToString("yyyy-MM-dd") };

            db.ExecuteInsertInto("customer", fields, values);

            Console.WriteLine("\nAsiakas lisätty tietokantaan onnistuneesti!");
        } //Lisää asiakkaan.
        public void DeleteCustomer()
        {
            Console.WriteLine("Valitse asiakas jonka haluat poistaa.\n");
            customer = RequestCustomer();

            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            RemoveCustomerFromAllGroupClassesByID(customer.ID);

            db.ExecuteDelete("customer", keyfield, keyValue);

            Console.WriteLine("\nAsiakas poistettu onnistuneesti.");
        } //Poistaa asiakkaan.
        public void EditCustomer() //Muokkaa asiakkaan tietoja.
        {
            Console.WriteLine("Valitse asiakas jonka tietoja haluat muokata.\n");
            customer = RequestCustomer();
            if (customer == null)
                return;
            bool run = true;
            int counter = 0;
            while (run)
            {
                Console.Clear();
                Console.WriteLine(customer.ToString());
                Console.WriteLine("\nValitse muokattava tieto:\n");
                Console.WriteLine("1: Nimi\n2: Syntymäpäivä\n3: Valmentaja\n4: Jäsenyyden päättymispäivä\n0: Poistu\n");
                Console.Write("Syötä valinta: ");
                int value = Convert.ToInt16(Console.ReadLine());

                switch (value)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + customer.Name);
                        Console.WriteLine("Syötä uusi nimi muodossa ETUNIMI SUKUNIMI: ");
                        customer.Name = Console.ReadLine();
                        counter++;
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + customer.BirthDay.ToString("dd-MM-yyyy"));
                        Console.Write("Syötä uusi syntymäpäivä muodossa PP-KK-VVVV: ");
                        customer.BirthDay = Convert.ToDateTime(Console.ReadLine());
                        counter++;
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + customer.PersonalTrainerID + ". " + customer.PersonalTrainerName);
                        Console.WriteLine("Määritä uusi valmentaja. Saatavilla olevat valmentajat:\n");
                        trainer = RequestTrainer();
                        customer.PersonalTrainerID = trainer.ID;
                        customer.PersonalTrainerName = trainer.Name;
                        counter++;
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + customer.MembershipEndDay);
                        Console.WriteLine("Syötä uusi jäsenyyden päättymispäivä muodossa PP-KK-VVVV: ");
                        customer.MembershipEndDay = Convert.ToDateTime(Console.ReadLine());
                        counter++;
                        break;
                    case 0:
                        if (counter == 0)
                        {
                            return;
                        }
                        else if (counter > 0)
                        {
                            run = false;
                            break;
                        }
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine(customer.ToString());
            Console.Write("\nHaluatko tallentaa muutokset? K/E: ");
            string response = Console.ReadLine();
            if (response == "K" || response == "k")
            {
                string[] fields = { "customer_name", "birthday", "trainer_ref", "gym_visits", "group_pt_visits", "membership_end" };
                string[] values = { customer.Name.ToString(), customer.BirthDay.ToString("yyyy-MM-dd"), customer.PersonalTrainerID.ToString(), customer.GymVisits.ToString(), customer.GroupVisits.ToString(), customer.MembershipEndDay.ToString("yyyy-MM-dd") };
                string keyfield = "customer_id";
                string keyValue = customer.ID.ToString();

                db.ExecuteUpdate("customer", fields, values, keyfield, keyValue);
                Console.Clear();
                Console.WriteLine("\nMuutokset tallennettu onnistuneesti.");
            }
            else
            {
                Console.WriteLine("\nMuutokset hylätty.");
            }
        }
        public void PrintCustomerByNameID()
        {
            customers = db.GetAllCustomers();

            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{customers[i].ID + ":"} {customers[i].Name}");
            }
            Console.Write("\nSyötä asiakkaan ID tai Nimi: ");
            bool found = false;
            string response = Console.ReadLine();
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].ID.ToString() == response || customers[i].Name == response)
                {
                    Console.WriteLine();
                    Console.WriteLine(customers[i].ToString());
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("\nAsiakasta ei löytynyt.");
            }

        } //Tulostaa asiakkaan tiedot ID:n tai nimen perusteella.       
        public void TimesCustomerUsedTrainer() //Tulostaa asiakkaan valmentajakäyntien määrän.
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jonka valmentajakäynnit haluat nähdä.\n");
            customer = RequestCustomer();
            if (customer == null)
                return;
            calendarEvents = db.GetPtReservations();
            int counter = 0;
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == customer.ID)
                {
                    counter++;
                }
            }
            Console.WriteLine("\nAsiakas " + customer.Name + " on käynyt valmentajalla " + counter + " kertaa.");
        }
        public void TimesClassesAttended()
        {
            Console.WriteLine("Valitse asiakas jonka ryhmäliikuntatunti käynnit haluat nähdä.\n");
            customer = RequestCustomer();
            if (customer == null)
                return;
            calendarEvents = db.GetAllCalendarEvents();
            int counter = 0;
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == customer.ID)
                {
                    counter++;
                }
            }
            Console.WriteLine("\nAsiakas " + customer.Name + " on käynyt ryhmäliikuntatunneilla " + counter + " kertaa.");
        } //Tulostaa asiakkaan ryhmäliikuntatunti käynnit.
        public void AddGroupPtVisits() //Lisää asiakkaalle ryhmäliikunta/pt käyntejä.
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas.\n");
            customer = RequestCustomer();
            if (customer == null)
            {
                return;
            }
            Console.WriteLine("Haluatko lisätä vai vähentää ryhmäliikuntatunteja? L/V.");
            string response = Console.ReadLine();
            if (response == "L" || response == "l")
            {
                Console.Write("\nKuinka monta käyntiä haluat lisätä: ");
                int value = Convert.ToInt16(Console.ReadLine());
                IncreaseDecreaseGroupPtVisits(customer.ID, value, 1);
                Console.WriteLine("\nKäynnit lisätty onnistuneesti. " + customer.Name + " ryhmä & valmentaja käyttökerrat: " + customer.GroupVisits);
            }
            else if (response == "V" || response == "v")
            {
                Console.Write("\nKuinka monta käyntiä haluat vähentää: ");
                int value = Convert.ToInt16(Console.ReadLine());
                if (customer.GroupVisits - value < 0)
                {
                    Console.WriteLine("Asiakkaalla ei ole tarpeeksi käyntejä vähennettäväksi.");
                    return;
                }
                else
                    IncreaseDecreaseGroupPtVisits(customer.ID, value, 0);
                Console.WriteLine("\nKäynnit vähennetty onnistuneesti. " + customer.Name + " ryhmä & valmentaja käyttökerrat: " + customer.GroupVisits);
            }

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
        }//Määrittää asiakkaalle uuden valmentajan.
        public void AssignNewMembershipEnd() //Määrittää asiakkaalle uuden jäsenyyden päättymispäivän.
        {
            Console.Clear();
            Console.WriteLine("Valitse asiakas jolle haluat määrittää uuden jäsenyyden päättymispäivän.\n");
            customer = RequestCustomer();

            Console.Write("\nUusi jäsenyyden päättymispäivä muodossa PP-KK-VVVV: ");
            DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());

            string[] fields = { "membership_end" };
            string[] values = { membershipEndDay.ToString("yyyy-MM-dd") };
            string keyfield = "customer_id";
            string keyValue = customer.ID.ToString();

            db.ExecuteUpdate("customer", fields, values, keyfield, keyValue);
        }

        //Ryhmäliikuntatuntien varauksien metodit.
        public void EditGroupClassReservation() //Muokkaa ryhmäliikuntatunnin varauksia.
        {
            Console.WriteLine("Muokattavissa olevat varaukset.\n");
            calendarEvent = RequestGcReservation();
            if (calendarEvent == null)
                return;
            int ogGc = calendarEvent.classID;
            int ogCust = calendarEvent.customerID;
            bool run = true;
            bool customerChanged = false;
            bool groupClassChanged = false;
            while (run)
            {
                Console.Clear();
                Console.WriteLine(calendarEvent.ToString());
                Console.WriteLine("\n1: Asiakas\n2: Ryhmäliikuntatunti\n0: Poistu\n");
                Console.Write("Syötä valinta: ");
                char value = Convert.ToChar(Console.ReadLine());

                switch (value)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + calendarEvent.customerID + ". " + calendarEvent.customerName);
                        Console.WriteLine("Valitse uusi asiakas.\n");
                        customer = RequestCustomer();
                        if (customer == null)
                            break;
                        calendarEvent.customerID = customer.ID;
                        calendarEvent.customerName = customer.Name;
                        customerChanged = true;
                        break;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + calendarEvent.classID + ".");
                        Console.WriteLine("Valitse uusi ryhmäliikuntatunti.\n");
                        groupClass = RequestGroupClass();
                        calendarEvent.classID = groupClass.ID;
                        groupClassChanged = true;
                        break;
                    case '0':
                        if (customerChanged == false && groupClassChanged == false)
                        {
                            return;
                        }
                        else
                        {
                            run = false;
                            break;
                        }
                }
            }
            Console.Clear();
            Console.WriteLine(calendarEvent.ToString());
            Console.Write("\nHaluatko tallentaa muutokset? K/E: ");
            string response = Console.ReadLine();
            if (response == "K" || response == "k")
            {
                string[] fields = { "customer_ref", "class_ref" };
                string[] values = { calendarEvent.customerID.ToString(), calendarEvent.classID.ToString() };
                string keyfield = "reservation_id";
                string keyValue = calendarEvent.ID.ToString();

                db.ExecuteUpdate("group_class_reservation", fields, values, keyfield, keyValue);
                if (customerChanged == true)
                {
                    IncreaseDecreaseGroupPtVisits(ogCust, 1, 1);
                    IncreaseDecreaseGroupPtVisits(calendarEvent.customerID, 1, 0);
                }
                if (groupClassChanged == true)
                {
                IncreaseDecreaseVisitorCount(groupClass.ID, 1, 1);
                IncreaseDecreaseVisitorCount(ogGc, 1, 0);
                }               
                Console.Clear();
                Console.WriteLine("\nMuutokset tallennettu onnistuneesti.");
            }
            else
            {
                Console.WriteLine("\nMuutokset hylätty.");
            }           
        }
        private void DeleteGroupClassReservationsByGcID(int groupClassID)
        {
            calendarEvents = db.GetAllCalendarEvents(); //haetaan kaikki varaukset.

            for (int i = 0; i < calendarEvents.Count ; i++) //Looppi joka tarkistaa kaikki varaukset, sekä kutsuu metodia joka poistaa varauksen.
            {
                if (calendarEvents[i].classID == groupClassID)
                {
                    db.ExecuteDelete("group_class_reservation", "class_ref", groupClassID.ToString());
                    //Kutsutaan metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
                    IncreaseDecreaseGroupPtVisits(calendarEvents[i].customerID, 1, 1);
                }
            }
        }//Poistaa ryhmäliikuntatunnin varaukset ID:n perusteella. Kutsuu metodia joka lisää tai vähentää asiakkaan ryhmäliikuntakertoja.
        private void DeleteGroupClassReservationsByReservationID(int reservationId) 
        {
            calendarEvents = db.GetAllCalendarEvents();

            for (int i = 0;i < calendarEvents.Count ; i++) 
            {
                if (calendarEvents[i].ID == reservationId)
                {
                    db.ExecuteDelete("group_class_reservation", "reservation_id", reservationId.ToString());

                    IncreaseDecreaseGroupPtVisits(calendarEvents[i].customerID, 1, 1);
                    IncreaseDecreaseVisitorCount(calendarEvents[i].classID, 1, 0);
                }
            }
        }//Poistaa ryhmäliikuntatunnin varauksen ID:n perusteella.       
        private void IncreaseDecreaseGroupPtVisits(int customerID, int increaseValue, int incrDecr) //Metodi joka vähentää tai lisää asiakkaan ryhmäliikuntakertoja tietokantaan. Kolmas parametri määrittää lisätäänkö vai vähennetäänkö. Säästetään vähän tilaa :-D
        {
            customer = db.GetCustomerByID(customerID);
            //Haetaan asiakas tietokannasta.
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
            db.ExecuteUpdate("customer", fields, values, "customer_id", customerID.ToString());
        }      
        private void IncreaseDecreaseGymVisits(Customer c, int increaseValue, int incrDecr) //Lisää tai vähentää asiakkaan kuntosalikäyntejä yhteensä.
        {
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

            db.ExecuteUpdate("customer", fields, values, "customer_id", c.ID.ToString());
        }      
        private void IncreaseDecreaseVisitorCount(int groupClassID, int increaseValue, int incrDecr) //Lisää tai vähentää ryhmäliikuntatunnin kävijämäärää.
        {
            groupClass = GetGroupClassByID(groupClassID);
            if (incrDecr == 1)
            {
                groupClass.VisitorCount += increaseValue;
            }
            else if (incrDecr == 0)
            {
                groupClass.VisitorCount -= increaseValue;
            }
            string[] fields = { "visitor_count" };
            string[] values = { groupClass.VisitorCount.ToString() };

            db.ExecuteUpdate("group_class", fields, values, "class_id", groupClass.ID.ToString());
        }

        //Käyntikertojen merkkaus, tulostus jne.
        public void MarkGymVisit()
        {
            Console.WriteLine("Valitse asiakas jolle haluat lisätä kuntosalikäynnin.\n");
            customer = RequestCustomer();

            IncreaseDecreaseGymVisits(customer, 1, 1);
            Console.WriteLine("\nKäynti lisätty onnistuneesti. Asiakkaan: " + customer.Name + " sali käynnit yhteensä: " + customer.GymVisits);
            string[] fields = { "customer_ref", "dateandtime" };
            string[] values = { customer.ID.ToString(), DateTime.Now.ToString() };
            db.ExecuteInsertInto("gym_visit", fields, values);
        }//Kysyy asiakkaan, jonka jälkeen lisää yhden käynnin.
        public void MarkGymVisitByID(int ID, DateTime time)
        {             customer = db.GetCustomerByID(ID);
                   IncreaseDecreaseGymVisits(customer, 1, 1);
            Console.WriteLine("\nKäynti lisätty onnistuneesti. Asiakkaan: " + customer.Name + " sali käynnit yhteensä: " + customer);
            string[] fields = { "customer_ref", "dateandtime" };
            string[] values = { customer.ID.ToString(), time.ToString() };
            db.ExecuteInsertInto("gym_visit", fields, values);
        }//ei käyttöä toistaiseksi.
        public void PrintAllGymVisits()
        {
            calendarEvents = db.GetAllGymVisits();
            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei kuntosalikäyntejä.");
                return;
            }

            Console.WriteLine("Kuntosalikäynnit:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        } //Tulostaa kaikki kuntosalikäynnit.
        public void PrintGymVisitsByCustID()
        {
            Console.WriteLine("Valitse asiakas jonka kuntosalikäynnit haluat nähdä.\n");
            customer = RequestCustomer();
            calendarEvents = db.GetGymVisitsByCustID(customer.ID);
            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei kuntosalikäyntejä.");
                return;
            }
            Console.Clear();
            Console.WriteLine(customer.Name + "kuntosalikäynnit:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        }//Tulostaa asiakkaan kuntosalikäynnit.
        public void PrintGymVisitsFromTime( )
        {
            Console.Write("Syötä ensimmäinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date1 = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Syötä toinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date2 = Convert.ToDateTime(Console.ReadLine());
            Console.Clear(); 
            calendarEvents = db.GetGymVisitsFromTime(date1, date2);
            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei kuntosalikäyntejä.");
                return;
            }
            Console.WriteLine("Kuntosalikäynnit ajalta " + date1.ToString("dd.MM.yyyy") + " - " + date2.ToString("dd.MM.yyyy") + "\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].ToString());
            }
        }//Tulostaa kuntosalikäynnit aikavälillä.

        //Personal trainer käyntien metodit.
        public void AddPtReservation()//Lisää asiakkaalle personal trainer käynnin.
        {
            Console.WriteLine("Valitse asiakas jolle haluat lisätä personal trainer käynnin.\n");
            customer = RequestCustomer();
            if (customer == null)
            { return; }
            Console.WriteLine("\nValitse PT käynnin valmentaja. Asiakkaan henkilökohtainen pt: " + customer.PersonalTrainerID + ". " + customer.PersonalTrainerName + "\n");
            trainer = RequestTrainer();
            if (trainer == null)
            { return; }
            Console.Write("\nSyötä päivämäärä ja aika muodossa PP-KK-VVVV HH:MM: ");
            DateTime date = Convert.ToDateTime(Console.ReadLine());
            if (date < DateTime.Now)
            {
                Console.WriteLine("Personal trainer käynnin aika ei voi olla pienempi kuin tämä päivä.");
                return;
            }
            string[] fields = { "customer_ref", "trainer_ref", "dateandtime" };
            string[] values = { customer.ID.ToString(), trainer.ID.ToString(), date.ToString("yyyy-MM-dd HH:mm") };
            Console.WriteLine("Personal trainer käynti lisätty onnistuneesti.");
            db.ExecuteInsertInto("pt_reservation", fields, values);
            IncreaseDecreaseGroupPtVisits(customer.ID, 1, 0);
        }
        public void RemovePtReservation()//Poistaa personal trainer käynnin
        {
            PrintPtReservations();
            Console.Write("\nPoistettavan varauksen ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < calendarEvents.Count; i++)
            {
                    if (calendarEvents[i].ID == id)
                {
                    IncreaseDecreaseGroupPtVisits(calendarEvents[i].customerID, 1, 1);
                    db.ExecuteDelete("pt_reservation", "reservation_id", id.ToString());
                    Console.WriteLine("Varaus poistettu onnistuneesti.");
                }
            }
        }
        public void EditPtReservation()//Muokkaa personal trainer käyntiä.
        {
            bool run = true;
            int counter = 0;
            calendarEvent = RequestPtReservation();        

            while (run)
            {
                Console.Clear();
                Console.WriteLine(calendarEvent.PtVisitToString());
                Console.WriteLine("Valitse muokattava tieto:\n");
                Console.WriteLine("1: Asiakas\n2: Valmentaja\n3: Päivämäärä\n0: Poistu\n");
                Console.Write("Syötä valinta: ");
                char value = Convert.ToChar(Console.ReadLine());
                switch (value)
                {

                    case '1':
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + calendarEvent.customerName + " ID: " + calendarEvent.customerID + "\n");
                        Console.WriteLine("Valitse uusi asiakas varaukselle.\n");
                        customer = RequestCustomer();
                        if (customer == null)
                        {
                            return;
                        }
                        calendarEvent.customerName = customer.Name;
                        calendarEvent.customerID = customer.ID;
                        counter++;
                    break;

                    case '2':
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + calendarEvent.trainerName + " ID: " + calendarEvent.trainerID + "\n");
                        Console.WriteLine("Valitse uusi valmentaja varaukselle.\n");
                        trainer = RequestTrainer();
                        if (trainer == null)
                        {
                            return;
                        }
                        calendarEvent.trainerName = trainer.Name;
                        calendarEvent.trainerID = trainer.ID;
                        counter++;
                    break;

                    case '3':
                        Console.Clear();
                        Console.WriteLine("Nykyinen arvo: " + calendarEvent.Date.ToString("dd.MM.yyyy HH:mm") + "\n");
                        Console.Write("Syötä uusi päivämäärä ja aika muodossa PP-KK-VVVV HH:MM: ");
                        calendarEvent.Date = Convert.ToDateTime(Console.ReadLine());
                        if (calendarEvent.Date < DateTime.Now)
                        {
                            Console.WriteLine("Personal trainer käynnin aika ei voi olla pienempi kuin tämä päivä.");
                            return;
                        }
                        counter++;
                    break;
                    
                    case '0':
                        if (counter == 0)
                        {
                            return;
                        }
                        else
                            run = false;
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine("Haluatko tallentaa muutokset? K/E: ");
            string response = Console.ReadLine();
            if (response == "K" || response == "k")
            {
                string[] fields = { "customer_ref", "trainer_ref", "dateandtime" };
                string[] values = { calendarEvent.customerID.ToString(), calendarEvent.trainerID.ToString(), calendarEvent.Date.ToString("yyyy-MM-dd HH:mm") };
                string keyfield = "reservation_id";
                string keyValue = trainer.ID.ToString();
                db.ExecuteUpdate("pt_reservation", fields, values, keyfield, keyValue);
                Console.Clear();
                Console.WriteLine("Muutokset tallennettu onnistuneesti.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Muutokset hylätty.");
            }



        }
        public void PrintPtReservations()//Tulostaa kaikki personal trainer käynnit.
        {
            calendarEvents = db.GetPtReservations();
            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei personal trainer käyntejä.");
                return;
            }
            Console.WriteLine("Personal trainer käynnit:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                Console.WriteLine(calendarEvents[i].PtVisitToString());
            }
        }
        public void PrintPtReservationsByPT()//Tulostaa personal trainer käynnit valmentajan mukaan.
        {
            calendarEvents = db.GetPtReservations();

            Console.WriteLine("Valitse valmentaja jonka personal trainer käynnit haluat nähdä.\n");
            trainer = RequestTrainer();
            if (trainer == null)
            {
                return;
            }
            Console.Clear();
            Console.WriteLine(trainer.Name + " varatut ajat:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].trainerID == trainer.ID)
                {
                    Console.WriteLine(calendarEvents[i].PtVisitToString());
                }
            }
        }
        public void PrintPtReservationsByCustID()//Tulostaa personal trainer käynnit asiakkaan mukaan.
        {
            Console.WriteLine("Valitse asiakas jonka personal trainer käynnit haluat nähdä.\n");
            customer = RequestCustomer();
            calendarEvents = db.GetPtReservations();
            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei personal trainer käyntejä.");
                return;
            }
            Console.Clear();
            Console.WriteLine(customer.Name + " personal trainer käynnit:\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].customerID == customer.ID)
                {
                    Console.WriteLine(calendarEvents[i].PtVisitToString());
                }
            }
        }
        public void PrintPtReservationsFromTo()//Tulostaa personal trainer käynnit aikavälillä.
        {
            Console.WriteLine("Valitse aika millä aikavälillä haluat nähdä personal trainer käynnit.\n");
            Console.Write("Syötä ensimmäinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date1 = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Syötä toinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date2 = Convert.ToDateTime(Console.ReadLine());
            calendarEvents = db.GetPtReservations();

            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei personal trainer käyntejä.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Personal trainer käynnit ajalta " + date1.ToString("dd.MM.yyyy") + " - " + date2.ToString("dd.MM.yyyy") + "\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date >= date1 && calendarEvents[i].Date <= date2)
                {
                    Console.WriteLine(calendarEvents[i].PtVisitToString());
                }
            }
        }
        public void PrintPtReservationsFromToByPT()//Tulostaa personal trainer käynnit valmentajan mukaan aikavälillä.
        {
            Console.WriteLine("Valitse valmentaja jonka personal trainer käynnit haluat nähdä.\n");
            trainer = RequestTrainer();
            if (trainer == null)
            {
                return;
            }
            Console.Write("Syötä ensimmäinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date1 = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Syötä toinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date2 = Convert.ToDateTime(Console.ReadLine());
            calendarEvents = db.GetPtReservations();

            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei personal trainer käyntejä.");
                return;
            }

            Console.Clear();
            Console.WriteLine(trainer.Name + " personal trainer käynnit ajalta " + date1.ToString("dd.MM.yyyy") + " - " + date2.ToString("dd.MM.yyyy") + "\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date >= date1 && calendarEvents[i].Date <= date2 && calendarEvents[i].trainerID == trainer.ID)
                {
                    Console.WriteLine(calendarEvents[i].PtVisitToString());
                }
            }
        }
        public void PrintPtReservationsFromToByCustID()//Tulostaa personal trainer käynnit asiakkaan mukaan aikavälillä.
        {
            Console.WriteLine("Valitse asiakas jonka personal trainer käynnit haluat nähdä.\n");
            customer = RequestCustomer();
            if (customer == null)
            {
                return;
            }
            Console.Write("Syötä ensimmäinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date1 = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Syötä toinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date2 = Convert.ToDateTime(Console.ReadLine());
            calendarEvents = db.GetPtReservations();

            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei personal trainer käyntejä.");
                return;
            }

            Console.Clear();
            Console.WriteLine(customer.Name + " personal trainer käynnit ajalta " + date1.ToString("dd.MM.yyyy") + " - " + date2.ToString("dd.MM.yyyy") + "\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date >= date1 && calendarEvents[i].Date <= date2 && calendarEvents[i].customerID == customer.ID)
                {
                    Console.WriteLine(calendarEvents[i].PtVisitToString());
                }
            }
        }
        public void PrintGymVisitsFromToByCustID()//Tulostaa asiakkaan kuntosalikäynnit aikavälillä.
        {
            Console.WriteLine("Valitse asiakas jonka kuntosalikäynnit haluat nähdä.\n");
            customer = RequestCustomer();
            if (customer == null)
            {
                return;
            }
            Console.Write("Syötä ensimmäinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date1 = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Syötä toinen päivämäärä muodossa PP-KK-VVVV: ");
            DateTime date2 = Convert.ToDateTime(Console.ReadLine());
            calendarEvents = db.GetGymVisitsByCustID(customer.ID);

            if (calendarEvents.Count < 1)
            {
                Console.WriteLine("Ei kuntosalikäyntejä.");
                return;
            }

            Console.Clear();
            Console.WriteLine(customer.Name + " kuntosalikäynnit ajalta " + date1.ToString("dd.MM.yyyy") + " - " + date2.ToString("dd.MM.yyyy") + "\n");
            for (int i = 0; i < calendarEvents.Count; i++)
            {
                if (calendarEvents[i].Date >= date1 && calendarEvents[i].Date <= date2)
                {
                    Console.WriteLine(calendarEvents[i].ToString());
                }
            }
        }

        //MUUT METODIT jos tulee.
    }
}
