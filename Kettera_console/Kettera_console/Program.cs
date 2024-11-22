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
            myCommand.CommandText = "SELECT * FROM customer ORDER BY customer_id";
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
                for (int i = 0; i < personalTrainers.Count; i++)
                {
                    if (temp == personalTrainers[i].ID)
                    {
                        c.PersonalTrainerName = personalTrainers[i].Name;
                    }
                }

                c.GymVisits = Convert.ToInt16(myReader["gym_visits"]);
                c.GroupVisits = Convert.ToInt16(myReader["group_pt_visits"]);
                c.MembershipEndDay = c.BirthDay = Convert.ToDateTime(myReader["membership_end"]);
                customers.Add(c);
                notEoF = myReader.Read();
            }
            myReader.Close();


            return true;

        }
    }

    public class Lists
    {
        public List<Customer> customers = new List<Customer>();
        public List<PersonalTrainer> personalTrainers = new List<PersonalTrainer>();
    }

    public class PersonalTrainer
    {
        public int ID { get; set; }
        public string Name { get; set; }
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

        public bool AddCustomer()
        { 
            try
            {
                dbConnection db = new dbConnection();
                db.myCommand.CommandText = "INSERT INTO customer(customer_name, birthday, trainer_ref, gym_visits, group_pt_visits, membership_end) " +
                "VALUES ('" + Name + "','" + BirthDay + "','" + PersonalTrainerID + 
                "','" + GymVisits + "','" + GroupVisits + "','" + MembershipEndDay + "')";
                db.myCommand.ExecuteNonQuery();
                db.myConnection.Close();

                Console.WriteLine("Asiakas lisätty tietokantaan onnistuneesti! Paina ENTER jatkaaksesi");
                Console.ReadLine();
                Console.Clear();
                return true;
            }
            
            catch (Exception ex) 
            { 
                Console.WriteLine("Asiakkaan lisääminen tietokantaan epäonnistui. Virhe: " + ex.Message);
                return false;
            }            
        }

        public bool AssignTrainer()
        {
            return true;
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

                try
                {
                    selectFunction(Convert.ToChar(Console.ReadLine()));
                }
                catch (Exception ex)
                {
                    PrintManual('5');

                }
                
                
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
            switch (x)
            {
                case '1': //Asiakkaan lisäys kyselyt, itse lisäys tehdään Customer luokan AddCustomer metodissa.
                    try
                    {
                    Console.Clear();
                    Console.Write("Asiakkaan nimi muodossa ETUNIMI SUKUNIMI: ");
                    string name = Console.ReadLine();
                    Console.Write("Syntymäpäivä muodossa PP.KK.VVVV: ");
                    DateTime birthDay = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Jäsenyyden päättymispäivä muodossa PP.KK.VVVV: ");
                    DateTime membershipEndDay = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Määritä valmentaja. Saatavilla olevat valmentajat:");
                        try
                        {
                            dbConnection db = new dbConnection();
                            db.GetInfo();
                            for (int i = 0; i < db.personalTrainers.Count; i++)
                            {
                                Console.WriteLine($"{db.personalTrainers[i].ID}: {db.personalTrainers[i].Name}");
                            }
                            db.myConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Valmentajien haku epäonnistui. Virhe: " + ex.Message);
                        }
                    Console.Write("Syötä valmentajan numero: ");
                    int personalTrainerID = Convert.ToInt16(Console.ReadLine());
                    Console.Write("Ryhmä ja valmentaja käynnit: ");
                    int groupVisits = Convert.ToInt16(Console.ReadLine());
                    Customer c = new Customer(name, birthDay, personalTrainerID, groupVisits, membershipEndDay); //Luodaan uusi asiakas olio.
                    c.AddCustomer(); //Kutsutaan olion AddCustomer metodia joka lisää asiakkaan tietokantaan.
                    }
                    catch
                    {
                        Console.WriteLine("Tarkista tietojen syöttömuoto.");
                    }                   
                    break;
                case '2':
                    
                    break;
                case '3':
                    
                    break;
                case '4':
                    
                    break;
                case '5': //Tulostaa asiakas ja valmentajatiedot.
                    Console.Clear();
                    dbConnection dbc = new dbConnection();
                    dbc.GetInfo();
                    Console.WriteLine("Valmentajat:");
                    for (int i = 0; i < dbc.personalTrainers.Count; i++)
                    {
                        Console.WriteLine($"{dbc.personalTrainers[i].ID}: {dbc.personalTrainers[i].Name}");
                    }
                    Console.WriteLine("\nAsiakkaat:");
                    for (int i = 0; i < dbc.customers.Count; i++)
                    {
                        Console.WriteLine(dbc.customers[i].ToString());
                    }
                    dbc.myConnection.Close();
                    Console.WriteLine("\nPaina ENTER jatkaaksesi.");
                    Console.ReadLine();
                    Console.Clear();
                    break;
            }
        }
    }
}
