using Zuydfit;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Test");

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
