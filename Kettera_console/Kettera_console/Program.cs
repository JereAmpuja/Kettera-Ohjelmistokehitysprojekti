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
        string appLocation = AppDomain.CurrentDomain.BaseDirectory; //Tämä hakee ohjelman sijainnin.
        string databasePath; //koko tietokannan sijainti tulee tähän rakentajassa.

        public dbConnection()
        {

            databasePath = Path.GetFullPath(Path.Combine(appLocation, @"..\..\..", @"Kettera_console\Data\Kettera.accdb"));

            // Testaa tulosteella
            Console.WriteLine("Tietokannan polku: " + databasePath);
            if (!File.Exists(databasePath))
            {
                Console.WriteLine("Tietokantatiedostoa ei löydy: " + databasePath);
                return;
            }
            else if (File.Exists(databasePath))
            {
                Console.WriteLine("Tietokantatiedosto löytyi: " + databasePath);
            }
            databasePath = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = " + databasePath;

            OleDbConnection myConnection = new OleDbConnection(); //luo OleDbConnection tyyppisen olion.
            myConnection.ConnectionString = databasePath; //Asettaa connectionstringin tietokannan sijainniksi.         
            myConnection.Open(); //Avaa yhteyden.


        
            //luodaan komento.
            OleDbCommand myCommand = new OleDbCommand(); //luo OleDbCommand tyyppisen olion. Tällä tehdään muokkaukset tietokantaan.
            myCommand.Connection = myConnection;

            // Komentoa muutetaan tällä tavalla:
            // myCommand.CommandText = "INSERT INTO Taulu (Kenttä1, Kenttä2) VALUES ('Arvo1', 'Arvo2')";
            // myCommand.CommandType = CommandType.Text; //Tämä kertoo että kyseessä on tekstikomento.
        }

        

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
