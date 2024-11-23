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
    public class dbConnection //Luokka jolla yhdistetään DATABASEEN.
    {
        private string appLocation = AppDomain.CurrentDomain.BaseDirectory; //Tämä hakee ohjelman sijainnin.
        private string databasePath; //koko tietokannan sijainti tulee tähän rakentajassa.
        public OleDbConnection myConnection;
        public OleDbCommand myCommand;
        public OleDbDataReader myReader;

        public List<Customer> customers = new List<Customer>();
        public List<PersonalTrainer> personalTrainers = new List<PersonalTrainer>();
        public List<GroupClass> groupClass = new List<GroupClass>();

        public dbConnection()
        {

            databasePath = Path.GetFullPath(Path.Combine(appLocation, @"..\..\..", @"Kettera_console\Data\Kettera.accdb"));

            // Testaa löytyykö tiedosto ja tulostaa tuloksen, kommentoitu pois koska homma toimii toistaiseksi.
            /*
             * Console.WriteLine("Tietokannan polku: " + databasePath);
            if (!File.Exists(databasePath))
            {
                Console.WriteLine("Tietokantatiedostoa ei löydy: " + databasePath);
                return;
            }
            else if (File.Exists(databasePath))
            {
                Console.WriteLine("Tietokantatiedosto löytyi: " + databasePath);
            }
            */

            databasePath = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = " + databasePath;

            myConnection = new OleDbConnection(); //luo OleDbConnection tyyppisen olion.
            myConnection.ConnectionString = databasePath; //Asettaa connectionstringin tietokannan sijainniksi.         
            myConnection.Open(); //Avaa yhteyden. KOMMENTOITU POIS KOSKA TOISTAISEKSI TOTEUTUS MUUALLA.


        
            //luodaan komento.
            myCommand = new OleDbCommand(); //luo OleDbCommand tyyppisen olion. Tällä tehdään muokkaukset tietokantaan.
            myCommand.Connection = myConnection; //yhdistää komennon yhteyteen! :D

            // Komentoa muutetaan tällä tavalla:
            // myCommand.CommandText = "INSERT INTO Taulu (Kenttä1, Kenttä2) VALUES ('Arvo1', 'Arvo2')";
            // myCommand.CommandType = CommandType.Text; //Tämä kertoo että kyseessä on tekstikomento.
        }

        //TÄSTÄ ETEENPÄIN METODIT JOILLA VOIDAAN TEHDÄ KOMENTOJA TIETOKANTAAN.

        public bool GetInfo()
        {
            //Ensin haetaan PT tiedot.
            myCommand.CommandText = "SELECT * FROM trainer";
            myCommand.CommandType = CommandType.Text;
            myReader = myCommand.ExecuteReader();
            bool notEoF = myReader.Read();

            while (notEoF)
            {
                PersonalTrainer pt = new PersonalTrainer();
                pt.ID = Convert.ToInt16 (myReader["trainer_id"]);
                pt.Name = myReader["trainer_name"].ToString();
                personalTrainers.Add(pt);
                notEoF = myReader.Read();
            }


            //Seuraavaksi haetaan asiakkaat.
            myCommand.CommandText = "SELECT c.*, t.trainer_name FROM customer c LEFT JOIN trainer t ON t.trainer_id = c.trainer_ref ORDER BY c.customer_id;";
            myCommand.CommandType = CommandType.Text;
            myReader.Close();
            myReader = myCommand.ExecuteReader();
            //Ja tehdään tiedoista customer olioita.
            notEoF = myReader.Read();

            while (notEoF)
            {                 
                Customer c = new Customer();
                c.ID = Convert.ToInt16(myReader["customer_id"]);
                c.Name = myReader["customer_name"].ToString();
                c.BirthDay = Convert.ToDateTime(myReader["birthday"]);   
                
                //haetaan asiakkaan PT:n nimi. 
                c.PersonalTrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                int temp = Convert.ToInt16(myReader["trainer_ref"]);
                c.PersonalTrainerName = myReader["trainer_name"].ToString();

                c.GymVisits = Convert.ToInt16(myReader["gym_visits"]);
                c.GroupVisits = Convert.ToInt16(myReader["group_pt_visits"]);
                c.MembershipEndDay = c.BirthDay = Convert.ToDateTime(myReader["membership_end"]);
                customers.Add(c);
                notEoF = myReader.Read();
            }

            //Haetaan ryhmäliikuntatunnit. 
            myCommand.CommandText = "SELECT g.*, t.trainer_name FROM group_class g LEFT JOIN trainer t ON t.trainer_id = g.trainer_ref ORDER BY dateandtime ASC;";
            myCommand.CommandType = CommandType.Text;
            myReader.Close();
            myReader = myCommand.ExecuteReader();
            notEoF = myReader.Read();
            while (notEoF)
            {
                GroupClass gc = new GroupClass();
                gc.ID = Convert.ToInt16(myReader["class_id"]);
                gc.TrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                gc.TrainerName = myReader["trainer_name"].ToString();
                gc.DateAndTime = Convert.ToDateTime(myReader["dateandtime"]);
                gc.VisitorLimit = Convert.ToInt16(myReader["visitor_limit"]);
                gc.VisitorCount = Convert.ToInt16(myReader["visitor_count"]);
                groupClass.Add(gc);
                notEoF = myReader.Read();             
            }
            myReader.Close();


            return true;

        }

        public bool executeQuery(string query)
        {
            try
            {
                myCommand.CommandText = query;
                myCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nKyselyn suorittaminen epäonnistui. Virhe: " + ex.Message);
                return false;
            }
        }

        public Customer RequestCustomer() //Tulostaa asiakkaat ja palauttaa valitun asiakkaan ID:n. Tarkistaa myös että arvo löytyy
        {
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{customers[i].ID}: {customers[i].Name}");
            }
            Console.Write("\nSyötä asiakkaan ID: ");
            try
            {
            int customerID = Convert.ToInt16(Console.ReadLine());
            for (int i = 0; i < customers.Count; i++)
                {
                    if (customers[i].ID == customerID)
                    {
                        return customers[i];
                    }
                    else if (i == customers.Count - 1)
                    {
                        Console.WriteLine("\nAsiakasta ei löytynyt. Paina ENTER jatkaaksesi.");
                        Console.ReadLine();
                        Console.Clear();

                    }
                }
            return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nAsiakkaan ID:n syöttäminen epäonnistui. Virhe: " + ex.Message);
                Console.WriteLine("Paina ENTER jatkaaksesi.");
                Console.ReadLine();
                Console.Clear();
                return null; ;
            }         
        }

        public PersonalTrainer RequestTrainerID()
        {
            Console.WriteLine("Määritä valmentaja. Saatavilla olevat valmentajat:");
            for (int i = 0; i < personalTrainers.Count; i++)
            {
                Console.WriteLine($"{personalTrainers[i].ID}: {personalTrainers[i].Name}");
            }
            Console.Write("\nSyötä valmentajan ID: ");
            try
            {
                int trainerID = Convert.ToInt16(Console.ReadLine());
                for (int i = 0; i < personalTrainers.Count; i++)
                {
                    if (personalTrainers[i].ID == trainerID)
                    {
                        return personalTrainers[i];
                    }
                    else if (i == personalTrainers.Count - 1)
                    {
                        Console.WriteLine("\nValmentajaa ei löytynyt. Paina ENTER jatkaaksesi.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nValmentajan ID:n syöttäminen epäonnistui. Virhe: " + ex.Message);
                Console.WriteLine("Paina ENTER jatkaaksesi.");
                Console.ReadLine();
                Console.Clear();
                return null;
            }
        }

        public GroupClass RequestGroupClass()
        {
            Console.WriteLine("Valitse ryhmäliikuntatunti jolle haluat lisätä asiakkaan. Saatavilla olevat tunnit:");
            for (int i = 0; i < groupClass.Count; i++)
            {
                Console.WriteLine(groupClass[i].ToString());
            }
            Console.Write("\nSyötä ryhmäliikuntatunnin ID: ");
            try
            {
                int groupClassID = Convert.ToInt16(Console.ReadLine());
                for (int i = 0; i < groupClass.Count; i++)
                {
                    if (groupClass[i].ID == groupClassID)
                    {
                        return groupClass[i];
                    }
                    else if (i == groupClass.Count - 1)
                    {
                        Console.WriteLine("\nRyhmäliikuntatuntia ei löytynyt. Paina ENTER jatkaaksesi.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nRyhmäliikuntatunnin ID:n syöttäminen epäonnistui. Virhe: " + ex.Message);
                Console.WriteLine("Paina ENTER jatkaaksesi.");
                Console.ReadLine();
                Console.Clear();
                return null;
            }
        }

        public void PrintCustomersAndTrainers()
        {
            Console.WriteLine("Valmentajat:");
            for (int i = 0; i < personalTrainers.Count; i++)
            {
                Console.WriteLine(personalTrainers[i].ToString());
            }
            Console.WriteLine("\nAsiakkaat:");
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine(customers[i].ToString());
            }
        }

        public void PrintGroupClasses()
        {
            Console.WriteLine("Ryhmäliikuntatunnit:");
            for (int i = 0; i < groupClass.Count; i++)
            {
                Console.WriteLine(groupClass[i].ToString());
            }
        }
    }

    public class CalendarEvent
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int customerID { get; set; }
        public int class_ref { get; set; }

        public override string ToString()
        {
            return $"Reservation ID: {ID}, Class ID: {class_ref}, Date: {Date.ToString("dd-MM-yyyy HH:mm")}, Customer ID: {customerID}";
        }
    }
    public class GroupClass
    {
        public int ID { get; set; }
        public int TrainerID { get; set; }
        public string TrainerName { get; set; }
        public DateTime DateAndTime { get; set; }
        public int VisitorLimit { get; set; }
        public int VisitorCount { get; set; }

        public GroupClass()
        {
            ID = 0;
            TrainerID = 0;
            TrainerName = "";
            DateAndTime = DateTime.Now;
            VisitorLimit = 0;
            VisitorCount = 0;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Trainer: {TrainerName}, Date and time: {DateAndTime.ToString("dd-MM-yyyy HH:mm")}";
        }

        public void AddGroupClass()
        {
            try
            {
                dbConnection db = new dbConnection();
                string query = "INSERT INTO group_class (trainer_ref, dateandtime, visitor_limit, visitor_count)" +
                                "VALUES ('" + TrainerID + "','" + DateAndTime.ToString("yyyy-MM-dd HH:mm") + "','" + VisitorLimit + "','" + VisitorCount + "');";
                db.executeQuery(query);
                db.myConnection.Close();
                Console.WriteLine("\nRyhmäliikuntatunti lisätty tietokantaan onnistuneesti! Paina ENTER jatkaaksesi.");
                Console.ReadLine();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nRyhmäliikuntatunnin lisääminen epäonnistui. Virhe: " + ex.Message);
            }
        }
    }

    public class PersonalTrainer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}";
        }
    }

    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime MembershipEndDay { get; set; }
        public int PersonalTrainerID { get; set; }
        public string PersonalTrainerName { get; set; }
        public int GymVisits { get; set; }
        public int GroupVisits { get; set; }

        public Customer()
        {
            ID = 0;
            Name = "";
            BirthDay = DateTime.Now;
            MembershipEndDay = DateTime.Now;
            PersonalTrainerID = 0;
            GymVisits = 0;
            GroupVisits = 0;
        }
        public Customer(string newName, DateTime newBirthDay, int newPersonalTrainerID, int newGroupVisits, DateTime newMembershipEndDay) : this()
        {
            Name = newName;
            BirthDay = newBirthDay;
            MembershipEndDay = newMembershipEndDay;
            PersonalTrainerID = newPersonalTrainerID;
            GymVisits = 0;
            GroupVisits = newGroupVisits;
        }

        public override string ToString()
        {

            return $"ID: {ID}, Name: {Name}, Birthday: {BirthDay.ToString("dd-MM-yyyy")}, Membership end day: {MembershipEndDay.ToString("dd-MM-yyyy")}, Personal trainer: {PersonalTrainerName}, Gym visits: {GymVisits}, Group visits left: {GroupVisits}";
        }

        public void AddCustomer()
        { 
            try
            {
                dbConnection db = new dbConnection();
                string query = "INSERT INTO customer (customer_name, birthday, trainer_ref, gym_visits, group_pt_visits, membership_end) " +
                "VALUES ('" + Name + "','" + BirthDay + "','" + PersonalTrainerID + 
                "','" + GymVisits + "','" + GroupVisits + "','" + MembershipEndDay + "')";
                db.executeQuery(query);
                db.myConnection.Close();

                Console.WriteLine("\nAsiakas lisätty tietokantaan onnistuneesti! Paina ENTER jatkaaksesi");
                Console.ReadLine();
                Console.Clear();
            }
            
            catch (Exception ex) 
            { 
                Console.WriteLine("\nAsiakkaan lisääminen tietokantaan epäonnistui. Virhe: " + ex.Message);
            }            
        }

        public bool AssignNewTrainer()
        {
            try
            {
            dbConnection db = new dbConnection();
            db.GetInfo();
            PersonalTrainer tempPT = db.RequestTrainerID();

            string query = "UPDATE customer SET trainer_ref = " + tempPT.ID + " WHERE customer_id = " + ID;
            db.executeQuery(query);
            db.myConnection.Close();

            Console.WriteLine("\nValmentaja päivitetty onnistuneesti! Paina ENTER jatkaaksesi.");
            Console.ReadLine();
            Console.Clear();

            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nValmentajien haku tai päivitys epäonnistui. Virhe: " + ex.Message);
                return false;
            }       
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Määritetään tietokanta olio. Konstruktori yhdistää ja avaa tietokannan. Mitään tietoa ei vielä haeta.
                dbConnection db = new dbConnection();
                db.GetInfo();

                Console.WriteLine("Yhteys tietokantaan avattu onnistuneesti!\n\nPaina ENTER jatkaaksesi systeemiin.");
                Console.ReadLine();
                Console.Clear();
                db.myConnection.Close();

            }
            //random mumbojumboa mikä tulostaa virheen jos yhteys tietokantaan ei onnistu :D virallisen oloinen teksti!
            catch (Exception ex)
            {
                Console.WriteLine("Yhteyden avaaminen epäonnistui. Ota yhteyttä IT-tukeen pikimiten!");
                Console.WriteLine($"Virhe: {ex.Message}");
            }

            bool running = true;

            while (running)
            {
                PrintManual('0');
                char input = 'x';
                try
                {
                    input = Convert.ToChar(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    PrintManual('5');

                }
                selectFunction(input);

            }

        }

        //Aliohjelma joka tulostaa ohjelman käyttöohjeet. Parametri määrittää tulostettavan ohjeen.
        static void PrintManual(char x)
        {
            switch(x)
            {
                case '0':
                    Console.WriteLine("1: Lisää asiakas");
                    Console.WriteLine("2: Määritä uusi valmentaja");
                    Console.WriteLine("3: Lisää ryhmäliikuntatunti");
                    Console.WriteLine("4: Lisää asiakas ryhmäliikuntatunnille");
                    Console.WriteLine("5: Näytä valmentaja ja asiakastiedot\n");
                    break;
                case '1':
                    Console.Clear();
                    break;
                case '5':
                    Console.Clear();
                    Console.WriteLine("Virheellinen syöte. Syötä numero 1-5.");
                    break;
            }    
        }

        static void selectFunction(char x)
        {
            dbConnection db;
            PersonalTrainer tempPT;
            switch (x)
            {
                case '1': //Asiakkaan lisäys kyselyt, itse lisäys tehdään Customer luokan AddCustomer metodissa.
                    try
                    {
                    Console.Clear();
                    Console.Write("Asiakkaan nimi muodossa ETUNIMI SUKUNIMI: ");
                    string name = Console.ReadLine();
                    Console.Write("\nSyntymäpäivä muodossa PP.KK.VVVV: ");
                    DateTime birthDay = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("\nJäsenyyden päättymispäivä muodossa PP.KK.VVVV: ");
                    DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());
                    
                    db = new dbConnection();
                    db.GetInfo();
                    tempPT = db.RequestTrainerID();
                    int personalTrainerID = tempPT.ID;
                    Console.Write("\nRyhmä ja valmentaja käynnit: ");
                    int groupVisits = Convert.ToInt16(Console.ReadLine());
                    Customer c = new Customer(name, birthDay, personalTrainerID, groupVisits, membershipEndDay); //Luodaan uusi asiakas olio.
                    c.AddCustomer(); //Kutsutaan olion AddCustomer metodia joka lisää asiakkaan tietokantaan.
                    }
                    catch
                    {
                        Console.WriteLine("Tarkista tietojen syöttömuoto. Paina ENTER jatkaaksesi.");
                        Console.ReadLine();
                        Console.Clear();
                    }                   
                    break;

                case '2': //Valmentajan määritys asiakkaalle.
                    Console.Clear();
                    Console.WriteLine("Valitse asiakas jolle haluat määrittää uuden valmentajan.}\n");
                    db = new dbConnection();
                    db.GetInfo(); //Hakee tiedot
                    
                    Customer temp = db.RequestCustomer(); //Kutsuu funktion joka tulostaa asiakkaat, pyytää käyttäjää syöttämään asiakkaan ID:n ja palauttaa sen.
                    if (temp == null)
                    {
                        break;
                    }
                    db.myConnection.Close();
                    temp.AssignNewTrainer();
                    break;

                case '3':
                    Console.Clear();
                    db = new dbConnection();
                    db.GetInfo();
                    GroupClass gc = new GroupClass();

                    tempPT = db.RequestTrainerID();
                    if (tempPT == null)
                    {
                        break;
                    }
                    gc.TrainerID = tempPT.ID;

                    Console.Write("\nSyötä ryhmäliikuntatunnin päivämäärä ja aika muodossa 21.10.2027 18:50: ");
                    gc.DateAndTime = Convert.ToDateTime(Console.ReadLine());

                    Console.Write("\nSyötä ryhmäliikuntatunnin maksimi osallistujamäärä: ");
                    gc.VisitorLimit = Convert.ToInt16(Console.ReadLine());

                    gc.AddGroupClass();
                    Console.Clear();
                    break;

                case '4':
                    
                    break;
                case '5': //Tulostaa asiakas ja valmentajatiedot.
                    Console.Clear();
                    db = new dbConnection();
                    db.GetInfo();
                    db.PrintCustomersAndTrainers();
                    db.myConnection.Close();

                    Console.WriteLine("\nPaina ENTER jatkaaksesi.");
                    Console.ReadLine();
                    Console.Clear();

                    break;
            }
        }
    }
}
