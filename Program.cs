using FitnessApp;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            Set set1 = new Set(1, 1, 100);
            Set set2 = new Set(1, 4, 90);
            Set set3 = new Set(1, 8, 80);

            List<Set> sets = new List<Set> { set1, set2, set3 };

            Exercise exercise1 = new Cardio(1, "Running", 30, 5);
            Exercise exercise2 = new Strength(2, "Bench Press");
            Exercise exercise3 = new Strength(3, "Lat Pulldown", [set1, set2, set3]);

            List<Exercise> exercises = new List<Exercise> { exercise1, exercise2, exercise3 };


            foreach (Exercise exercise in exercises)
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
