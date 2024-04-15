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

            //List<Workout> workouts = Workout.ReadWorkouts();
            //PrintWorkouts(workouts);

            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Feedback> feedbacks = new List<Feedback>();
            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location, feedbacks);
            Administrator administrator = new Administrator(1, "karel", "kerel", "hebikniet", "66", "9999", []);
            Coach coach = new Coach(1, "zuch", "mabaulz", "zweetweg", "69", "4200", []);


            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            PrintWorkouts(workouts);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Ingelogd als coach");
                Console.WriteLine("");
                CoachMenu(coach);
                flag = false;
            }
        }

        static void AdministratorMenu(Administrator administrator)
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
                    ViewCoaches(administrator);
                    break;
                case 2:
                    AddCoach(administrator);
                    break;
                case 3:
                    DeleteCoach(administrator);
                    break;
                case 4:
                    UpdateCoach(administrator);
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

        public static void UpdateCoach(Administrator administrator)
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

        public static int DisplayMenuOptions(List<string> options, string title = "", Workout workout = null, bool clearConsole = true)
        {
            if (clearConsole)
            {
                //Console.Clear();
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
                    AthleteProgression(athlete);
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
                Exercise newExercise = CreateExercise(workout);
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
                "To do - Edit exercise",
                "Delete workout",
                "Go back",
                "Main menu",
            ];

            int choice = DisplayMenuOptions(options, "Single workout", workout);

            if (choice == 1)
            {
                // Add exercise
                Exercise newExercise = CreateExercise(workout);
                workout.Exercises.Add(newExercise);
                AthleteSingleWorkout(athlete, workout);
            }
            else if (choice == 2)
            {
                // Remove exercise
                List<Exercise> exercises = RemoveExerciseFromList(workout.Exercises);
                workout.Exercises = exercises;
                AthleteSingleWorkout(athlete, workout);
            }
            else if (choice == 3)
            {
                // Edit exercise
                //List<Exercise> updatedExercises = EditExercises();
            }
            else if (choice == 4)
            {
                // Delete workout
                // To do - delete workout
                //AthleteViewWorkouts(athlete);
                workout.DeleteWorkout();
                AthleteViewWorkouts(athlete);
            }
            else if (choice == 5)
            {
                // Previous menu
                AthleteViewWorkouts(athlete);
            }
            else if (choice == 6)
            {
                // Main menu
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static Exercise CreateExercise(Workout workout)
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
                        //Sets newSet = set.CreateSet();
                        //newSet = newSet.CreateSet();
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
                Cardio newExercise = new Cardio(0, "test", "10 min", "500m");
                return newExercise;
            }
            // To do - exercise type controleren
            return new Strength(0, "test");
        }

        public static List<Exercise> RemoveExerciseFromList(List<Exercise> exercises)
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
                Exercise exerciseToRemove = exercises[choice -2];
                exerciseToRemove.DeleteExercise();
                exercises.RemoveAt(choice - 2);
                return exercises;
            }
        }

        //public static void AthleteWorkoutAddExercise(Workout workout, Athlete athlete)
        //{
        //    Console.Write("Exercise name: ");
        //    string exerciseName = Console.ReadLine();
        //    Console.Write("Exercise type (strength or cardio): ");
        //    string exerciseType = Console.ReadLine();
        //    List<Sets> sets = new List<Sets>();
        //    if (exerciseType == "strength")
        //    {
        //        bool addSetsFlag = true;
        //        while (addSetsFlag)
        //        {
        //            Console.WriteLine("Add set? (y/n)");
        //            string newSetBool = Console.ReadLine();
        //            if (newSetBool == "y")
        //            {
        //                Console.Write("Weight: (in Kg)");
        //                double weight = Convert.ToDouble(Console.ReadLine());
        //                Console.Write("Reps: ");
        //                int reps = Convert.ToInt32(Console.ReadLine());
        //                Sets set = new Sets(0, reps, weight);
        //                Sets newSet = set.CreateSet();
        //                newSet = newSet.CreateSet();
        //                sets.Add(newSet);
        //            }
        //            else
        //            {
        //                addSetsFlag = false;
        //            }
        //        }

        //        Strength newExercise = new Strength(0, exerciseName, sets);
        //        newExercise.CreateExercise(workout, newExercise);
        //        workout.Exercises.Add(newExercise);
        //        AthleteCreateWorkout(athlete, workout);
        //    }
        //    else if (exerciseType == "cardio")
        //    {
        //        // To do cardio
        //    }
        //}

        //public static void AthleteCreateExercise(Workout workout, Athlete athlete)
        //{
        //    Console.Write("Exercise name: ");
        //    string exerciseName = Console.ReadLine();
        //    Console.Write("Exercise type (strength or cardio): ");
        //    string exerciseType = Console.ReadLine();
        //    List<Sets> sets = new List<Sets>();
        //    if (exerciseType == "strength")
        //    {
        //        bool addSetsFlag = true;
        //        while (addSetsFlag) {
        //            Console.WriteLine("Add set? (y/n)");
        //            string newSetBool = Console.ReadLine();
        //            if (newSetBool == "y")
        //            {
        //                Console.Write("Weight: (in Kg)");
        //                double weight = Convert.ToDouble(Console.ReadLine());
        //                Console.Write("Reps: ");
        //                int reps = Convert.ToInt32(Console.ReadLine());
        //                Sets set = new Sets(0, reps, weight);
        //                Sets newSet = set.CreateSet();
        //                newSet = newSet.CreateSet();
        //                sets.Add(newSet);
        //            }
        //            else
        //            {
        //                addSetsFlag = false;
        //            }
        //        }

        //        Strength newExercise = new Strength(0, exerciseName, sets);
        //        newExercise.CreateExercise(workout, newExercise);
        //        workout.Exercises.Add(newExercise);
        //        AthleteCreateWorkout(athlete, workout);
        //    } else if (exerciseType == "cardio")
        //    {
        //        // To do cardio
        //    }
        //}

        // static void AthleteWorkoutRemoveExercise(Workout workout, Athlete athlete)
        // {
        //    List<string> options = [
        //        "Go back",
        //        "Main menu",
        //    ];
        //    foreach (Exercise exercise in workout.Exercises)
        //    {
        //        options.Add(exercise.Name);
        //    }
        //    int choice = DisplayMenuOptions(options, "Choose wich exercise you'd like to remove");

        //    if (choice == 1)
        //    {
        //        AthleteCreateWorkout(athlete, workout);
        //    }
        //    if (choice == 2)
        //    {
        //        AthleteMainMenu(athlete);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}

        //public static void AthleteWorkoutEditExercise(Workout workout, Athlete athlete)
        //{
        //    List<string> options = [
        //        //"Add set",
        //        //"Remove exercise",
        //        //"Edit exercise",
        //        "Go back",
        //        "Main menu",
        //    ];
        //    int choice = DisplayMenuOptions(options, "Edit workout exercise");

        //    if (choice == 1)
        //    {
        //        //AthleteWorkoutAddExercise(workout, athlete);
        //    }
        //    //else if (choice == 2)
        //    //{
        //    //    AthleteWorkoutRemoveExercise(workout, athlete);
        //    //}
        //    else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}

        //public static void AthleteSingleWorkoutMenu(Workout workout, Athlete athlete)
        //{
        //    List<string> options = [
        //        "To do - Add exercise",
        //        "Edit exercise",
        //        "To do - Remove exercise",
        //        "Go back",
        //        "Main menu",
        //    ];
        //    int choice = DisplayMenuOptions(options, "View workout", workout);

        //    if (choice == 1)
        //    {
        //    }
        //    else if (choice== 2)
        //    {
        //        AthleteWorkoutEditExercise(workout, athlete);
        //    }
        //    else if (choice == 4)
        //    {
        //        AthleteViewWorkouts(athlete);
        //    }
        //    else if (choice == 5)
        //    {
        //        AthleteMainMenu(athlete);

        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}


        //public static void AthleteViewWorkout(Workout workout, Athlete athlete)
        //{

        //    //Console.Clear();
        //    PrintWorkout(workout);
        //    Console.WriteLine("");
        //    Console.WriteLine("1. Go back");
        //    Console.WriteLine("2. Main menu");

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        // Previous menu
        //        AthleteSingleWorkoutMenu(workout, athlete);
        //    }
        //    else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu(athlete);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid choice");
        //    }
        //}

        //public static void AthleteEditWorkout(Workout workout, Athlete athlete)
        //{
        //    //Console.Clear();
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
        //                foreach (Sets set in strength.Sets)
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
        //        AthleteSingleWorkoutMenu(workout, athlete);
        //    }
        //    else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu(athlete);
        //    }
        //    else
        //    {
        //        Exercise exercise = workout.Exercises[choice - 3];
        //        AthleteEditExercise(workout, exercise, athlete);
        //    }
        //}


        //public static void AthleteEditExercise(Workout workout, Exercise exercise, Athlete athlete)
        //{
        //    //Console.Clear();

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
        //        foreach (Sets set in strength.Sets)
        //        {
        //            Console.WriteLine($"{index} - Weight: {set.Weight} Reps: {set.Reps}");
        //            index++;
        //        }
        //    }

        //    Console.WriteLine("");
        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    if (choice == 1)
        //    {
        //        // Previous menu
        //        AthleteEditWorkout(workout, athlete);
        //    }
        //    else if (choice == 2)
        //    {
        //        // Main menu
        //        AthleteMainMenu(athlete);
        //    }
        //    else
        //    {
        //        if (exercise is Strength)
        //        {
        //            Strength strength = (Strength)exercise;
        //            AthleteEditSet(workout, exercise, strength.Sets[choice - 5], athlete);
        //        }
        //        else if (exercise is Cardio)
        //        {
        //            // To do - Edit cardio
        //        }
        //    }
        //}

        //public static void AthleteEditSet(Workout workout, Exercise exercise, Sets set, Athlete athlete)
        //{
        //    //Console.Clear();

        //    Console.WriteLine($"Editing {exercise.Name}");
        //    Console.WriteLine("");

        //    Console.Write("Weight: ");
        //    string weight = Console.ReadLine();
        //    Console.Write("Reps: ");
        //    int reps = Convert.ToInt32(Console.ReadLine());

        //    // To do - Update set
        //    AthleteEditExercise(workout, exercise, athlete);

        //}





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
        // keuze menu coach
        //
        //
        //
        //
        static void CoachMenu(Coach coach)
        {
            bool continueMenu = true;
            while (continueMenu)
            {
                Console.Clear();
                List<string> options = new List<string> {
            "Add Athlete",
            "Show Athlete Progression",
            "Add Activity for athlete",
            "Give Athlete Feedback",
            "Read Athlete Feedback",
            "Exit"
        };
                int choice = DisplayMenuOptions(options, "Coach Menu");

                switch (choice)
                {
                    case 1:
                        Createperson();
                        break;
                    case 2:
                        AthleteProgression();
                        break;
                    case 3:
                        CreateActivity();
                        break;
                    case 4:
                        CreateFeedback();
                        break;
                    case 5:
                        ReadAllFeedback();
                        break;
                    case 6: 
                        continueMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void Createperson()
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

            // Hier kun je verdere inputvalidatie toevoegen, zoals het controleren of de ingevoerde gegevens geldig zijn

            List<Feedback> feedback = new List<Feedback>();
            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            // Maak een nieuwe person met de ingevoerde gegevens
            Person newPerson = new Athlete(1, firstName, lastName, streetName, houseNumber, postalCode, location, feedback);
            newPerson.CreatePerson();
            Console.WriteLine("Athlete added succesfully!");
        }

        public static void AthleteProgression()
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

        public static void CreateActivity()
        {
            Console.Clear();
            Console.WriteLine("Adding a new activity:");


            // Vraag de gebruiker om de gegevens van de nieuwe activiteit in te voeren
            Console.Write("Enter activity name: ");
            string name = Console.ReadLine();
            Console.Write("Enter activity duration: ");
            string duration = Console.ReadLine();

            List<Activity> activities = new List<Activity>();
            List<Athlete> athlete = new List<Athlete>();
            Activity newActivity = new Activity(1, name, duration , athlete);
            newActivity.CreateActivity();
            Console.WriteLine("Activity Created Succesfully!");

        }
        public static void CreateFeedback()
        {
            Console.Clear();
            Console.WriteLine("Adding a new feedback:");

            // Vraag de gebruiker om de gegevens van de nieuwe feedback in te voeren
            Console.Write("Enter feedback message: ");
            string message = Console.ReadLine();
            
            

            List<Feedback> feedbacks = new List<Feedback>();
            Feedback newFeedback = new Feedback(1, message, DateTime.Now);
            newFeedback.CreateFeedback();
            Console.WriteLine("Feedback Created Succesfully!");

        }

        public static void ReadAllFeedback()
        {
            List<Feedback> feedbacks = Feedback.ReadAllFeedback();
            foreach (Feedback feedback in feedbacks)
            {
                Console.WriteLine($"Feedback: {feedback.Id} - {feedback.FeedbackMessage} - {feedback.Date}");
            }
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }
    }
}




