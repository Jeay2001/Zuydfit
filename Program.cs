using Zuydfit;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<Workout> workouts = Workout.ReadWorkouts();
            foreach (Workout workout in workouts)
            {
                //Console.WriteLine($"Workout ID: {workout.Id}");
                //Console.WriteLine($"Date: {workout.Date}");
                //Console.WriteLine("Exercises:");
                foreach (Exercise exercise in workout.Exercises)
                {
                    //Console.WriteLine($"Exercise Name: {exercise.Name}");
                    if (exercise is Cardio)
                    {
                        Cardio cardio = (Cardio)exercise;
                        //Console.WriteLine($"Duration: {cardio.Duration} minutes");
                        //Console.WriteLine($"Distance: {cardio.Distance} miles");
                    }
                    else if (exercise is Strength)
                    {
                        Strength strength = (Strength)exercise;
                        foreach (Set set in strength.Sets)
                        {
                            //Console.WriteLine($"Reps: {set.Reps}");
                            //Console.WriteLine($"Weight: {set.Weight} lbs");
                        }
                    }
                }
                Console.WriteLine();
            }

            List<Person> persons = Person.GetPersons();
            foreach (Person person in persons)
            {
                Console.WriteLine($"Person ID: {person.Id}");
                Console.WriteLine($"Name: {person.FirstName} {person.LastName}");
                Console.WriteLine($"Address: {person.StreetName} {person.HouseNumber}, {person.PostalCode}");
                if (person is Athlete)
                {
                    Athlete athlete = (Athlete)person;
                    Console.WriteLine("Workouts:");
                    foreach (Workout workout in athlete.Workouts)
                    {
                        Console.WriteLine($"Workout ID: {workout.Id}");
                        Console.WriteLine($"Date: {workout.Date}");
                        Console.WriteLine("Exercises:");
                        foreach (Exercise exercise in workout.Exercises)
                        {
                            Console.WriteLine($"Exercise Name: {exercise.Name}");
                            if (exercise is Cardio)
                            {
                                Cardio cardio = (Cardio)exercise;
                                Console.WriteLine($"Duration: {cardio.Duration} minutes");
                                Console.WriteLine($"Distance: {cardio.Distance} miles");
                            }
                            else if (exercise is Strength)
                            {
                                Strength strength = (Strength)exercise;
                                foreach (Set set in strength.Sets)
                                {
                                    Console.WriteLine($"Reps: {set.Reps}");
                                    Console.WriteLine($"Weight: {set.Weight} lbs");
                                }
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
   
   
}
