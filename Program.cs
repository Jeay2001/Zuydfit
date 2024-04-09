using Zuydfit;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Vraag de gebruiker om activiteitsgegevens in te voeren
            Console.WriteLine("Voer de naam van de activiteit in:");
            string name = Console.ReadLine();
            {
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


