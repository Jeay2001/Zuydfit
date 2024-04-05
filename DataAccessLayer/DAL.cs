using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zuydfit.DataAccessLayer
{
    public class DAL
    {


        //private string connectionString = "Server=sqlserverjeaysnijders.database.windows.net;Database=Zuydfit;User Id=Jeay2001;Password=Snijders2208@;";

        //private readonly string connectionString = "Data Source=.;Initial Catalog=KassaSysteem;Integrated Security=true";
        //private readonly string connectionString = "Server=sqlserverjeaysnijders.database.windows.net;Database=Zuydfit;User Id=Jeay2001;Password=Snijders2208@;";
        private readonly string connectionString = "Data Source=sqlserverjeaysnijders.database.windows.net; Initial Catalog = Zuydfit; User ID = Jeay2001; Password=Snijders2208@";

        public List<Workout> Workouts { get; set; } = new List<Workout>();




        public Workout CreateWorkout(Workout workout)
        {
            // To do - create workout in DataBase
            return workout;
        }

        public List<Workout> ReadWorkouts()
        {

            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "SELECT Id, Date FROM Workout";
            using SqlCommand command = new(productQuery, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                int id = Convert.ToInt32(reader[0]);
                //DateOnly date = reader[1];
                //int id = Convert.ToInt32(reader[2]);
                //string name = reader[1].ToString();
                //double price = Convert.ToDouble(reader[2]);
                //string description = reader[3].ToString();
                //string type = reader[4].ToString();

                //string length = reader[5].ToString();
                //string width = reader[6].ToString();
                //string height = reader[7].ToString();

                Console.WriteLine($"Id: {id}");
                //Console.WriteLine($"Data: {date}");
                //Product product = new Item(id, name, price, description, length, width, height);
                //Products.Add(product);
            }
            return Workouts;
        }


        public Workout ReadWorkout(Workout workout)
        {
            // To do - read workout from DataBase
            return workout;
        }
        
        public Workout UpdateWorkout(Workout workout)
        {
            // To do - update workout in DataBase
            return workout;
        }

        public bool DeleteWorkout(Workout workout)
        {
            // To do - delete workout in DataBase
            return true;
        }
    }
}
