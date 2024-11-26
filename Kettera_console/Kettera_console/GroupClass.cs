using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
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

        public GroupClass(int newID, int newTrainerID, string newTrainerName, DateTime newDateAndTime, int newVisitorLimit, int newVisitorCount)
        {
            ID = newID;
            TrainerID = newTrainerID;
            TrainerName = newTrainerName;
            DateAndTime = newDateAndTime;
            VisitorLimit = newVisitorLimit;
            VisitorCount = newVisitorCount;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Trainer ID & Name: {TrainerID}, {TrainerName}, Date & Time: {DateAndTime.ToString("dd-MM-yyyy HH:mm")}, Visitor Limit: {VisitorLimit}, Visitor Count: {VisitorCount}";
        }
    }
}
