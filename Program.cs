using System;
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
            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location);



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

            Workout testWorkout = new Workout(1, DateTime.Now);
            Console.WriteLine(" Test workout");
            testWorkout = testWorkout.ReadWorkout(testWorkout);
            PrintWorkout(testWorkout);

        }


        public static void AthleteMainMenu(Athlete athlete)
        {
            Console.Clear();
            Console.WriteLine("Choose one of the options:");
            Console.WriteLine("1. View workouts");
            Console.WriteLine("2. New workout");
            Console.WriteLine("3. My progression");
            Console.WriteLine("4. View feedback");

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    AthleteViewWorkouts(athlete);
                    break;
                case 2:
                    AthleteCreateWorkout(athlete);
                    Console.WriteLine("New workout");
                    break;
                case 3:
                    Console.WriteLine("My progression");
                    break;
                case 4:
                    Console.WriteLine("View instructor feedback");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

        public static void AthleteViewWorkouts(Athlete athlete)
        {
            Console.Clear();
            Console.WriteLine("Choose a workout to view/edit:");
            Console.WriteLine("");
            Console.WriteLine("1. Go back");
            List<Workout> workouts = Workout.ReadWorkouts(athlete);
            int index = 2;
            foreach (Workout workout in workouts)
            {
                Console.WriteLine($"{index}. {workout.Date.ToString("dd/MM/yyyy")}");
                index++;
            }

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                AthleteMainMenu(athlete);
            }
            else
            {
                Workout workout = workouts[choice - 2];
                AthleteSingleWorkoutMenu(workout, athlete);
            }
        }

        
        public static void AthleteCreateWorkout(Athlete athlete)
        {
            Console.Clear();
            //Console.WriteLine("Choose a workout to view/edit:");
            //Console.WriteLine("");
            //Console.WriteLine("1. Go back");
            //Console.WriteLine("2. Add strength exercise");
            //Console.WriteLine("2. Add cardio exercise");
            //Console.WriteLine("3. Remove exercise");
            //List<Workout> workouts = Workout.ReadWorkouts(athlete);
            //int index = 2;
            //foreach (Workout workout in workouts)
            //{
            //    Console.WriteLine($"{index}. {workout.Date.ToString("dd/MM/yyyy")}");
            //    index++;
            //}

            //Console.WriteLine("");
            //int choice = Convert.ToInt32(Console.ReadLine());
            //if (choice == 1)
            //{
            //    AthleteMainMenu(athlete);
            //}
            //else
            //{
            //    Workout workout = workouts[choice - 2];
            //    AthleteSingleWorkoutMenu(workout, athlete);
            //}
        }

        public static void AthleteSingleWorkoutMenu(Workout workout, Athlete athlete)
        {
            Console.Clear();
            Console.WriteLine("1. View workout");
            Console.WriteLine("2. Edit workout");
            Console.WriteLine("3. Delete workout");
            Console.WriteLine("4. Go back");
            Console.WriteLine("5. Main menu");

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                AthleteViewWorkout(workout, athlete);
            }
            if (choice == 2)
            {
                AthleteEditWorkout(workout, athlete);
            }
            else if (choice == 3)
            {
                //workout.DeleteWorkout();
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


        public static void AthleteViewWorkout(Workout workout, Athlete athlete)
        {

            Console.Clear();
            PrintWorkout(workout);
            Console.WriteLine("");
            Console.WriteLine("1. Go back");
            Console.WriteLine("2. Main menu");

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                // Previous menu
                AthleteSingleWorkoutMenu(workout, athlete);
            }
            else if (choice == 2)
            {
                // Main menu
                AthleteMainMenu(athlete);
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }

        public static void AthleteEditWorkout(Workout workout, Athlete athlete)
        {
            Console.Clear();
            //PrintWorkout(workout);
            Console.WriteLine("1. Go back");
            Console.WriteLine("2. Main menu");
            int index = 3;
            foreach (Exercise exercise in workout.Exercises)
            {
                Console.WriteLine($"{index}. {exercise.Name}");
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
                index++;
            }

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                // Previous menu
                AthleteSingleWorkoutMenu(workout, athlete);
            }
            else if (choice == 2)
            {
                // Main menu
                AthleteMainMenu(athlete);
            }
            else
            {
                Exercise exercise = workout.Exercises[choice - 3];
                AthleteEditExercise(workout, exercise, athlete);
            }
        }


        public static void AthleteEditExercise(Workout workout, Exercise exercise, Athlete athlete)
        {
            Console.Clear();

            Console.WriteLine($"Edit {exercise.Name}");
            Console.WriteLine("");
            Console.WriteLine("1. Go back");
            Console.WriteLine("2. Main menu");
            Console.WriteLine("3. Add set");
            Console.WriteLine("4. Remove set");
            if (exercise is Cardio)
            {
                // To do - Show cardio exercise
            }
            else if (exercise is Strength)
            {
                Strength strength = (Strength)exercise;
                int index = 5;
                foreach (Sets set in strength.Sets)
                {
                    Console.WriteLine($"{index} - Weight: {set.Weight} Reps: {set.Reps}");
                    index++;
                }
            }

            Console.WriteLine("");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                // Previous menu
                AthleteEditWorkout(workout, athlete);
            }
            else if (choice == 2)
            {
                // Main menu
                AthleteMainMenu(athlete);
            }
            else
            {
                if (exercise is Strength)
                {
                    Strength strength = (Strength)exercise;
                    AthleteEditSet(workout, exercise, strength.Sets[choice - 5], athlete);
                }
                else if (exercise is Cardio)
                {
                    // To do - Edit cardio
                }
            }
        }

        public static void AthleteEditSet(Workout workout, Exercise exercise, Sets set, Athlete athlete)
        {
            Console.Clear();

            Console.WriteLine($"Editing {exercise.Name}");
            Console.WriteLine("");

            Console.Write("Weight: ");
            string weight = Console.ReadLine();
            Console.Write("Reps: ");
            int reps = Convert.ToInt32(Console.ReadLine());

            // To do - Update set
            AthleteEditExercise(workout, exercise, athlete);

        }





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


        //List<Person> persons = Person.GetPersons();
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
        //Console.WriteLine(cardio.Duration);
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





