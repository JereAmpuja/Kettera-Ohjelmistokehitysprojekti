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
        public List<PersonalTrainer> personalTrainers = new List<PersonalTrainer();

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
            // myConnection.Open(); //Avaa yhteyden. KOMMENTOITU POIS KOSKA TOISTAISEKSI TOTEUTUS MUUALLA.


        
            //luodaan komento.
            myCommand = new OleDbCommand(); //luo OleDbCommand tyyppisen olion. Tällä tehdään muokkaukset tietokantaan.
            myCommand.Connection = myConnection; //yhdistää komennon yhteyteen! :D

            myReader = myCommand.ExecuteReader();

            // Komentoa muutetaan tällä tavalla:
            // myCommand.CommandText = "INSERT INTO Taulu (Kenttä1, Kenttä2) VALUES ('Arvo1', 'Arvo2')";
            // myCommand.CommandType = CommandType.Text; //Tämä kertoo että kyseessä on tekstikomento.
        }

        //TÄSTÄ ETEENPÄIN METODIT JOILLA VOIDAAN TEHDÄ KOMENTOJA TIETOKANTAAN.

        public bool GetInfo()
        {
            //Ensin haetaan PT tiedot.
            myConnection.Open();
            bool notEoF = myReader.Read();
            myCommand.CommandText = "SELECT * FROM Valmentaja";
            myCommand.CommandType = CommandType.Text;

            while (notEoF)
            {
                PersonalTrainer pt = new PersonalTrainer();
                pt.ID = myReader.GetInt32(0);
                pt.Name = myReader.GetString(1);
                personalTrainers.Add(pt);
            }


            //Seuraavaksi haetaan asiakkaat.
            myCommand.CommandText = "SELECT * FROM Asiakas";
            myCommand.CommandType = CommandType.Text;
            //Ja tehdään tiedoista customer olioita.

            while (notEoF)
            {                 
                Customer c = new Customer();
                c.ID = myReader.GetInt32(0);
                c.Name = myReader.GetString(1);
                c.BirthDay = myReader.GetDateTime(2);             
                c.PersonalTrainerName = myReader.GetString(3);
                c.GymVisits = myReader.GetInt32(4);
                c.GroupVisits = myReader.GetInt32(5);
                c.MembershipEndDay = myReader.GetDateTime(6);
                customers.Add(c);
            }

        }
        public bool AddCustomer(string name, DateTime bDay, string ptName, DateTime membershipEndDay)
        { }

        

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
        public string PersonalTrainerName { get; set; }
        public int GymVisits { get; set; }
        public int GroupVisits { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            dbConnection db;

            try
            {
               db = new dbConnection();
                Console.WriteLine("Yhteys tietokantaan avattu onnistuneesti!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Yhteyden avaaminen epäonnistui.");
                Console.WriteLine($"Virhe: {ex.Message}");
            }


        }
    }
}
