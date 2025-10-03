using OOP1homework.Classes;
using System.ComponentModel.DataAnnotations;
using OOP1homework.Enums;
using System.ComponentModel;

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
Event AddEventWithAttendance(string name, string location, DateTime start, DateTime end, List<string> emails)
{
    var newEvent = new Event(name, location, start, end, emails);
    events.Add(newEvent);

    foreach(var email in emails) //u pocetku svima stavim da su bili prisutni iz liste emails
    {
        var person = people.Find(p => p.Email == email);
        if (person != null)   //moze vratiti null ako nema u listi osobe s tim emailom, mozda kasnije stvoriti za null novi person s tim emailom
        {
            bool zauzet = false;
            foreach (var e in events) //za sve evente
            {
                if (person.hasAttended(e.Id)) //gledaj je li person prisutna na njima, ako je
                {
                    if (start < e.EndDate && end > e.StartDate)
                    {
                        Console.WriteLine($"Osoba {person.Email} je zauzeta u terminu {e.Name} ({e.StartDate} - {e.EndDate}) i ne može biti dodana na {newEvent.Name}.");
                        zauzet = true;
                        break; //ne tribas ic dalje gledat je za sljedece evente
                    }
                }
            }
            if (!zauzet)
            {
                person.AddAttendance(newEvent.Id);
                Console.WriteLine($"Osoba {person.Email} uspješno dodana na {newEvent.Name}.");
            }

        }
        else
        {
            Console.WriteLine($"Osoba s emailom {email} ne postoji u sustavu!");
        }



    }
    if (newEvent == null) return null;

    return newEvent;
}

AddEventWithAttendance("Radionica programiranja", "Split", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2).AddHours(3), emailsEvent2);
AddEventWithAttendance("Sportski dan", "Rijeka", DateTime.Now.AddDays(12), DateTime.Now.AddDays(12).AddHours(4), emailsEvent3);
AddEventWithAttendance("Božićni party", "Zagreb", DateTime.Now.AddDays(20), DateTime.Now.AddDays(20).AddHours(5), emailsEvent1);
AddEventWithAttendance("Predavanje o AI", "Osijek", DateTime.Now.AddDays(15), DateTime.Now.AddDays(15).AddHours(2), emailsEvent2);
AddEventWithAttendance("Teambuilding", "Zagreb", DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-5).AddHours(6), emailsEvent1);
//aktivni eventi, tumacit cemo one koji se trenutno odrzavaju npr prvi

while (true)
{
    Console.WriteLine("1 Aktivni eventi");
    Console.WriteLine("2 Nadolazeci eventi");
    Console.WriteLine("3 Eventi koji su zavrsili");
    Console.WriteLine("4 Kreiraj event");
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
        case "4": 
            CreateEvent(events);
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
            Console.WriteLine($" - {email}");  //ode se ispisuju samo emailovi, a ne gleda se je li osoba prisutna ili nije ali to se ni ne trazi 
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
    if (!int.TryParse(choice, out int index) || index < 1 || index > upcomingEvent.Count)
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
        person.removeAttendance(selectedEvent.Id);
    /*    selectedEvent.RemoveParticipant(person.Email);*/ //doda novu metodu remove participant da maknem s liste emailova tog eventa taj mail
        //ovo gore zakomentirano jer zelim da lista emails ostane uvijek fiksna, to su pozvani ljudi a samo cu im staviti false onima koji nisu prisutni
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
    printPresentAndAbsent(finishedEvents);
   
}

void printPresentAndAbsent(List<Event> events_)
{
 foreach(var e in events_)
    {
        foreach (var email_ in e.GetParticipantEmails())
        {
            var person = people.Find(p => p.Email == email_);
            if (person != null && person.hasAttended(e.Id))
                Console.WriteLine($"Prisutan - {email_}");
            else if (person != null)
                Console.WriteLine($"Nije prisutan - {email_}");
            else
                Console.WriteLine($"Osoba s tim emailom {email_} ne postoji u sustavu"); //samo za pastevents se ispisuju ne prisutni
        }
    }
}



void CreateEvent(List<Event> events)
{
    var newEvent = inputEvent();

    if(newEvent != null)
    {
        AddEventWithAttendance(newEvent.Name, newEvent.Location, newEvent.StartDate, newEvent.EndDate, newEvent.GetParticipantEmails());
        Console.WriteLine("Event uspješno kreiran!"); 
    }


}

Event? inputEvent()
{
    Console.WriteLine("Unesite naziv eventa");
    string name = Console.ReadLine().Trim();

    Console.WriteLine("Unesi lokaciju: ");
    string location = Console.ReadLine().Trim();

    Console.WriteLine("Unesi datum početka (yyyy-MM-dd HH:mm): ");
    //kao sto mozes parsati u int mozes i u dateTime
    if(!DateTime.TryParse(Console.ReadLine(), out  DateTime startDate))
    {
        Console.WriteLine("Nevažeći format datuma početka.");
        return null;
    }

    Console.Write("Unesi datum završetka (yyyy-MM-dd HH:mm): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
    {
        Console.WriteLine("Nevažeći format datuma završetka.");
        return null;
    }

    //validacije za upcoming te start>end logicno dodaj gore
    if (startDate >= endDate)
    {
        Console.WriteLine("Kraj eventa ne smije biti prije pocetka");
        return null;
    }
    if (startDate <= DateTime.Now)
    {
        Console.WriteLine("Mora zapoceti u buducnosti");
        return null;
    }



    Console.WriteLine("Unesi emailove sudionika ovog eventa");
    var emails = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(e=>e.Trim().ToLower()).ToList();
    return AddEventWithAttendance(name, location, startDate, endDate, emails); 

}