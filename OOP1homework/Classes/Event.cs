using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1homework.Classes
{
    internal class Event
    {
        public Guid Id { get; } //Generira se automatski isključivo pri instanciranju objekta i nije ga moguće mijenjati kasnije na nikoji način

        public string Name { get; set; } 
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private List<string> participantEmails { get; set; }
        public Event (string name, string location, DateTime start, DateTime end, List<string> emails)
            {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            StartDate = start;
            EndDate = end;
            participantEmails = new List<string>(emails);
            }


        public bool AddParticipant(string email)
        {
            if (participantEmails.Contains(email)) return false;
            participantEmails.Add(email);
            return true;
        }

        public List<string> GetParticipantEmails()
        {
            return new List<string>(participantEmails);
        }





    }
}
