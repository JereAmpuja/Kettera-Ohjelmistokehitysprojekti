using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Kettera_console
{
    public class dbConnection //Luokka jolla yhdistetään DATABASEEN.
    {
        string connectionString = @"Kettera_console/Data/Kettera.accdb"; //Tietokannan sijainti (ainakin pitäisi olla XD).
        public dbConnection()
        {
            OleDbConnection myConnection = new OleDbConnection(); //luo OleDbConnection tyyppisen olion.
            myConnection.ConnectionString = connectionString; //Asettaa connectionstringin tietokannan sijainniksi.
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
        }
    }
}
