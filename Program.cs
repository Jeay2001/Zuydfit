using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zuydfit!");


            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Feedback> feedbacks = new List<Feedback>();

            Athlete athlete = new Athlete(26, "John", "Doe", "Street", "1", "1234", [], location, []);

            //List<Feedback> feedbacks = Feedback.ReadPersonFeedback(athlete.Id);
            //Console.WriteLine("Feedback:");
            //foreach (Feedback fb in feedbacks)
            //{
            //    Console.WriteLine("test");
            //    Console.WriteLine($"- {fb.FeedbackMessage}");
            //}


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
                "Duplicate workout",
                "My progression",
                "View feedback",
                "My activities",
                "Join activity",
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
                    // Duplicate workout
                    AthleteDuplicateWorkout(athlete);
                    break;
                case 4:
                    // Progression
                    AthleteProgression(athlete);
                    break;
                case 5:
                    // Instructor feedback
                    AthleteFeedback(athlete);
                    break;
                case 6:
                    // Join activity
                    AthleteViewActivities(athlete);
                    break;
                case 7:
                    // Join activity
                    AthleteJoinActivity(athlete);
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

        public static void AthleteProgression(Athlete athlete)
        {
            Console.Clear();
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


            int choice = DisplayMenuOptions(options, "", null, false);

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void AthleteDuplicateWorkout(Athlete athlete)
        {
            List<string> options = [
                "Go back",
            ];

            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            foreach (Workout workout in workouts)
            {
                options.Add(workout.Date.ToString("dd/MM/yyyy"));
            }


            int choice = DisplayMenuOptions(options, "Choose a workout to duplicate");

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Workout chosenWorkout = workouts[choice - 2];
                Console.WriteLine("chosenWorkout.Date");
                Console.WriteLine(chosenWorkout.Date);
                Workout duplicatedWorkout = chosenWorkout.DuplicateWorkout(athlete);
                AthleteSingleWorkout(athlete, duplicatedWorkout);

            }
        }

        public static void AthleteFeedback(Athlete athlete)
        {
            List<string> options = [
                "Go back",
            ];  
            List<Feedback> feedbacks = Feedback.ReadPersonFeedback(athlete.Id);
            Console.Clear();

            Console.WriteLine("Your feedback:");
            Console.WriteLine("");
            foreach (Feedback fb in feedbacks)
            {
                if (fb.FeedbackMessage != null || fb.FeedbackMessage != "")
                {
                    Console.WriteLine($"- {fb.FeedbackMessage}");
                }
            }
            Console.WriteLine("");

            int choice = DisplayMenuOptions(options, "", null, false);

                if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void AthleteViewActivities(Athlete athlete)
        {
            Console.Clear();
            List<string> options = new List<string>
            {
                "Go back",
            };
            List<Activity> activities = Activity.ReadAthleteActivities(athlete);

            Console.WriteLine("My activities:");
            Console.WriteLine("");
            foreach (Activity activity in activities)
            {
                Console.WriteLine($"Activity: {activity.Id} - {activity.Name} - {activity.Duration}");
            }
            Console.WriteLine("");

            int choice = DisplayMenuOptions(options, "", null, false);

            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
        }

        public static void AthleteJoinActivity(Athlete athlete)
        {
            Console.Clear();
            List<Activity> activities = Activity.ReadAllActivities();
            List<string> options = new List<string>
            {
                "Go back",
            };
            foreach (Activity activity in activities)
            {
                options.Add(activity.Name);
            }
            int choice = DisplayMenuOptions(options, "Choose an activity to join");

            if (choice == 1) { 
                // Previous menu
                AthleteMainMenu(athlete);
            } else
            {
                Activity chosenActivity = activities[choice - 2];
                chosenActivity.AddAthlete(athlete);
                AthleteJoinActivity(athlete);
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
                Console.Write("Exercise duration (leave empty if needed): ");
                string duration = Console.ReadLine();
                Console.Write("Exercise distance (leave empty if needed): ");
                string distance = Console.ReadLine();

                Cardio newExercise = new Cardio(0, exerciseName, duration, distance);
                newExercise.CreateExercise(workout, newExercise);
                return newExercise;
            }
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
                "Give Athlete Feedback",
                "View activity members",
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
                case 7:
                    ReadActivitieMembers();
                        break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        public static void ReadActivitieMembers()
        {
            Console.Clear();
            List<Activity> activities = Activity.ReadActivitieMembers();
            Console.WriteLine("Activity members:");
            foreach (Activity activity in activities)
            {
                Console.WriteLine($"ActivityId: {activity.Id}, Activity name: {activity.Name} - {activity.Duration}");
                //foreach (Machine machine in location.Machines)\\
                
                foreach(Athlete athlete in activity.Athletes)
                {
                    Console.WriteLine($"    Members: Firstname: {athlete.FirstName}, Lastname: {athlete.LastName}");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            List<string>options = new List<string>
            {

                "Go back",
            };
            int choice = DisplayMenuOptions(options,"",null,false);
            if (choice == 1)
            {
                CoachMainMenu();
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
            CoachMainMenu();

        }

        public static void CoachSeeAthleteProgression()
        {
            Console.Clear();

            List<Person> persons = Person.GetPersons();

            foreach (Person person in persons)
            {
                if (person is Athlete)
                {
                    Console.WriteLine($"{person.Id}: {person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine("");

            Console.WriteLine("Choose an athlete id to view progression:");
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

                int choice = DisplayMenuOptions(options, "", null, false);
                if (choice == 1)
                {
                    CoachMainMenu();
                } 
            }
            else
            {
                CoachSeeAthleteProgression();
            }

        }

        public static void CoachViewAllActivities()
        {
            Console.Clear();
            List<Activity> activities = Activity.ReadAllActivities();
            foreach (Activity activity in activities)
            {
                Console.WriteLine($"Activity: {activity.Id} - {activity.Name} - {activity.Duration}");
            }

            Console.WriteLine("");

            List<string> options = new List<string> {
                "Go back",
            };

            int choice = DisplayMenuOptions(options, "", null, false);

            switch (choice)
            {
                case 1:
                    CoachMainMenu();
                    break;
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

            CoachViewAllActivities();
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
            List<Person> persons = Person.GetPersons();

            foreach (Person person in persons)
            {
                if (person is Athlete)
                {
                    Console.WriteLine($"Athlete: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }

            
            Console.WriteLine("Choose a person to view Feedback:");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            if (persons.Find(p => p.Id == id) is Athlete athlete)
            {
                Console.WriteLine($"Feedback van {athlete.FirstName} {athlete.LastName}:");
                Console.WriteLine("");

                List<Feedback> feedbacks = Feedback.ReadAllFeedback();
                foreach (Feedback feedback in feedbacks)
                {
                    if (feedback.Id == id)
                    {
                        Console.WriteLine($"Feedback: {feedback.Id} - {feedback.FeedbackMessage} - {feedback.Date}");
                    }
                }
            }

            Console.WriteLine("");

            List<string> options = new List<string> {
                "Go back",
            };

            int choice = DisplayMenuOptions(options, "", null, false);

            switch (choice)
            {
                case 1:
                    CoachMainMenu();
                    break;
            }

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

            Feedback newFeedback = new Feedback(1, message, DateTime.Now);
            newFeedback = newFeedback.CreateFeedback();

            newFeedback.CreatePersonFeedback(athleteId, newFeedback.Id);
            Console.WriteLine("Feedback Created Succesfully!");


            CoachMainMenu();
        }


        /* Administrator menu's */
        static void AdministratorMainMenu()
        {
            Console.Clear();
            List<string> options = new List<string> {
            "View coaches",
            "Add coach",
            "Delete coach",
            "Update coach",
            "Show locations",
            "Show machines",
            "Show machine locations",
            "Add location",
            "Delete location",
            "Add machine",
            "Delete machine",
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
                case 5:
                    ReadLocations();
                    break;
                case 6:
                    ReadMachines();
                    break;
                case 7:
                    AdministratorReadMachineLocation();
                    break;
                case 8:
                    CreateLocation();
                    break;
                case 9:
                    DeleteLocation();
                    break;
                case 10:
                    CreateMachine();
                    break;
                case 11:
                    DeleteMachine();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
        
        public static void DeleteMachine()
        {
            Console.Clear();
            Console.WriteLine("Deleting a machine:");

            Console.WriteLine("Select the machine to delete:");

            List<Machine> machines = Machine.ReadMachines();
            foreach (Machine machine in machines)
            {
                Console.WriteLine($"Machine: {machine.Id} - {machine.Name}");
            }

            if (machines.Count == 0)
            {
                Console.WriteLine("No machines available to delete.");
                Console.WriteLine("Go back.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the number of the machine to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Machine machineToDelete = machines.Find(m => m.Id == id);
            machineToDelete.DeleteMachine(machineToDelete);

            Console.WriteLine("Machine deleted successfully.");
            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
        }

        public static void CreateMachine()
        {
            Console.Clear();
            Console.WriteLine("Adding a new machine:");

            Console.Write("Enter machine name: ");
            string name = Console.ReadLine();
            

            Machine newMachine = new Machine(1, name);
            newMachine.CreateMachine(newMachine);
            Console.WriteLine("Machine Created Successfully!");

            Console.WriteLine("Go back");
            Console.ReadKey();

            AdministratorMainMenu();
        }
        
        public static void DeleteLocation()
        {
            Console.Clear();
            Console.WriteLine("Deleting a location:");

            Console.WriteLine("Select the location to delete:");

            List<Location> locations = Location.ReadLocations();
            foreach (Location location in locations)
            {
                Console.WriteLine($"Location: {location.Id} - {location.Name}");
            }

            if (locations.Count == 0)
            {
                Console.WriteLine("No locations available to delete.");
                Console.WriteLine("Go back");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the number of the location to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Location locationToDelete = locations.Find(l => l.Id == id);
            locationToDelete.DeleteLocation(locationToDelete);

            Console.WriteLine("Location deleted successfully.");
            Console.WriteLine("Go back");

            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
        }
        
        public static void CreateLocation()
        {
            Console.Clear();
            Console.WriteLine("Adding a new location:");

            Console.Write("Enter location name: ");
            string name = Console.ReadLine();
            Console.Write("Enter street name: ");
            string streetName = Console.ReadLine();
            Console.Write("Enter house number: ");
            string houseNumber = Console.ReadLine();
            Console.Write("Enter postal code: ");
            string postalCode = Console.ReadLine();

            Location newLocation = new Location(1, name, streetName, houseNumber, postalCode, []);
            newLocation.CreateLocation(newLocation);
            Console.WriteLine("Location Created Successfully!");

            Console.WriteLine("Go back");
            Console.ReadKey();

            AdministratorMainMenu();
        }
        
        public static void AdministratorReadMachineLocation()
        {
            Console.Clear();
            List<Location> locations = Location.ReadMachineLocations();
            Console.WriteLine("Machine locations:");

            Console.WriteLine("");
            foreach (Location location in locations)
            {
                Console.WriteLine($"Location: {location.Id} - {location.Name}");
                foreach (Machine machine in location.Machines)
                {
                    Console.WriteLine($"  Machine: {machine.Id} - {machine.Name}");
                }
            }
            Console.WriteLine("");


            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
        }
        
        public static void ReadMachines()
        {
            Console.Clear();
            List<Machine> machines = Machine.ReadMachines();
            Console.WriteLine("Machines:");

            Console.WriteLine("");
            foreach (Machine machine in machines)
            {
                Console.WriteLine($"Machine ID = {machine.Id} Name = {machine.Name}");
            }
            Console.WriteLine("");

            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
        }

        public static void ReadLocations()
        {
            Console.Clear();
            List<Location> locations = Location.ReadLocations();
            Console.WriteLine("Locations:");

            Console.WriteLine("");
            foreach (Location location in locations)
            { 
                Console.WriteLine($"Location ID = {location.Id} Name = {location.Name} Street name = {location.StreetName} House number = {location.HouseNumber} Postal code = {location.PostalCode}");
            }
            Console.WriteLine("");

            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }

        }

        public static void AdministratorViewCoaches()
        {

            Console.Clear();
            Console.WriteLine("Coaches:");

            List<Person> persons = Person.GetPersons();

            Console.WriteLine("");
            foreach (Person person in persons)
            {
                if (person is Coach)
                {
                    Console.WriteLine($"Coach: {person.Id} - {person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine("");


            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
        }

        public static void AdministratorCreateCoach()
        {
            Console.Clear();
            Console.WriteLine("Adding a new coach:");

            Console.WriteLine("");

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

            Console.WriteLine("");

            List<Feedback> feedback = new List<Feedback>();

            Person newCoach = new Coach(1, firstName, lastName, streetName, houseNumber, postalCode, feedback);
            newCoach.CreatePerson();

            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }
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
                Console.WriteLine("Go back");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter the number of the coach to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Person personToUpdate = persons.Find(p => p.Id == id);
            personToUpdate.DeletePerson();

            List<string> options = new List<string>
            {
                "Go back",
            };
            int choice = DisplayMenuOptions(options, "", null, false);
            if (choice == 1)
            {
                AdministratorMainMenu();
            }

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
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                personToUpdate.UpdatePerson();
            }
            AdministratorMainMenu();
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
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                return choice;
            }
            catch
            {
                Console.WriteLine("Invalid choice. Please try again.");
                return DisplayMenuOptions(options, title, workout, clearConsole);
            }
        }
    }
}




