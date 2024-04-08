using Zuydfit;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //    string keuze;
            //    do
            //    {
            //        Console.WriteLine("Welcome to the cash register");
            //        Console.WriteLine("1. Update a Person");
            //        Console.WriteLine("2. Delete a Person");
            //        Console.WriteLine("3. Create a Person");
            //        Console.WriteLine("Pick your number: 1 / 2 / 3");
            //        keuze = Console.ReadLine();
            //        Console.Clear();

            //        if (keuze != "1" && keuze != "2" && keuze != "3")
            //        {
            //            Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
            //            Console.WriteLine("=====================================================");
            //        }
            //    }
            //    while (keuze != "1" && keuze != "2" && keuze != "3");

            //    switch (keuze)
            //    {
            //        case "1":
            //            // Update a person
            //            bool updatePerson = true;
            //            while (updatePerson)
            //            {
            //                List<Person> persons = Person.GetPersons();
            //                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            //                Console.WriteLine($"| {"PersonID",-10} | {"Firstname",-10} | {"Lastname",-10} | {"Streetname",-12} | {"Housenumber",-10} | {"Postalcode",-10} | {"LocationID",-10} | {"WorkoutId",-10}");
            //                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            //                foreach (Person person in persons)
            //                {
            //                    if (person is Athlete athlete)
            //                    {
            //                        Console.WriteLine($"| {person.Id,-10} | {person.FirstName,-10} | {person.LastName,-10} | {person.StreetName,-12} | {person.HouseNumber,-11} | {person.PostalCode,-10} | {athlete.LocationId,-10} | {athlete.WorkoutId,-10}");
            //                    }
            //                    else if (person is Coach coach)
            //                    {
            //                        Console.WriteLine($"| {person.Id,-10} | {person.FirstName,-10} | {person.LastName,-10} | {person.StreetName,-12} | {person.HouseNumber,-11} | {person.PostalCode,-10} |");
            //                    }
            //                }
            //                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            //                Console.WriteLine("==============================================");
            //                Console.WriteLine("Enter the ID of the person you want to Update:");
            //                int personid = Convert.ToInt32(Console.ReadLine());
            //                Person personToUpdate = persons.FirstOrDefault(p => p.Id == personid);

            //                if (personToUpdate != null)
            //                {
            //                    Console.Clear();
            //                    Console.WriteLine("Choose what you want to update:");
            //                    Console.WriteLine("1. Firstname");
            //                    Console.WriteLine("2. Lastname");
            //                    Console.WriteLine("3. Streetname");
            //                    Console.WriteLine("4. Housenumber");
            //                    Console.WriteLine("5. Postalcode");

            //                    if (personToUpdate is Athlete)
            //                    {
            //                        Console.WriteLine("6. LocationId");
            //                        Console.WriteLine("7. WorkoutId");
            //                    }
            //                    Console.WriteLine("Enter your choice:");
            //                    string updateChoice = Console.ReadLine();
            //                    switch (updateChoice)
            //                    {
            //                        case "1":
            //                            personToUpdate.FirstName = InputValue("Person FirstName");
            //                            break;

            //                        case "2":
            //                            personToUpdate.LastName = InputValue("Person LastName");
            //                            break;

            //                        case "3":
            //                            personToUpdate.StreetName = InputValue("Person StreetName");
            //                            break;

            //                        case "4":
            //                            personToUpdate.HouseNumber = InputValue("Person HouseNumber");
            //                            break;

            //                        case "5":
            //                            personToUpdate.PostalCode = InputValue("Person PostalCode");
            //                            break;

            //                        case "6":
            //                            if (personToUpdate is Athlete athleteToUpdate)
            //                            {
            //                                athleteToUpdate.LocationId = Convert.ToInt32(InputValue("Person LocationId"));
            //                            }
            //                            break;

            //                        case "7":
            //                            if (personToUpdate is Athlete athleteToUpdateWorkoutId)
            //                            {
            //                                athleteToUpdateWorkoutId.WorkoutId = Convert.ToInt32(InputValue("Person WorkoutId"));
            //                            }
            //                            break;
            //                    }
            //                    personid = personToUpdate.Id;
            //                    personToUpdate.UpdatePerson();
            //                    Console.WriteLine("Person updated successfully!");
            //                    Console.WriteLine("==============");
            //                }
            //                else
            //                {
            //                    Console.WriteLine("Person not found!");
            //                }
            //                Console.WriteLine("Do you want to update another person? (Y/N)");
            //                string choice = Console.ReadLine();
            //                if (choice.ToUpper() != "Y")
            //                {
            //                    updatePerson = false;
            //                }
            //            }
            //            break;

            //        case "2":
            //            // Delete a person
            //            List<Person> personsToDelete = Person.GetPersons();
            //            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            //            Console.WriteLine($"| {"PersonID",-10} | {"Firstname",-10} | {"Lastname",-10} | {"Streetname",-12} | {"Housenumber",-10} | {"Postalcode",-10} | {"LocationID",-10} | {"WorkoutId",-10}");
            //            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            //            foreach (Person person in personsToDelete)
            //            {
            //                if (person is Athlete athlete)
            //                {
            //                    Console.WriteLine($"| {person.Id,-10} | {person.FirstName,-10} | {person.LastName,-10} | {person.StreetName,-12} | {person.HouseNumber,-11} | {person.PostalCode,-10} | {athlete.LocationId,-10} | {athlete.WorkoutId,-10}");
            //                }
            //                else if (person is Coach coach)
            //                {
            //                    Console.WriteLine($"| {person.Id,-10} | {person.FirstName,-10} | {person.LastName,-10} | {person.StreetName,-12} | {person.HouseNumber,-11} | {person.PostalCode,-10} |");
            //                }
            //            }
            //            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            //            Console.WriteLine("==============================================");
            //            Console.WriteLine("Enter the ID of the person you want to Delete:");
            //            int personIdToDelete = Convert.ToInt32(Console.ReadLine());
            //            Person personToDelete = personsToDelete.FirstOrDefault(p => p.Id == personIdToDelete);

            //            if (personToDelete != null)
            //            {
            //                personToDelete.DeletePerson();
            //                Console.WriteLine("Person deleted successfully!");
            //            }
            //            else
            //            {
            //                Console.WriteLine("Person not found!");
            //            }
            //            break;

            //        case "3":
            //            // Create a person
            //            Console.WriteLine("Do you want to create a 1. Coach or 2. Athlete?");
            //            string createChoice = Console.ReadLine();
            //            if (createChoice == "1")
            //            {
            //                string firstname = InputValue("Enter Coach Firstname:");
            //                string lastname = InputValue("Enter Coach Lastname:");
            //                string streetname = InputValue("Enter Coach Streetname:");
            //                string housenumber = InputValue("Enter Coach Housenumber:");
            //                string postalcode = InputValue("Enter Coach Postalcode:");
            //                Coach coach = new Coach(0, firstname, lastname, streetname, housenumber, postalcode);
            //                coach.CreatePerson();
            //                Console.WriteLine("Coach created successfully!");
            //            }
            //            else if (createChoice == "2")
            //            {
            //                string firstname = InputValue("Enter Athlete Firstname:");
            //                string lastname = InputValue("Enter Athlete Lastname:");
            //                string streetname = InputValue("Enter Athlete Streetname:");
            //                string housenumber = InputValue("Enter Athlete Housenumber:");
            //                string postalcode = InputValue("Enter Athlete Postalcode:");
            //                int locationid = Convert.ToInt32(InputValue("Enter Athlete LocationId:"));
            //                int workoutid = Convert.ToInt32(InputValue("Enter Athlete WorkoutId:"));
            //                Athlete athlete = new Athlete(0, firstname, lastname, streetname, housenumber, postalcode, workoutid, locationid);
            //                athlete.CreatePerson();
            //                Console.WriteLine("Athlete created successfully!");
            //            }
            //            break;
            //    }
            //}

            //static string InputValue(string prompt)
            //{
            //    Console.WriteLine(prompt);
            //    string input = Console.ReadLine();
            //    while (string.IsNullOrEmpty(input))
            //    {
            //        Console.WriteLine(prompt + " cannot be empty. Please enter a value:");
            //        input = Console.ReadLine();
            //    }
            //    return input;
            //}
        }
    }
}
