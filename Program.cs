﻿using System;
using System.Collections.Generic;
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
            Console.WriteLine("Hello, World!");

            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Feedback> feedbacks = new List<Feedback>();
            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location, feedbacks);


            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            PrintWorkouts(workouts);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Ingelogd als atleet");
                Console.WriteLine("");
                AthleteMainMenu(athlete);
                flag = false;
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

    }
}





