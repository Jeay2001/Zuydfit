using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Zuydfit;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welkom bij Zuydfit!");


            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Feedback> feedbacks = new List<Feedback>();

            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location, feedbacks);
            Coach coach = new Coach(1, "zuch", "mabaulz", "zweetweg", "69", "4200", []);
            Administrator administrator = new Administrator(1, "karel", "kerel", "hebikniet", "66", "9999", []);

            while (true) // Oneindige lus om de console open te houden
            {
                MainMenu(athlete, coach, administrator);
            }
        }


            bool flag = true;
            while (flag)
            {
                Console.WriteLine("");
                MainMenu(athlete);
                flag = false;
            }
        }


        /* Main Menu */
        public static void MainMenu(Athlete athlete)
        {
            List<string> options = new List<string> {
                "Athlete",
                "Coach",
                "Administrator",
            };
            int choice = DisplayMenuOptions(options, "Main menu - select the type you want to login with");
            switch (choice)
            {
                case 1:
                    AthleteMainMenu(athlete);
                    break;
                case 2:
                    CoachMainMenu();
                    break;
                case 3:
                    AdministratorMainMenu();
                    break;
            }
        }


        /* Athlete menu's */
        public static void AthleteMainMenu(Athlete athlete)
        {
            List<string> options = [
                "View workouts",
                "New workout",
                "My progression",
                "View feedback"
            ];
            int choice = DisplayMenuOptions(options, "Main menu, choose one of the options");

            switch (choice)
            {
                case 1:
                    // View workouts
                    AthleteViewWorkouts(athlete);
                    break;
                case 2:
                    // New workout
                    AthleteCreateWorkout(athlete);
                    break;
                case 3:
                    // Progression
                    CoachAthleteProgression(athlete);
                    break;
                case 4:
                    // Instructor feedback
                    AthleteFeedback(athlete);
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        public static void AthleteViewWorkouts(Athlete athlete)
        {
            List<string> options = [
                "Go back",
            ];

            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            PrintWorkouts(workouts);
            foreach (Workout workout in workouts)
            {
                options.Add(workout.Date.ToString("dd/MM/yyyy"));
            }
            int choice = DisplayMenuOptions(options, "Choose a workout to view/edit");

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Workout workout = workouts[choice - 2];
                AthleteSingleWorkout(athlete, workout);
            }
        }

        public static void AthleteCreateWorkout(Athlete athlete)
        {
            List<string> options = [
                "Add exercise",
                "Go back",
                "Main menu",
            ];
            Workout workout = new Workout(0, DateTime.Now);
            workout = workout.CreateWorkout(workout, athlete);
            int choice = DisplayMenuOptions(options, "Create workout menu", workout);
                
            if (choice == 1)
            {
                Exercise newExercise = AthleteCreateExercise(workout);
                workout.Exercises.Add(newExercise);
                AthleteSingleWorkout(athlete, workout);
            }
            else if (choice == 2)
            {
                AthleteViewWorkouts(athlete);
            }
            else if (choice == 3)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void CoachAthleteProgression(Athlete athlete)
        {
            List<string> options = [
                "Go back",
            ];

            int[] data = { 5, 6, 8, 10, 11, 11, 8, 9, 12, 15 }; // Sample data

            // Find the maximum value in the data
            int maxValue = 0;
            foreach (int value in data)
            {
                if (value > maxValue)
                    maxValue = value;
            }

            // Draw the graph
            Console.WriteLine("   ^");
            Console.WriteLine("   |");
            Console.WriteLine("   |");
            for (int i = maxValue; i > 0; i--)
            {
                Console.Write($"   |");
                foreach (int value in data)
                {
                    if (value >= i)
                        Console.Write(" * ");
                    else
                        Console.Write("   ");
                }
                Console.WriteLine();
            }

            // Print the x-axis labels
            Console.Write("   +");
            for (int i = 0; i < data.Length * 3; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine(">");


            int choice = DisplayMenuOptions(options, "View your progression", null, false);

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void AthleteFeedback(Athlete athlete)
        {
            List<string> options = [
                "Go back",
            ];

            int choice = DisplayMenuOptions(options, "To do - View feedback");

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void AthleteSingleWorkout(Athlete athlete, Workout workout)
        {
            List<string> options = [
                "Add exercise",
                "Remove exercise",
                "Delete workout",
                "Go back",
                "Main menu",
            ];

            int choice = DisplayMenuOptions(options, "Single workout", workout);

            if (choice == 1)
            {
                // Add exercise
                Exercise newExercise = AthleteCreateExercise(workout);
                workout.Exercises.Add(newExercise);
                AthleteSingleWorkout(athlete, workout);
            }
            else if (choice == 2)
            {
                // Remove exercise
                List<Exercise> exercises = AthleteRemoveExerciseFromList(workout.Exercises);
                workout.Exercises = exercises;
                AthleteSingleWorkout(athlete, workout);
            }
            else if (choice == 3)
            {
                // Delete workout
                workout.DeleteWorkout();
                AthleteViewWorkouts(athlete);
            }
            else if (choice == 4)
            {
                // Previous menu
                AthleteViewWorkouts(athlete);
            }
            else if (choice == 5)
            {
                // Main menu
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static Exercise AthleteCreateExercise(Workout workout)
        {
            Console.Write("Exercise name: ");
            string exerciseName = Console.ReadLine();
            Console.Write("Exercise type (strength or cardio): ");
            string exerciseType = Console.ReadLine();
            List<Sets> sets = new List<Sets>();
            if (exerciseType == "strength")
            {
                bool addSetsFlag = true;
                while (addSetsFlag)
                {
                    Console.WriteLine("Add set? (y/n)");
                    string newSetBool = Console.ReadLine();
                    if (newSetBool == "y")
                    {
                        Console.Write("Weight: (in Kg)");
                        double weight = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Reps: ");
                        int reps = Convert.ToInt32(Console.ReadLine());
                        Sets set = new Sets(0, reps, weight);
                        sets.Add(set);
                    }
                    else
                    {
                        addSetsFlag = false;
                    }
                }

                Strength newExercise = new Strength(0, exerciseName, sets);
                Exercise createdExercise = newExercise.CreateExercise(workout, newExercise);
                return createdExercise;
            }
            else if (exerciseType == "cardio")
            {
                // To do - cardio
                Console.Write("Exercise duration (leave empty if needed): ");
                string duration = Console.ReadLine();
                Console.Write("Exercise distance (leave empty if needed): ");
                string distance = Console.ReadLine();

                Cardio newExercise = new Cardio(0, exerciseName, duration, distance);
                newExercise.CreateExercise(workout, newExercise);
                return newExercise;
            }
            // To do - exercise type controleren
            return new Strength(0, "test");
        }

        public static List<Exercise> AthleteRemoveExerciseFromList(List<Exercise> exercises)
        {
            List<string> options = [
                "Go back",
            ];
            foreach (Exercise exercise in exercises)
            {
                options.Add(exercise.Name);
            }
            int choice = DisplayMenuOptions(options, "Choose an exercise to remove");

            if (choice == 1)
            {
                // Previous menu
                return exercises;
            }
            else
            {
                Exercise exerciseToRemove = exercises[choice - 2];
                exerciseToRemove.DeleteExercise();
                exercises.RemoveAt(choice - 2);
                return exercises;
            }
        }


        /* Coach Menu's */
        static void CoachMainMenu()
        {
            List<string> options = new List<string> {
                "Create Athlete",
                "Show Athlete Progression",
                "View all activities",
                "Create activity",
                "Read Athlete Feedback",
                "To do - Give Athlete Feedback",
            };

            int choice = DisplayMenuOptions(options, "Coach Menu");

            switch (choice)
            {
                case 1:
                    CoachCreateAthlete();
                    break;
                case 2:
                    CoachSeeAthleteProgression();
                    break;
                case 3:
                    CoachViewAllActivities();
                    break;
                case 4:
                    CoachCreateActivity();
                    break;
                case 5:
                    CoachReadAthleteFeedback();
                    break;
                case 6:
                    CoachCreateFeedback();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        public static void CoachCreateAthlete()
        {
            Console.Clear();
            Console.WriteLine("Adding a new person:");

            // Verzamel de gegevens van de nieuwe persoon
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


            List<Feedback> feedback = new List<Feedback>();
            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);

            Person newPerson = new Athlete(1, firstName, lastName, streetName, houseNumber, postalCode, location, feedback);
            newPerson.CreatePerson();
            Console.WriteLine("Athlete added succesfully!");
        }

        public static void CoachSeeAthleteProgression()
        {
            List<Person> persons = Person.GetPersons();

            foreach (Person person in persons)
            {
                if (person is Athlete)
                {
                    Console.WriteLine($"Athlete: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            Console.WriteLine("Choose a person to view progression:");
            int id = Convert.ToInt32(Console.ReadLine());

            if (persons.Find(p => p.Id == id) is Athlete athlete)
            {
                Console.Clear();

                List<string> options = [
                    "Go back",
                ];

                Console.WriteLine($"Progression of {athlete.FirstName} {athlete.LastName}");
                Console.WriteLine($"");

                int[] data = { 5, 6, 8, 10, 11, 11, 8, 9, 12, 15 }; // Sample data

                // Find the maximum value in the data
                int maxValue = 0;
                foreach (int value in data)
                {
                    if (value > maxValue)
                        maxValue = value;
                }

                // Draw the graph
                Console.WriteLine("   ^");
                Console.WriteLine("   |");
                Console.WriteLine("   |");
                for (int i = maxValue; i > 0; i--)
                {
                    Console.Write($"   |");
                    foreach (int value in data)
                    {
                        if (value >= i)
                            Console.Write(" * ");
                        else
                            Console.Write("   ");
                    }
                    Console.WriteLine();
                }

                // Print the x-axis labels
                Console.Write("   +");
                for (int i = 0; i < data.Length * 3; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine(">");
                Console.WriteLine("");

                DisplayMenuOptions(options, "", null, false);

            }
            else
            {
                Console.WriteLine("Invalid choice");
            }

        }

        public static void CoachViewAllActivities()
        {
            List<Activity> activities = Activity.ReadAllActivities();
            foreach (Activity activity in activities)
            {
                Console.WriteLine($"Activity: {activity.Id} - {activity.Name} - {activity.Duration}");
            }
        }

        public static void CoachCreateActivity()
        {
            Console.Clear();
            Console.WriteLine("Creating new activity:");

            Console.Write("Enter activity name: ");
            string name = Console.ReadLine();
            Console.Write("Enter activity duration: ");
            string duration = Console.ReadLine();

            Activity newActivity = new Activity(1, name, duration);
            newActivity.CreateActivity();
            Console.WriteLine("Activity Created Succesfully!");

            CoachMainMenu();
        }
        
        public static void CoachAthleteProgression()
        {
            List<Person> persons = Person.GetPersons();
            foreach (Person person in persons)
            {
                if (person is Athlete)
                {
                    Console.WriteLine($"Athlete: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine("Choose a person to view progression:");
            int id = Convert.ToInt32(Console.ReadLine());
            if (persons.Find(p => p.Id == id) is Athlete athlete)
            {
                List<string> options = [
                "Go back",
            ];

                int[] data = { 5, 6, 8, 10, 11, 11, 8, 9, 12, 15 }; // Sample data

                // Find the maximum value in the data
                int maxValue = 0;
                foreach (int value in data)
                {
                    if (value > maxValue)
                        maxValue = value;
                }

                // Draw the graph
                Console.WriteLine("   ^");
                Console.WriteLine("   |");
                Console.WriteLine("   |");
                for (int i = maxValue; i > 0; i--)
                {
                    Console.Write($"   |");
                    foreach (int value in data)
                    {
                        if (value >= i)
                            Console.Write(" * ");
                        else
                            Console.Write("   ");
                    }
                    Console.WriteLine();
                }

                // Print the x-axis labels
                Console.Write("   +");
                for (int i = 0; i < data.Length * 3; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine(">");
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }


            //int choice = DisplayMenuOptions(options, "View your progression", null, false);

            //if (choice == 1)
            //{
            //    AthleteMainMenu(athlete);
            //}
            //else
            //{
            //    Console.WriteLine("Invalid choice");
            //}
        }

        public static void CoachReadAthleteFeedback()
        {
            Console.Clear();
            Console.WriteLine("Choose a person to view progression:");

            List<Person> persons = Person.GetPersons();

            foreach (Person person in persons)
            {
                if (person is Athlete)  
                {
                    Console.WriteLine($"Athlete: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            int id = Convert.ToInt32(Console.ReadLine());

            Console.Clear();
            if (persons.Find(p => p.Id == id) is Athlete athlete)
            {
                Console.WriteLine($"Feedback van {athlete.FirstName} {athlete.LastName}:");
                Console.WriteLine("");
            }
            
            List<Feedback> feedbacks = Feedback.ReadAllFeedback();
            foreach (Feedback feedback in feedbacks)
            {
                Console.WriteLine($"Feedback: {feedback.Id} - {feedback.FeedbackMessage} - {feedback.Date}");
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();

            CoachMainMenu();
        }
        
        public static void CoachCreateFeedback()
        {
            Console.Clear();
            Console.WriteLine("Choose a person to give them feedback:");

            List<Person> persons = Person.GetPersons();

            foreach (Person person in persons)
            {
                if (person is Athlete)
                {
                    Console.WriteLine($"Athlete: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            int athleteId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter feedback message: ");
            string message = Console.ReadLine();

            // To do - create feedback in PersonFeedback table in database
            Feedback newFeedback = new Feedback(1, message, DateTime.Now);
            newFeedback.CreateFeedback();
            Console.WriteLine("Feedback Created Succesfully!");

        }


        /* Administrator menu's */
        static void AdministratorMainMenu()
        {
            List<string> options = new List<string> {
                "View coaches",
                "Add coach",
                "Delete coach",
                "Update coach"
            };
            int choice = DisplayMenuOptions(options, "Administrator Menu");

            switch (choice)
            {
                case 1:
                    AdministratorViewCoaches();
                    break;
                case 2:
                    AdministratorCreateCoach();
                    break;
                case 3:
                    AdministratorDeleteCoach();
                    break;
                case 4:
                    AdministratorUpdateCoach();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        public static void AdministratorViewCoaches()
        {

            Console.Clear();
            Console.WriteLine("Coaches:");

            List<Person> persons = Person.GetPersons();
            foreach (Person person in persons)
            {
                if (person is Coach)
                {
                    Console.WriteLine($"Coach: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        public static void AdministratorCreateCoach()
        {
            Console.Clear();
            Console.WriteLine("Adding a new coach:");

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

        public static void AdministratorDeleteCoach()
        {
            Console.Clear();
            Console.WriteLine("Deleting a coach:");

            Console.WriteLine("Select the coach to delete:");

            List<Person> persons = Person.GetPersons();
            foreach (Person person in persons)
            {
                if (person is Coach)
                {
                    Console.WriteLine($"Coach: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            if (persons.Count == 0)
            {
                Console.WriteLine("No coaches available to delete.");
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }

            // Vraag de gebruiker om de keuze van coach
            Console.WriteLine("Enter the number of the coach to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Person personToUpdate = persons.Find(p => p.Id == id);
            personToUpdate.DeletePerson();

            Console.WriteLine("Coach deleted successfully.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static void AdministratorUpdateCoach()
        {
            static string InputValue(string prompt)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                while (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(prompt + " cannot be empty. Please enter a value:");
                    input = Console.ReadLine();
                }
                return input;
            }
            List<Person> persons = Person.GetPersons();
            foreach (Person person in persons)
            {
                if (person is Coach)
                {
                    Console.WriteLine($"Coach: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine("Choose a person to Update:");
            int id = Convert.ToInt32(Console.ReadLine());
            Person personToUpdate = persons.Find(p => p.Id == id);

            if (personToUpdate != null)
            {
                Console.Clear();
                Console.WriteLine("Choose what you want to update:");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                Console.WriteLine("3. Street Name");
                Console.WriteLine("4. House Number");
                Console.WriteLine("5. Postal Code");

                if (personToUpdate is Coach)
                {
                    Console.WriteLine("6. Feedback ID");
                }

                Console.WriteLine("Enter your choice:");
                string updateChoice = Console.ReadLine();
                switch (updateChoice)
                {
                    case "1":
                        personToUpdate.FirstName = InputValue("First Name");
                        break;
                    case "2":
                        personToUpdate.LastName = InputValue("Last Name");
                        break;
                    case "3":
                        personToUpdate.StreetName = InputValue("Street Name");
                        break;
                    case "4":
                        personToUpdate.HouseNumber = InputValue("House Number");
                        break;
                    case "5":
                        personToUpdate.PostalCode = InputValue("Postal Code");
                        break;
                    case "6":
                        if (personToUpdate is Athlete athleteToUpdate)
                        {

                            int locationId = Convert.ToInt32(InputValue("Location ID"));
                            athleteToUpdate.Location.Id = locationId;
                        }
                        else if (personToUpdate is Coach coachToUpdate)
                        {
                            int feedbackId = Convert.ToInt32(InputValue("Feedback ID"));
                            coachToUpdate.Feedback.Id = feedbackId;
                        }
                        break;
                    case "7":
                        if (personToUpdate is Athlete athleteToUpdateFeedback)
                        {
                            int feedbackId = Convert.ToInt32(InputValue("Feedback ID"));
                            athleteToUpdateFeedback.Feedback.Id = feedbackId;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                personToUpdate.UpdatePerson();
                Console.WriteLine("Person updated successfully!");
                Console.WriteLine("==============");

            }

        }


        /* Print Method's */
        public static void PrintWorkouts(List<Workout> workouts)
        {
            foreach (Workout workout in workouts)
            {
                PrintWorkout(workout);
                Console.WriteLine();
            }
        }

        public static void PrintWorkout(Workout workout)
        {
            Console.WriteLine($"Workout Date: {workout.Date.ToString("dd/MM/yyyy")}");


            foreach (Exercise exercise in workout.Exercises)
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
                        foreach (Sets set in strength.Sets)
                        {
                            Console.WriteLine($"    - Weight: {set.Weight} Reps: {set.Reps}");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public static int DisplayMenuOptions(List<string> options, string title = "", Workout workout = null, bool clearConsole = true)
        {
            if (clearConsole)
            {
                Console.Clear();
            }
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
    }
}




