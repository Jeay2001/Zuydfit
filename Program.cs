using Zuydfit;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<Workout> workouts = Workout.ReadWorkouts();
            foreach(Workout workout in workouts)
            {
                Console.WriteLine(workout.Id);
                Console.WriteLine(workout.Date);
                foreach (Exercise exercise in workout.Exercises)
                {
                    Console.WriteLine(exercise.Name);
                    if (exercise is Cardio)
                    {
                        Cardio cardio = (Cardio)exercise;
                        Console.WriteLine(cardio.Duration);
                    }
                    else if (exercise is Strength)
                    {
                        Strength strength = (Strength)exercise;
                        foreach (Set set in strength.Sets)
                        {
                            Console.WriteLine(set.Reps);
                            Console.WriteLine(set.Weight);
                        }
                    }
                }
            }

            List<Person> persons = Person.GetPersons();
            foreach(Person person in persons)
            {
                Console.WriteLine(person.Id);
                Console.WriteLine(person.FirstName);
                Console.WriteLine(person.LastName);
                Console.WriteLine(person.StreetName);
                Console.WriteLine(person.HouseNumber);
                Console.WriteLine(person.PostalCode);
                if (person is Athlete)
                {
                    Athlete athlete = (Athlete)person;
                    foreach (Workout workout in athlete.Workouts)
                    {
                        Console.WriteLine(workout.Id);
                        Console.WriteLine(workout.Date);
                        foreach (Exercise exercise in workout.Exercises)
                        {
                            Console.WriteLine(exercise.Name);
                            if (exercise is Cardio)
                            {
                                Cardio cardio = (Cardio)exercise;
                                Console.WriteLine(cardio.Duration);
                                Console.WriteLine(cardio.Distance);
                            }
                            else if (exercise is Strength)
                            {
                                Strength strength = (Strength)exercise;
                                foreach (Set set in strength.Sets)
                                {
                                    Console.WriteLine(set.Reps);
                                    Console.WriteLine(set.Weight);
                                }
                            }
                        }
                    }
                }
            }


        }
    }
   
   
}
