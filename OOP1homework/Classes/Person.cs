using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1homework.Classes
{
    internal class Person
    {
        public string Name { get; }
        public string Surname { get; }

        public string Email { get; }
        private Dictionary<Guid, bool> Attendance { get; set; }
        public Person(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Attendance = new Dictionary<Guid, bool>();
            
        }

        public void AddAttendance(Guid eventId)
        {
            Attendance[eventId] = true;
        }

    }
}
