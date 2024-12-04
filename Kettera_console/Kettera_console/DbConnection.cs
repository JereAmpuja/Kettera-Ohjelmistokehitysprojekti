using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
    public class DbConnection //Luokka jolla yhdistetään DATABASEEN.
    {
        private OleDbConnection myConnection;

        public DbConnection() //määrittää tietokannan sijainnin ja avaa yhteyden! :D jee
        {
            string appLocation = AppDomain.CurrentDomain.BaseDirectory; //Hakee ohjelman sijainnin.
            string dbPath = Path.GetFullPath(Path.Combine(appLocation, @"..\..\..", @"Kettera_console\Data\Kettera.accdb"));

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

            dbPath = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = " + dbPath;

            myConnection = new OleDbConnection(); //luo OleDbConnection tyyppisen olion.
            myConnection.ConnectionString = dbPath; //Asettaa connectionstringin tietokannan sijainniksi.         
            myConnection.Open(); //Avaa yhteyden tietokantaan.
        }

        //TÄSTÄ ETEENPÄIN METODIT JOILLA VOIDAAN TEHDÄ KOMENTOJA TIETOKANTAAN.
        private OleDbDataReader GetData(string[] fields, string table)//Parametreina kentät joita halutaan hakea ja taulu josta ne haetaan.
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = "SELECT "; //määritetään SQL kysely teksti.

            for (int i = 0; i < fields.Length; i++) //Lisätään kentät kyselyyn.
            {
                myCommand.CommandText += fields[i];
                if (i < fields.Length - 1)
                {
                    myCommand.CommandText += ", ";
                }
            }
            myCommand.CommandText += " FROM " + table; //Lisätään taulu kyselyyn.

            myCommand.CommandType = CommandType.Text; //Määritetään kyselyn tyyppi (teksti).

            //Console.WriteLine(myCommand.CommandText); //Tulostaa kyselyn konsoliin.
            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader(); //Suorittaa kyselyn ja palauttaa lukijan.

            return myReader;
        }
        private OleDbDataReader GetDataWhere(string[] fields, string table, string keyField, string keyValue)//Sama homma, mutta lisätään where tekijät parametreina.
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = "SELECT ";

            for (int i = 0; i < fields.Length; i++)
            {
                myCommand.CommandText += fields[i];
                if (i < fields.Length - 1)
                {
                    myCommand.CommandText += ", ";
                }
            }
            myCommand.CommandText += " FROM " + table;

            myCommand.CommandText += " WHERE " + keyField + " = " + keyValue + ";"; //Lisätään where ehto sekä parametrina tulleet arvot kyselyyn!

            myCommand.CommandType = CommandType.Text;

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();
            return myReader;
        }
        private OleDbDataReader GetDataWhereBetween(string[] fields, string table, string keyField, string minValue, string maxValue, int x)//Haetaan tiettyä tietoa, missä arvo on joltain väliltä (esim. päivämäärät).
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = "SELECT ";

            for (int i = 0; i < fields.Length; i++)
            {
                myCommand.CommandText += fields[i];
                if (i < fields.Length - 1)
                {
                    myCommand.CommandText += ", ";
                }
            }
            myCommand.CommandText += " FROM " + table;

            myCommand.CommandText += " WHERE " + keyField + " BETWEEN #" + minValue + "# AND #" + maxValue + "#"; //Lisätään where ehto sekä parametrina tulleet arvot kyselyyn.
            
            if (x == 0)
            {
                myCommand.CommandText += ";";
            }

            else if (x == 1)
            {
                myCommand.CommandText += " ORDER BY dateandtime ASC;";
            }
            myCommand.CommandType = CommandType.Text;

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();
            return myReader;
        }
        private string NonQueryUpdate(string table, string[] fields, string[] values, string keyField, string keyValue)//Päivittää tietoa tietokantaan.
        {
            string query = "UPDATE ";
            query += table + " SET ";
            for (int i = 0; i < fields.Length; i++)
            {
                query += fields[i] + " = " + "'" + values[i] + "'";
                if (i < fields.Length - 1)
                {
                    query += ", ";
                }
            }
            if (keyField != "" & keyValue != "")
            {
                query += " WHERE " + keyField + " = " + keyValue ;
            }

            return query;
        }
        private string NonQueryInsertInto(string table, string[] fields, string[] values)//Lisää tietoa tietokantaan.
        {
            string query = "INSERT INTO ";
            query += table + " (";
            for (int i = 0; i < fields.Length; i++)
            {
                query += fields[i];
                if (i < fields.Length - 1)
                {
                    query += ", ";
                }
            }
            query += ") VALUES (";
            for (int i = 0; i < values.Length; i++)
            {
                query += "'";
                query += values[i];
                query += "'";
                if (i < fields.Length - 1)
                {
                    query += ",";
                }
            }
            return query + ");";
        } 
        public string NonQueryDelete(string table, string keyField, string keyValue)//Rakentaa delete komennon parametreina saaduilla arvoilla.
        {
            string query = "DELETE FROM " + table + " WHERE " + keyField + " = " + keyValue;
            return query;
        }
        public void ExecuteDelete(string table, string keyField, string keyValue)//Suorittaa delete komennon tietokantaan. Kutsuu yllä olevaa metodia.
        {
            string query = NonQueryDelete(table, keyField, keyValue);
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = query;

            myCommand.CommandType = CommandType.Text;

            myCommand.ExecuteNonQuery();
        }
        public void ExecuteInsertInto(string table, string[] fields, string[] values)//Suorittaa insert into komennon tietokantaan. Kutsuu yllä olevaa metodia "NonQueryInsertInto".
        {
            string query = NonQueryInsertInto(table, fields, values);
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = query;

            myCommand.CommandType = CommandType.Text;

            myCommand.ExecuteNonQuery();
        }
        public void ExecuteUpdate(string table, string[] fields, string[] values, string keyField, string keyValue)//Suorittaa update komennon tietokantaan. Kutsuu yllä olevaa metodia nimeltä "NonQueryUpdate".
        {
            string query = NonQueryUpdate(table, fields, values, keyField, keyValue);
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = query;

            myCommand.CommandType = CommandType.Text;

            myCommand.ExecuteNonQuery();
        }
        public Customer GetCustomerByID(int custID)//Hakee asiakkaan ID:n perusteella.
        {
            Customer newC = null;
            string[] fields = { "c.*, t.trainer_name" };
            string table = "customer c LEFT JOIN trainer t ON t.trainer_id = c.trainer_ref";

            OleDbDataReader myReader;
            myReader = GetDataWhere(fields, table, "customer_id", custID.ToString());

            //Tarkistaa onko tietoja luettavana.    
            bool notEoF;

            notEoF = myReader.Read();
            //lukee tietoja niin kauan kun niitä on.
            while (notEoF)
            {
                int ID = Convert.ToInt16(myReader["customer_id"]);
                string Name = myReader["customer_name"].ToString();
                DateTime BirthDay = Convert.ToDateTime(myReader["birthday"]);
                int PersonalTrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                string PersonalTrainerName = myReader["trainer_name"].ToString();
                int GymVisits = Convert.ToInt16(myReader["gym_visits"]);
                int GroupVisits = Convert.ToInt16(myReader["group_pt_visits"]);
                DateTime MembershipEndDay = Convert.ToDateTime(myReader["membership_end"]);

                newC = new Customer(ID, Name, BirthDay, MembershipEndDay, PersonalTrainerID, PersonalTrainerName, GymVisits, GroupVisits);
                break;
            }
            return newC;
        }
        public Customer GetCustomerByName(string custName)//Hakee asiakkaan tiedot nimen perusteella.
        {
            Customer newC = null;
            string[] fields = { "c.*, t.trainer_name" };
            string table = "customer c LEFT JOIN trainer ON t.trainer_id = c.trainer_ref";

            OleDbDataReader myReader;
            myReader = GetDataWhere(fields, table, "customer_name", custName);

            //Tarkistaa onko tietoja luettavana.
            bool notEoF;

            notEoF = myReader.Read();
            //lukee tietoja niin kauan kun niitä on.
            while (notEoF)
            {
                int ID = Convert.ToInt16(myReader["customer_id"]);
                string Name = myReader["customer_name"].ToString();
                DateTime BirthDay = Convert.ToDateTime(myReader["birthday"]);
                int PersonalTrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                string PersonalTrainerName = myReader["trainer_name"].ToString();
                int GymVisits = Convert.ToInt16(myReader["gym_visits"]);
                int GroupVisits = Convert.ToInt16(myReader["group_pt_visits"]);
                DateTime MembershipEndDay = Convert.ToDateTime(myReader["membership_end"]);

                newC = new Customer(ID, Name, BirthDay, MembershipEndDay, PersonalTrainerID, PersonalTrainerName, GymVisits, GroupVisits);
                break;
            }
            return newC;
        }
        public List<Customer> GetAllCustomers()//Hakee kaikki asiakkaat.
        {
            List<Customer> customers = new List<Customer>();
            string[] fields = { "c.*, t.trainer_name" };
            string table = "customer c LEFT JOIN trainer t ON t.trainer_id = c.trainer_ref ORDER BY c.customer_id"; //Lisätään order by jotta asiakkaat tulostuvat oikeassa järjestyksessä.

            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                Customer newC;
                int ID = Convert.ToInt16(myReader["customer_id"]);
                string Name = myReader["customer_name"].ToString();
                DateTime BirthDay = Convert.ToDateTime(myReader["birthday"]);
                int PersonalTrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                string PersonalTrainerName = myReader["trainer_name"].ToString();
                int GymVisits = Convert.ToInt16(myReader["gym_visits"]);
                int GroupVisits = Convert.ToInt16(myReader["group_pt_visits"]);
                DateTime MembershipEndDay = Convert.ToDateTime(myReader["membership_end"]);

                newC = new Customer(ID, Name, BirthDay, MembershipEndDay, PersonalTrainerID, PersonalTrainerName, GymVisits, GroupVisits);
                customers.Add(newC);
                notEoF = myReader.Read();
            }
            return customers;
        }
        public List<Trainer> GetAllTrainers() //Hakee kaikki valmentajat.
        {
            List<Trainer> trainers = new List<Trainer>();
            string[] fields = { "*" };
            string table = "trainer";

            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                Trainer newT;
                int ID = Convert.ToInt16(myReader["trainer_id"]);
                string Name = myReader["trainer_name"].ToString();

                newT = new Trainer(ID, Name);
                trainers.Add(newT);
                notEoF = myReader.Read();
            }
            return trainers;
        }
        public List<GroupClass> GetGroupClasses()//Hakee kaikki ryhmäliikuntatunni.
        {
            List<GroupClass> groupClasses = new List<GroupClass>();
            string[] fields = { "g.*, t.trainer_name" };
            string table = "group_class g LEFT JOIN trainer t ON t.trainer_id = g.trainer_ref ORDER BY dateandtime ASC;";

            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                GroupClass newGC;
                int ID = Convert.ToInt16(myReader["class_id"]);
                int TrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                string TrainerName = myReader["trainer_name"].ToString();
                DateTime DateAndTime = Convert.ToDateTime(myReader["dateandtime"]);
                int VisitorLimit = Convert.ToInt16(myReader["visitor_limit"]);
                int VisitorCount = Convert.ToInt16(myReader["visitor_count"]);

                newGC = new GroupClass(ID, TrainerID, TrainerName, DateAndTime, VisitorLimit, VisitorCount);
                groupClasses.Add(newGC);
                notEoF = myReader.Read();
            }
            return groupClasses;
        }
        public List<CalendarEvent> GetAllCalendarEvents()//Hakee kaikki kalenteritapahtumat.
        {
            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();
            string[] fields = { "gcr.*, gc.dateandtime, cm.customer_name, gc.trainer_ref" };
            string table = "(group_class_reservation AS gcr";
            table += " LEFT JOIN customer AS cm ON cm.customer_id = gcr.customer_ref) " +
                     "LEFT JOIN group_class AS gc ON gc.class_id = gcr.class_ref " +
                     "ORDER BY dateandtime ASC";
            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                CalendarEvent newCE;
                int ID = Convert.ToInt16(myReader["reservation_id"]);
                DateTime Date = Convert.ToDateTime(myReader["dateandtime"]);
                int CustomerID = Convert.ToInt16(myReader["customer_ref"]);
                string CustomerName = myReader["customer_name"].ToString();
                int TrainerID = Convert.ToInt16(myReader["trainer_ref"]);
                int ClassID = Convert.ToInt16(myReader["class_ref"]);

                newCE = new CalendarEvent(ID, Date, CustomerID, CustomerName, ClassID, TrainerID);
                calendarEvents.Add(newCE);
                notEoF = myReader.Read();
            }
            return calendarEvents;
        }
        public List <CalendarEvent> GetAllGymVisits()//Hakee kaikki kuntosalikäynnit.
        { 
            List <CalendarEvent> gymVisits = new List<CalendarEvent>();
            string[] fields = { "gv.customer_ref, gv.dateandtime, cm.customer_name" };
            string table = "gym_visit gv LEFT JOIN customer cm ON cm.customer_id = gv.customer_ref ORDER BY dateandtime ASC";
            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;
            notEoF = myReader.Read();
            while (notEoF) 
            {
                CalendarEvent newGV;
                int CustomerID = Convert.ToInt16(myReader["customer_ref"]);
                DateTime Date = Convert.ToDateTime(myReader["dateandtime"]);
                string CustomerName = myReader["customer_name"].ToString();
                newGV = new CalendarEvent(CustomerID, Date, CustomerName);
                gymVisits.Add(newGV);
                notEoF = myReader.Read();
            }
            return gymVisits;
        }
        public List<CalendarEvent> GetGymVisitsByCustID(int custID)//Hakee asiakkaan kuntosalikäynnit ID:n perusteella.
        {
            List<CalendarEvent> gymVisits = new List<CalendarEvent>();
            string[] fields = { "gv.customer_ref, gv.dateandtime, cm.customer_name" };
            string table = "gym_visit gv LEFT JOIN customer cm ON cm.customer_id = gv.customer_ref";
            OleDbDataReader myReader;
            myReader = GetDataWhere(fields, table, "customer_ref", custID.ToString());

            bool notEoF;
            notEoF = myReader.Read();
            while (notEoF)
            {
                CalendarEvent newGV;
                int CustomerID = Convert.ToInt16(myReader["customer_ref"]);
                DateTime Date = Convert.ToDateTime(myReader["dateandtime"]);
                string CustomerName = myReader["customer_name"].ToString();
                newGV = new CalendarEvent(CustomerID, Date, CustomerName);
                gymVisits.Add(newGV);
                notEoF = myReader.Read();
            }
            return gymVisits;
        }
        public List<CalendarEvent> GetGymVisitsFromTime(DateTime time1, DateTime time2)//Hakee kuntosalikäynnit aikavälin perusteella.
        {
            List<CalendarEvent> gymVisits = new List<CalendarEvent>();
            string[] fields = { "gv.customer_ref, gv.dateandtime, cm.customer_name" };
            string table = "gym_visit gv LEFT JOIN customer cm ON cm.customer_id = gv.customer_ref";
            OleDbDataReader myReader;
            myReader = GetDataWhereBetween(fields, table, "gv.dateandtime", time1.ToString("yyyy-MM-dd"), time2.ToString("yyyy-MM-dd"), 1);   

            bool notEoF;
            notEoF = myReader.Read();
            while (notEoF)
            {
                CalendarEvent newGV;
                int CustomerID = Convert.ToInt16(myReader["customer_ref"]);
                DateTime Date = Convert.ToDateTime(myReader["dateandtime"]);
                string CustomerName = myReader["customer_name"].ToString();
                newGV = new CalendarEvent(CustomerID, Date, CustomerName);
                gymVisits.Add(newGV);
                notEoF = myReader.Read();
            }
            return gymVisits;
        }
        public List<CalendarEvent> GetPtReservations()//Hakee kaikki PT varaukset.
        {
            List<CalendarEvent> ptVisits = new List<CalendarEvent>();
            string[] fields = { "pt.*, t.trainer_name, cm.customer_name" };
            string table = "(pt_reservation AS pt LEFT JOIN trainer AS t ON t.trainer_id = pt.trainer_ref) LEFT JOIN customer cm ON cm.customer_id = pt.customer_ref;";
            OleDbDataReader myReader;
            myReader = GetData(fields, table);

            bool notEoF;    
            notEoF = myReader.Read();
            while (notEoF)
            {
                CalendarEvent newGV = new CalendarEvent();
                newGV.ID = Convert.ToInt16(myReader["reservation_id"]);
                newGV.trainerID = Convert.ToInt16(myReader["trainer_ref"]);
                newGV.trainerName = myReader["trainer_name"].ToString();
                newGV.Date = Convert.ToDateTime(myReader["dateandtime"]);
                newGV.customerID = Convert.ToInt16(myReader["customer_ref"]);
                newGV.customerName = myReader["customer_name"].ToString();
                ptVisits.Add(newGV);
                notEoF = myReader.Read();
            }
            return ptVisits;
        }
    }
}
