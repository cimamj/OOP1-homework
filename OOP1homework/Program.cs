using OOP1homework.Classes;

//lista evenata i ljudi
var events = new List<Event>();
var people = new List<Person>();

//mailovi koji sudjeluju na eventima
var emailsEvent1 = new List<string>()
{
    "jure.mamic@gmail.com",
    "ana.kovac@yahoo.com",
    "marko.peric@outlook.com",
    "ivana.novak@hotmail.com",
    "petar.klaric@gmail.com",
    "luka.babic@yahoo.com"
};
var emailsEvent2 = new List<string>()
{
    "mario.juric@gmail.com",
    "katja.milic@yahoo.com",
    "tomislav.petrovic@outlook.com"
};

var emailsEvent3 = new List<string>()
{
    "ivica.kovac@gmail.com",
    "ana.babic@yahoo.com",
    "marin.klaric@outlook.com"
};


//stvoriti ljude
var person1 = new Person("Jure", "Mamić", "jure.mamic@gmail.com");

people.Add(person1);
people.Add(new Person("Ana", "Kovač", "ana.kovac@gmail.com"));
people.Add(new Person("Marko", "Horvat", "marko.horvat@gmail.com"));
people.Add(new Person("Petra", "Novak", "petra.novak@gmail.com"));
people.Add(new Person("Ivan", "Babić", "ivan.babic@gmail.com"));
people.Add(new Person("Lucija", "Marić", "lucija.maric@gmail.com"));
people.Add(new Person("Tomislav", "Jurić", "tomislav.juric@gmail.com"));
people.Add(new Person("Maja", "Knežević", "maja.knezevic@gmail.com"));
people.Add(new Person("Stjepan", "Pavić", "stjepan.pavic@gmail.com"));
people.Add(new Person("Katarina", "Šimić", "katarina.simic@gmail.com"));


//stvoriti evente i dodati attendance pristunost
void AddEventWithAttendance(string name, string location, DateTime start, DateTime end, List<string> emails)
{
    var newEvent = new Event(name, location, start, end, emails);
    events.Add(newEvent);

    foreach(var email in emails)
    {
        var person = people.Find(p => p.Email == email);
        if(person != null) //moze vratiti null ako nema u listi osobe s tim emailom
        person.AddAttendance(newEvent.Id);
    }
}

AddEventWithAttendance("Radionica programiranja", "Split", DateTime.Now.AddDays(10), DateTime.Now.AddDays(10).AddHours(3), emailsEvent2);
AddEventWithAttendance("Sportski dan", "Rijeka", DateTime.Now.AddDays(12), DateTime.Now.AddDays(12).AddHours(4), emailsEvent3);
AddEventWithAttendance("Božićni party", "Zagreb", DateTime.Now.AddDays(20), DateTime.Now.AddDays(20).AddHours(5), emailsEvent1);
AddEventWithAttendance("Predavanje o AI", "Osijek", DateTime.Now.AddDays(15), DateTime.Now.AddDays(15).AddHours(2), emailsEvent2);
AddEventWithAttendance("Konferencija", "Zadar", DateTime.Now.AddDays(18), DateTime.Now.AddDays(18).AddHours(8), emailsEvent3);
AddEventWithAttendance("Team lunch", "Split", DateTime.Now.AddDays(7), DateTime.Now.AddDays(7).AddHours(2), emailsEvent1);
AddEventWithAttendance("Hackathon", "Zagreb", DateTime.Now.AddDays(25), DateTime.Now.AddDays(26).AddHours(6), emailsEvent2);
AddEventWithAttendance("Seminar produktivnosti", "Rijeka", DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddHours(3), emailsEvent3);
AddEventWithAttendance("Networking event", "Zagreb", DateTime.Now.AddDays(35), DateTime.Now.AddDays(35).AddHours(4), emailsEvent1);
AddEventWithAttendance("Teambuilding", "Zagreb", DateTime.Now.AddDays(5), DateTime.Now.AddDays(5).AddHours(6), emailsEvent1);


