using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
    public class CalendarEvent
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int customerID { get; set; }
        public string customerName { get; set; }
        public int classID { get; set; }
        public string trainerName { get; set; }
        public int trainerID { get; set; }

        public CalendarEvent()
        {
            ID = 0;
            Date = DateTime.Now;
            customerID = 0;
            customerName = "";
            classID = 0;
            trainerID = 0;
            trainerName = "";
        }

        //Konstruktori PT-käynneille
        public CalendarEvent(int newID, DateTime newDate, int NewCustomerID, string newCustomerName, string newTrainerName, int newTrainerID) : this()
        {
            ID = newID;
            Date = newDate;
            customerID = NewCustomerID;
            customerName = newCustomerName;
            trainerName = newTrainerName;
            trainerID = newTrainerID;
        }
        public CalendarEvent(int newCustomerID, DateTime newDate, string newCustomerName) : this() //Konstruktori salikäynneille
        {
            customerID = newCustomerID;
            Date = newDate;
            customerName = newCustomerName;
        }

        public CalendarEvent(int newID, DateTime newDate, int NewCustomerID, string newCustomerName, int newClassID, int newTrainerID)
        {
            ID = newID;
            Date = newDate;
            customerID = NewCustomerID;
            customerName = newCustomerName;
            classID = newClassID;
            trainerID = newTrainerID;
        }

        public string PtVisitToString()
        {
            return $"Reservation ID: {ID}, Date: {Date.ToString("dd-MM-yyyy HH:mm")}, Customer ID & Name: {customerID}, {customerName}, Trainer ID & Name: {trainerID}, {trainerName}";
        }
        public override string ToString()
        {
            return $"Reservation ID: {ID}, Class ID: {classID}, Date: {Date.ToString("dd-MM-yyyy HH:mm")}, Customer ID & Name: {customerID}, {customerName}";
        }
    }
}
