using Zuydfit;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            //List<Workout> workouts = Workout.ReadWorkouts();
            //foreach(Workout workout in workouts)
            //{
            //    Console.WriteLine(workout.Id);
            //    Console.WriteLine(workout.Date);
            //    foreach (Exercise exercise in workout.Exercises)
            //    {
            //        Console.WriteLine(exercise.Name);
            //        if (exercise is Cardio)
            //        {
            //            Cardio cardio = (Cardio)exercise;
            //            Console.WriteLine(cardio.Duration);
            //        }
            //        else if (exercise is Strength)
            //        {
            //            Strength strength = (Strength)exercise;
            //            foreach (Set set in strength.Sets)
            //            {
            //                Console.WriteLine(set.Reps);
            //                Console.WriteLine(set.Weight);
            //            }
            //        }
            //    }
            //}

            //List<Person> persons = Person.GetPersons();
            //foreach(Person person in persons)
            //{
            //    Console.WriteLine(person.Id);
            //    Console.WriteLine(person.FirstName);
            //    Console.WriteLine(person.LastName);
            //    Console.WriteLine(person.StreetName);
            //    Console.WriteLine(person.HouseNumber);
            //    Console.WriteLine(person.PostalCode);
            //    if (person is Athlete)
            //    {
            //        Athlete athlete = (Athlete)person;
            //        foreach (Workout workout in athlete.Workouts)
            //        {
            //            Console.WriteLine(workout.Id);
            //            Console.WriteLine(workout.Date);
            //            foreach (Exercise exercise in workout.Exercises)
            //            {
            //                Console.WriteLine(exercise.Name);
            //                if (exercise is Cardio)
            //                {
            //                    Cardio cardio = (Cardio)exercise;
            //                    Console.WriteLine(cardio.Duration);
            //                    Console.WriteLine(cardio.Distance);
            //                }
            //                else if (exercise is Strength)
            //                {
            //                    Strength strength = (Strength)exercise;
            //                    foreach (Set set in strength.Sets)
            //                    {
            //                        Console.WriteLine(set.Reps);
            //                        Console.WriteLine(set.Weight);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            // Vraag de gebruiker om activiteitsgegevens in te voeren
            Console.WriteLine("Voer de naam van de activiteit in:");
            string name = Console.ReadLine();

            Console.WriteLine("Voer de duur van de activiteit in (HH:MM:SS formaat):");
            string duration = Console.ReadLine();

            // Maak een nieuwe activiteit aan met ingevoerde gegevens
            Activity activity = new Activity(name, duration, null);

            // Roep de CreateActivity methode van de DAL klasse aan om de activiteit toe te voegen aan de database
            DAL dal = new DAL();
            Activity createdActivity = dal.CreateActivity(activity);

            // Controleer of de activiteit succesvol is toegevoegd aan de database
            if (createdActivity != null && createdActivity.Id > 0)
            {
                Console.WriteLine("Activiteit succesvol toegevoegd aan de database.");
                Console.WriteLine($"Activiteit ID: {createdActivity.Id}");
            }
            else
            {
                Console.WriteLine("Er is een fout opgetreden bij het toevoegen van de activiteit aan de database.");
            }
        }
    }
}


