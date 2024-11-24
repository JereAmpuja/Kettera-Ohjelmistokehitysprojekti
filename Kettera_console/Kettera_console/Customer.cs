using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kettera_console
{
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

        //Konstruktori jolla voidaan luoda asiakas perusarvoilla. Varmuuden vuokis tässä! :D
        public Customer()
        {
            ID = 0;
            Name = "";
            BirthDay = DateTime.Now;
            MembershipEndDay = DateTime.Now;
            PersonalTrainerID = 0;
            PersonalTrainerName = "";
            GymVisits = 0;
            GroupVisits = 0;

        }

        //Konstruktori jolla voidaan luoda asiakas olio hakiessa tietoja tietokannasta.
        public Customer(int newID, string newName, DateTime newBirthDay, DateTime newMembershipEndDay, int newPersonalTrainerID, string newPersonalTrainerName, int gymVisits, int groupVisits) : this()
        {
            ID = newID;
            Name = newName;
            BirthDay = newBirthDay;
            MembershipEndDay = newMembershipEndDay;
            PersonalTrainerID = newPersonalTrainerID;
            PersonalTrainerName = newPersonalTrainerName;
            GymVisits = gymVisits;
            GroupVisits = groupVisits;
        }
        //Konstruktori jolla voidaan luoda uusi asiakas olio lisätessä asiakas tietokantaan.
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

            return $"{ID,-4}{Name,-20}{BirthDay.ToString("dd-MM-yyyy"),-20}{MembershipEndDay.ToString("dd-MM-yyyy"),-30}{PersonalTrainerName,-20}{GymVisits,-20}{GroupVisits,-20}";
        }
    }
}
