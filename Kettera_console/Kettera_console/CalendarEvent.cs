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

        public CalendarEvent()
        {
            ID = 0;
            Date = DateTime.Now;
            customerID = 0;
            customerName = "";
            classID = 0;
        }

        public CalendarEvent(int newID, DateTime newDate, int NewCustomerID, string newCustomerName, int newClassID)
        {
            ID = newID;
            Date = newDate;
            customerID = NewCustomerID;
            customerName = newCustomerName;
            classID = newClassID;
        }

        public override string ToString()
        {
            return $"Reservation ID: {ID}, Class ID: {classID}, Date: {Date.ToString("dd-MM-yyyy HH:mm")}, Customer ID & Name: {customerID}, {customerName}";
        }
    }
}
