using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Zuydfit;
using Zuydfit.DataAccessLayer;

using System;

namespace Zuydfit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welkom bij Zuydfit!");

           


            //List<Workout> workouts = Workout.ReadWorkouts();
            //PrintWorkouts(workouts);

            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Feedback> feedbacks = new List<Feedback>();
            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location, feedbacks);
            Administrator administrator = new Administrator(1, "karel", "kerel", "hebikniet", "66", "9999", [], []);

            //AthleteMainMenu();

            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            PrintWorkouts(workouts);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Ingelogd als atleet");
                Console.WriteLine("");
                AdministratorMenu(administrator);
                flag = false;
            }


        static void AdministratorMenu(Administrator administrator)
        {
            List<string> options = new List<string> {
        "View coaches",
        "Add coach",
        "Delete coach"
    };
            int choice = DisplayMenuOptions(options, "Administrator Menu");

            switch (choice)
            {
                case 1:
                    ViewCoaches(administrator);
                    break;
                case 2:
                    AddCoach(administrator);
                    break;
                case 3:
                    DeleteCoach(administrator);
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        public static void ViewCoaches(Administrator administrator)
        {
            // Je zou de coaches moeten ophalen vanuit de data access layer of ergens anders
            List<Coach> coaches = GetCoaches();

            Console.Clear();
            Console.WriteLine("Coaches:");

            if (coaches.Count == 0)
            {
                Console.WriteLine("No coaches available.");
            }
            else
            {
                for (int i = 0; i < coaches.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {coaches[i].FirstName}");
                }
            }

            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        private static List<Coach> GetCoaches()
        {

            List<Coach> coaches = new List<Coach>
    {
                new Coach(1, "Coach 1", "LastName", "StreetName", "HouseNumber", "PostalCode", new List<Feedback>()),
                new Coach(2, "Coach 2", "LastName", "StreetName", "HouseNumber", "PostalCode", new List<Feedback>()),
                new Coach(3, "Coach 3", "LastName", "StreetName", "HouseNumber", "PostalCode", new List<Feedback>())
    };

            return coaches;
        }

public static void AddCoach(Administrator administrator)
{
    Console.Clear();
    Console.WriteLine("Adding a new coach:");

    // Vraag de gebruiker om de gegevens van de nieuwe coach in te voeren
    Console.Write("Enter first name: ");
    string firstName = Console.ReadLine();
    Console.Write("Enter last name: ");
    string lastName = Console.ReadLine();
    Console.Write("Enter street name: ");
    string streetName = Console.ReadLine();
    Console.Write("Enter house number: ");
    string houseNumber = Console.ReadLine();
    Console.Write("Enter postal code: ");
    string postalCode = Console.ReadLine();

            // Hier kun je verdere inputvalidatie toevoegen, zoals het controleren of de ingevoerde gegevens geldig zijn

            List<Feedback> feedback = new List<Feedback>();

    // Maak een nieuwe coach met de ingevoerde gegevens
    Person newCoach = new Coach(1, firstName, lastName, streetName, houseNumber, postalCode, feedback);
    newCoach.CreatePerson();
    // Voeg de nieuwe coach toe aan de lijst van coaches van de administrator
    //administrator.Coaches.Add(newCoach);


    Console.WriteLine("Coach added successfully.");
    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
}




        public static void DeleteCoach(Administrator administrator)
        {
            Console.Clear();
            Console.WriteLine("Deleting a coach:");

            // Laat eerst de lijst met coaches zien om te kiezen welke coach te verwijderen
            Console.WriteLine("Select the coach to delete:");

            List<Coach> coaches = administrator.Coaches;

            if (coaches.Count == 0)
            {
                Console.WriteLine("No coaches available to delete.");
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < coaches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {coaches[i].FirstName}");
            }

            // Vraag de gebruiker om de keuze van coach
            Console.Write("Enter the number of the coach to delete: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > coaches.Count)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                Console.Write("Enter the number of the coach to delete: ");
            }

            // Verwijder de geselecteerde coach
            Coach coachToDelete = coaches[choice - 1];
            administrator.Coaches.Remove(coachToDelete);

            Console.WriteLine("Coach deleted successfully.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static int DisplayMenuOptions(List<string> options, string title = "", Workout workout = null)
        {
            Console.Clear();
            if (title != "")
            {
                Console.WriteLine(title);
                Console.WriteLine();
            }
            if (workout != null)
            {
                PrintWorkout(workout);
            }
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());
            return choice;
        }

            List<Workout> workouts = Workout.ReadWorkouts(athlete);

            foreach (Workout workout in workouts)
            {
                Console.WriteLine($"Workout Date: {workout.Date.ToString("dd/MM/yyyy")}");
            }


            //Workout workout = workouts[0];

            //workout.Exercises.Add(new Cardio(1,"Running", "30 minutes", "5 km"));


            List<Exercise> exercises = Exercise.ReadExerciseListFromAthlete(athlete);

            foreach (Exercise exercise in exercises)
            {
                Console.WriteLine($"Exercise: {exercise.Name}");

                if (exercise is Cardio)
                {
                    Cardio cardio = (Cardio)exercise;
                    Console.WriteLine($"  Type: Cardio");
                    if (cardio.Duration != "")
                    {
                        Console.WriteLine($"  Duration: {cardio.Duration}");
                    }
                    if (cardio.Distance != "")
                    {
                        Console.WriteLine($"  Distance: {cardio.Distance}");
                    }
                }
                else if (exercise is Strength)
                {
                    Strength strength = (Strength)exercise;
                    if (strength.Sets.Count > 0)
                    {
                        Console.WriteLine("  Sets:");
                        foreach (Set set in strength.Sets)
                        {
                            Console.WriteLine($"    - Weight: {set.Weight} Reps: {set.Reps}");
                        }
                    }
                }
                Console.WriteLine();
            }
        }


        //public static void AthleteMainMenu()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Choose one of the options:");
        //    Console.WriteLine("1. View workouts");
        //    Console.WriteLine("2. New workout");
        //    Console.WriteLine("3. My progression");
        //    Console.WriteLine("4. View feedback");

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());
        //    switch(choice)
        //    {
        //        case 1:
        //            AthleteViewWorkouts();
        //            break;
        //        case 2:
        //            Console.WriteLine("New workout");
        //            break;
        //        case 3:
        //            Console.WriteLine("My progression");
        //            break;
        //        case 4:
        //            Console.WriteLine("View instructor feedback");
        //            break;
        //        default:
        //            Console.WriteLine("Invalid choice");
        //            break;
        //    }
        //}

        //public static void AthleteViewWorkouts()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Choose a workout to view/edit:");
        //    Console.WriteLine("");
        //    Console.WriteLine("1. Go back");
        //    List<Workout> workouts = Workout.ReadWorkouts();
        //    int index = 2;
        //    foreach (Workout workout in workouts)
        //    {
        //        Console.WriteLine($"{index}. {workout.Date.ToString("dd/MM/yyyy")}");
        //        index ++;
        //    }

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());
        //    if (choice == 1)
        //    {
        //        AthleteMainMenu();
        //    }
        //    else
        //    {
        //        Workout workout = workouts[choice - 2];
        //        AthleteSingleWorkoutMenu(workout);                
        //    }
        //}

        //public static void AthleteSingleWorkoutMenu(Workout workout)
        //{
        //    Console.Clear();
        //    Console.WriteLine("1. View workout");
        //    Console.WriteLine("2. Edit workout");
        //    Console.WriteLine("3. Delete workout");
        //    Console.WriteLine("4. Go back");
        //    Console.WriteLine("5. Main menu");

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        AthleteViewWorkout(workout);
        //    }
        //    if (choice == 2)
        //    {
        //        AthleteEditWorkout(workout);
        //    }
        //    else if (choice == 3)
        //    {
        //        //workout.DeleteWorkout();
        //        AthleteViewWorkouts();

        //    }
        //    else if (choice == 4)
        //    {
        //        // Previous menu
        //        AthleteViewWorkouts();
        //    } else if (choice == 5)
        //    {
        //        // Main menu
        //        AthleteMainMenu();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}


        //public static void AthleteViewWorkout(Workout workout) {

        //    Console.Clear();
        //    PrintWorkout(workout);
        //    Console.WriteLine("");
        //    Console.WriteLine("1. Go back");
        //    Console.WriteLine("2. Main menu");

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        // Previous menu
        //        AthleteSingleWorkoutMenu(workout);
        //    } else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu();
        //    } else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}

        //public static void AthleteEditWorkout(Workout workout)
        //{
        //    Console.Clear();
        //    //PrintWorkout(workout);
        //    Console.WriteLine("1. Go back");
        //    Console.WriteLine("2. Main menu");
        //    int index = 3;
        //    foreach (Exercise exercise in workout.Exercises)
        //    {
        //        Console.WriteLine($"{index}. {exercise.Name}");
        //        if (exercise is Cardio)
        //        {
        //            Cardio cardio = (Cardio)exercise;
        //            Console.WriteLine($"  Type: Cardio");
        //            if (cardio.Duration != "")
        //            {
        //                Console.WriteLine($"  Duration: {cardio.Duration}");
        //            }
        //            if (cardio.Distance != "")
        //            {
        //                Console.WriteLine($"  Distance: {cardio.Distance}");
        //            }
        //        }
        //        else if (exercise is Strength)
        //        {
        //            Strength strength = (Strength)exercise;
        //            if (strength.Sets.Count > 0)
        //            {
        //                Console.WriteLine("  Sets:");
        //                foreach (Set set in strength.Sets)
        //                {
        //                    Console.WriteLine($"    - Weight: {set.Weight} Reps: {set.Reps}");
        //                }
        //            }
        //        }
        //        index++;
        //    }

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        // Previous menu
        //        AthleteSingleWorkoutMenu(workout);
        //    }
        //    else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu();
        //    }
        //    else
        //    {
        //        Exercise exercise = workout.Exercises[choice - 3];
        //        AthleteEditExercise(workout, exercise);
        //    }
        //}


        //public static void AthleteEditExercise(Workout workout, Exercise exercise)
        //{
        //    Console.Clear();

        //    Console.WriteLine($"Edit {exercise.Name}");
        //    Console.WriteLine("");
        //    Console.WriteLine("1. Go back");
        //    Console.WriteLine("2. Main menu");
        //    Console.WriteLine("3. Add set");
        //    Console.WriteLine("4. Remove set");
        //    if (exercise is Cardio)
        //    {
        //        // To do - Show cardio exercise
        //    }
        //    else if (exercise is Strength)
        //    {
        //        Strength strength = (Strength)exercise;
        //        int index = 5;
        //        foreach (Set set in strength.Sets)
        //        {
        //            Console.WriteLine($"{index} - Weight: {set.Weight} Reps: {set.Reps}");
        //            index ++;
        //        }
        //    }

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        // Previous menu
        //        AthleteEditWorkout(workout);
        //    }
        //    else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu();
        //    } else
        //    {
        //        if (exercise is Strength)
        //        {
        //            Strength strength = (Strength)exercise;
        //            AthleteEditSet(workout, exercise, strength.Sets[choice - 5]);
        //        } else if (exercise is Cardio)
        //        {
        //            // To do - Edit cardio
        //        }
        //    }
        //}

        //public static void AthleteEditSet(Workout workout, Exercise exercise, Set set)
        //{
        //    Console.Clear();

        //    Console.WriteLine($"Editing {exercise.Name}");
        //    Console.WriteLine("");

        //    Console.Write("Weight: ");
        //    string weight= Console.ReadLine();
        //    Console.Write("Reps: ");
        //    int reps = Convert.ToInt32(Console.ReadLine());

        //    // To do - Update set
        //    AthleteEditExercise(workout, exercise);

        //}





        //public static void PrintWorkouts(List<Workout> workouts)
        //{
        //    foreach (Workout workout in workouts)
        //    {
        //        PrintWorkout(workout);
        //        Console.WriteLine();
        //    }
        //}

        //public static void PrintWorkout(Workout workout)
        //{
        //    Console.WriteLine($"Workout Date: {workout.Date.ToString("dd/MM/yyyy")}");


        //    foreach (Exercise exercise in workout.Exercises)
        //    {
        //        Console.WriteLine($"Exercise: {exercise.Name}");

        //        if (exercise is Cardio)
        //        {
        //            Cardio cardio = (Cardio)exercise;
        //            Console.WriteLine($"  Type: Cardio");
        //            if (cardio.Duration != "")
        //            {
        //                Console.WriteLine($"  Duration: {cardio.Duration}");
        //            }
        //            if (cardio.Distance != "")
        //            {
        //                Console.WriteLine($"  Distance: {cardio.Distance}");
        //            }
        //        }
        //        else if (exercise is Strength)
        //        {
        //            Strength strength = (Strength)exercise;
        //            if (strength.Sets.Count > 0)
        //            {
        //                Console.WriteLine("  Sets:");
        //                foreach (Set set in strength.Sets)
        //                {
        //                    Console.WriteLine($"    - Weight: {set.Weight} Reps: {set.Reps}");
        //                }
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //}


        //    List<Person> persons = Person.GetPersons();
        //    foreach(Person person in persons)
        //    {
        //        Console.WriteLine(person.Id);
        //        Console.WriteLine(person.FirstName);
        //        Console.WriteLine(person.LastName);
        //        Console.WriteLine(person.StreetName);
        //        Console.WriteLine(person.HouseNumber);
        //        Console.WriteLine(person.PostalCode);
        //        if (person is Athlete)
        //        {
        //            Athlete athlete = (Athlete)person;
        //            foreach (Workout workout in athlete.Workouts)
        //            {
        //                Console.WriteLine(workout.Id);
        //                Console.WriteLine(workout.Date);
        //                foreach (Exercise exercise in workout.Exercises)
        //                {
        //                    Console.WriteLine(exercise.Name);
        //                    if (exercise is Cardio)
        //                    {
        //                        Cardio cardio = (Cardio)exercise;
        //                        Console.WriteLine(cardio.Duration);
        //                        Console.WriteLine(cardio.Distance);
        //                    }
        //                    else if (exercise is Strength)
        //                    {
        //                        Strength strength = (Strength)exercise;
        //                        foreach (Set set in strength.Sets)
        //                        {
        //                            Console.WriteLine(set.Reps);
        //                            Console.WriteLine(set.Weight);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

    }
}








