using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Zuydfit;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            //List<Workout> workouts = Workout.ReadWorkouts();
            //PrintWorkouts(workouts);

            //bool flag = true;
            //while(flag)
            //{
            //    Console.WriteLine("Ingelogd als atleet");
            //    Console.WriteLine("");

            //AthleteMainMenu();

            //flag = false;

            //}

            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            Athlete athlete = new Athlete(1, "John", "Doe", "Street", "1", "1234", [], location);



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





