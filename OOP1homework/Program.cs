using OOP1homework.Classes;
using System.ComponentModel.DataAnnotations;
using OOP1homework.Enums;

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
var person1 = new Person("Jure", "Mamic", "jure.mamic@gmail.com");
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

    foreach(var email in emails) //u pocetku svima stavim da su bili prisutni iz liste emails
    {
        var person = people.Find(p => p.Email == email);
        if(person != null) //moze vratiti null ako nema u listi osobe s tim emailom, mozda kasnije stvoriti za null novi person s tim emailom
        person.AddAttendance(newEvent.Id);
    }
}

AddEventWithAttendance("Radionica programiranja", "Split", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2).AddHours(3), emailsEvent2);
AddEventWithAttendance("Sportski dan", "Rijeka", DateTime.Now.AddDays(12), DateTime.Now.AddDays(12).AddHours(4), emailsEvent3);
AddEventWithAttendance("Božićni party", "Zagreb", DateTime.Now.AddDays(20), DateTime.Now.AddDays(20).AddHours(5), emailsEvent1);
AddEventWithAttendance("Predavanje o AI", "Osijek", DateTime.Now.AddDays(15), DateTime.Now.AddDays(15).AddHours(2), emailsEvent2);
AddEventWithAttendance("Konferencija", "Zadar", DateTime.Now.AddDays(18), DateTime.Now.AddDays(18).AddHours(8), emailsEvent3);
AddEventWithAttendance("Team lunch", "Split", DateTime.Now.AddDays(7), DateTime.Now.AddDays(7).AddHours(2), emailsEvent1);
AddEventWithAttendance("Hackathon", "Zagreb", DateTime.Now.AddDays(25), DateTime.Now.AddDays(26).AddHours(6), emailsEvent2);
AddEventWithAttendance("Seminar produktivnosti", "Rijeka", DateTime.Now.AddDays(30), DateTime.Now.AddDays(30).AddHours(3), emailsEvent3);
AddEventWithAttendance("Networking event", "Zagreb", DateTime.Now.AddDays(35), DateTime.Now.AddDays(35).AddHours(4), emailsEvent1);
AddEventWithAttendance("Teambuilding", "Zagreb", DateTime.Now.AddDays(5), DateTime.Now.AddDays(5).AddHours(6), emailsEvent1);


//aktivni eventi, tumacit cemo one koji se trenutno odrzavaju npr prvi

while (true)
{
    Console.WriteLine("1 Aktivni eventi");
    Console.WriteLine("2 Nadolazeci eventi");
    Console.WriteLine("3 Eventi koji su zavrsili");
    Console.WriteLine("5 Izađi");
    Console.Write("Odaberi opciju: ");

    string choice = (Console.ReadLine());

    switch (choice)
    {
        case "1": ShowActiveEvents(events);
            break;
        case "2": ShowUpcomingEvents(events);
            break;
        case "3": ShowPastEvents(events);
            break;
        case "5": return;
        default:
            Console.WriteLine("Nevažeći unos. Pokušaj ponovno.");
            break;
    }
}

void ShowActiveEvents(List<Event> events)
{

    var now = DateTime.Now;
    var activeEvents = events.Where(e => e.StartDate <= now && e.EndDate >= now).ToList();

    //ako ih nema
    if(activeEvents.Count() == 0)
    {
        Console.WriteLine("Nema aktivnih evenata.");
        return;
    }

    printEvents(activeEvents, EventStatus.Active);

    Console.WriteLine("\nSubmenu:");
    Console.WriteLine("1. Zabiljezi neprisutnosti");
    Console.WriteLine("2. Povratak na glavni meni");
    string submenuChoice = Console.ReadLine();

    while (true)
    {

        switch (submenuChoice) 
        {
            case "1":
                recordAbsence(activeEvents);
                break;
            case "2":
                return;
            default: //
                Console.WriteLine("Nevažeći unos. Pokušaj ponovno.");
                break;
        }
    }
}

void recordAbsence(List<Event> activeEvents)
{
    if (activeEvents.Count == 0)
    {
        Console.WriteLine("Nema aktivnih evenata za bilježenje neprisutnosti.");
        return;
    }
    //event id
    Console.WriteLine("Unesi redni broj eventa za kojeg zelis promjeniti prisutnost");
    for (int i = 0; i < activeEvents.Count; i++)
    { Console.WriteLine($"{i + 1} {activeEvents[i].Name}"); }

    string choice = Console.ReadLine();
    if (!int.TryParse(choice, out var index) || index < 1 || index > activeEvents.Count) 
    {
        Console.WriteLine("Nevažeći odabir eventa.");
        return;
    }

    
    var selectedEvent = activeEvents[index-1];
    var eventId = selectedEvent.Id;
    //email 
    Console.WriteLine("Unesi emailove osoba koje nisu bile prisutne"); //ako unese dva razmaka , strinsplitoptions nece uzeti prazan string
    string[] absenceEmails = Console.ReadLine().Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    foreach (var email in absenceEmails) 
    { 
        var person = people.Find(p=>p.Email == email);

        if(!selectedEvent.GetParticipantEmails().Contains(email))
        {
            Console.WriteLine($"Osoba {email} nije sudionik ovog eventa.");
            continue;   
        }
        if(person == null)
        {
            Console.WriteLine($"Osoba s emailom {email} ne postoji u sustavu.");
            continue;
        }


        person.removeAttendance(eventId);
            Console.WriteLine($"Email {email} nije sudionik ovog eventa.");
    }

}


void ShowUpcomingEvents (List<Event> events) 
{
    var now = DateTime.Now;
    var upcomingEvents = events.Where(e=>e.StartDate > now).ToList(); //TOLIST
    if (upcomingEvents.Count == 0) { Console.WriteLine("Nema nadolazećih evenata"); return; }
    printEvents(upcomingEvents, EventStatus.Upcoming);


    Console.WriteLine("\nSubmenu:");
    Console.WriteLine("1. Izbrisi event");
    Console.WriteLine("2. Ukloni osobe s eventa");
    Console.WriteLine("3. Povratak na glavni meni");
    string submenuChoice = Console.ReadLine();

    while (true)
    {

        switch (submenuChoice)
        {
            case "1":
                deleteEvent(upcomingEvents);
                break;
            case "2":
                removePeople(upcomingEvents);
                break;
            case "3":
                return;
            default: //
                Console.WriteLine("Nevažeći unos. Pokušaj ponovno.");
                break;
        }
    }

}

void printEvents(List<Event> events, EventStatus status) //moram poslati varijablu koja odgovara za ends ili begins
{

  var now = DateTime.Now;
    foreach (var e in events)
    {
        string prefix;
        //string timeInfo = "";
        string unit = "hours";

        double timeLeft = 0;
        double hoursLeft = 0;

        ////ovo se moglo bolje sa npr corr = isActive ?  "ends in " : "begins in "; vidi ternarni operator koristit

        //string corr = "x";
        //string unit = "hours";
        if (status == EventStatus.Active)
        {
            prefix = "ends in ";
            hoursLeft = (e.EndDate - now).TotalHours;
        }
        else
        { 
            if(status == EventStatus.Upcoming)
            {
                prefix = "begins in ";
                hoursLeft = (e.StartDate - now).TotalHours;
                
            }
            else
            {   
                prefix = "ended before ";
                hoursLeft = (now - e.EndDate).TotalHours;
                
            }
                
           
        }
        if (hoursLeft >= 24)
        {
            timeLeft = Math.Round(hoursLeft / 24, 1);
            unit = " days";
        }
        else         
          timeLeft = hoursLeft;


        Console.WriteLine($"{e.Id}");
        Console.WriteLine($"{e.Name} - {e.Location} - {prefix} {timeLeft} {unit}");
        Console.WriteLine("Popis sudionika:");

        foreach (var email in e.GetParticipantEmails())
        {
            Console.WriteLine($" - {email}");
        }
    }
}


void deleteEvent(List<Event> upcomingEvent)
{
    Console.WriteLine("Unesi broj eventa kojeg zelis izbrisati");
    //provjeri ima li ih uopce
    if (upcomingEvent.Count == 0) { return; }
    for (int i = 0; i < upcomingEvent.Count; i++)
    {
        Console.WriteLine($"{i + 1} {upcomingEvent[i].Name}");
    }
    string choice = Console.ReadLine();
    if (!int.TryParse(choice, out int index) || index < 0 || index > upcomingEvent.Count)
    {
        Console.WriteLine("Pogresan unos");
        return; //da vrati na submenu
    }

    var selectedEvent = upcomingEvent[index-1];

    //izbrisat prisutnost svih osoba
    foreach (var e in selectedEvent.GetParticipantEmails()) 
    {
        var person = people.Find(p => p.Email == e);
        person?.removeAttendance(selectedEvent.Id);
    }
    //je li ovo dovoljno maknit ga s liste, treba li bas objekt izbrisati
    events.Remove(selectedEvent);
    Console.WriteLine($"Event \"{selectedEvent.Name}\" je izbrisan.");
}

void removePeople(List<Event> upcomingEvent)
{
    Console.WriteLine("Unesi broj eventa s kojeg zelis izbrisati ljude");
    //provjeri ima li ih uopce
    if (upcomingEvent.Count == 0) { return; }
    for (int i = 0; i < upcomingEvent.Count; i++)
    {
        Console.WriteLine($"{i + 1} {upcomingEvent[i].Name}");
    }
    string choice = Console.ReadLine();
    if (!int.TryParse(choice, out int index) || index < 1 || index > upcomingEvent.Count)
    {
        Console.WriteLine("Pogresan unos");
        return; //da vrati na submenu
    }

    var selectedEvent = upcomingEvent[index - 1];

    Console.WriteLine("Unesi ime i prezime osoba koje zelis ukloniti s eventa, neka osobe budu odvojene zarezom");
    var unwantedPeople = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLower()).ToList();
    foreach (var fullname in unwantedPeople)
    {
        var person = people.Find(p => (p.Name + " " + p.Surname).ToLower() == fullname);
        if (person == null) { Console.WriteLine("Osoba ne postoji u sustavu"); }
        if (!selectedEvent.GetParticipantEmails().Contains(person.Email)) { Console.WriteLine($"Osoba {person} nije sudionik ovog eventa"); }
        person?.removeAttendance(selectedEvent.Id);
        selectedEvent.RemoveParticipant(person.Email); //doda novu metodu remove participant da maknem s liste emailova tog eventa taj mail

        Console.WriteLine($"Osoba {fullname} uspješno uklonjena s eventa {selectedEvent.Name}.");
    }

    return; //Na submenu inace se vrati opet u ovu funkciju 
}



void ShowPastEvents(List<Event> events)
{
        var now = DateTime.Now;
        var finishedEvents = events.Where(e => e.EndDate < now).ToList();
    if (finishedEvents.Count == 0)
    {
        Console.WriteLine("Nema završenih evenata.");
        return;
    }

    printEvents(finishedEvents, EventStatus.Past);
}