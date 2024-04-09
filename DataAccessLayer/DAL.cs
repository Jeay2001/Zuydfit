using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zuydfit.DataAccessLayer
{
    public class DAL
    {

        private readonly string connectionString = "Data Source=sqlserverjeaysnijders.database.windows.net; Initial Catalog = Zuydfit; User ID = Jeay2001; Password=Snijders2208@";

        public List<Workout> Workouts { get; set; } = new List<Workout>();


        public Workout CreateWorkout(Workout workout)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Workout (Date) VALUES (@Date); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", workout.Date);
                    int workoutId = Convert.ToInt32(command.ExecuteScalar());
                    workout.Id = workoutId;
                }
            }
            return workout;
        }

        public List<Workout> ReadWorkouts()
        {

            //using SqlConnection connection = new(connectionString);
            //connection.Open();
            //string productQuery = "SELECT " +
            //    "workout.id, " +
            //    "workout.date," +
            //    "exercise.id," +
            //    "exercise.name," +
            //    "exercise.duration," +
            //    "exercise.MachineId," +
            //    "exercise.type," +
            //    "sets.id," +
            //    "sets.amount," +
            //    "sets.weight " +
            //    "FROM workout " +
            //    "JOIN exercise ON workout.id = exercise.WorkoutId " +
            //    "LEFT JOIN ExerciseSet on exercise.id = ExerciseSet.ExerciseId " +
            //    "LEFT JOIN Sets on ExerciseSet.SetId = Sets.id";
            //using SqlCommand command = new(productQuery, connection);
            //using SqlDataReader reader = command.ExecuteReader();

            //Workout previousWorkout = new Workout(0, new DateTime());
            //Exercise previousExercise = new Strength(0, "");
            //while (reader.Read())
            //{
            //    int id = Convert.ToInt32(reader[0]);
            //    if (previousWorkout.Id != id)
            //    {
            //        DateTime date = Convert.ToDateTime(reader[1]);
            //        Workout workout = new Workout(id, date);
            //        previousWorkout = workout;
            //        Workouts.Add(workout);
            //    }

            //    int exerciseId = Convert.ToInt32(reader[2]);
            //    string name = reader[3].ToString();
            //    string type = reader[6].ToString();

            //    if (type.ToLower() == "strength")
            //    {
            //        if (previousExercise.Id != exerciseId)
            //        {
            //            Strength strengthExercise = new Strength(exerciseId, name, []);
            //            previousWorkout.Exercises.Add(strengthExercise);
            //            previousExercise = strengthExercise;
            //        }

            //        if (reader[7] != DBNull.Value)
            //        {
            //            int setId = Convert.ToInt32(reader[7]);
            //            int amount = Convert.ToInt32(reader[8]);
            //            int weight = Convert.ToInt32(reader[9]);
            //            Set set = new Set(setId, amount, weight);
                    
            //            Strength previousStrengthExercise = previousExercise as Strength;
            //            previousStrengthExercise.Sets.Add(set);

            //        }
            //    } else if (type.ToLower() == "cardio")
            //    {
            //        string duration = Convert.ToString(reader[4]);
            //        string distance = Convert.ToString(reader[5]);
            //        if (previousExercise.Id != exerciseId)
            //        {
            //            Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
            //            previousWorkout.Exercises.Add(cardioExercise);
            //            previousExercise = cardioExercise;
            //        }
            //    }   

            //}
            return Workouts;
        }


        public Workout ReadWorkout(Workout workout)
        {
            // To do - read workout from DataBase
            return workout;
        }


        public List<Exercise> ReadExerciseListFromAthlete(Athlete athlete)
        {
            List<Exercise> exercises = new List<Exercise>();

            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "select exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Amount  from personWorkout\r\ninner join workout on workout.id = personWorkout.workoutid\r\ninner join ExerciseWorkout on ExerciseWorkout.workoutid = workout.id\r\ninner join exercise on exercise.id = exerciseWorkout.ExerciseID\r\ninner join exerciseSet on exerciseSet.setId = setid\r\ninner join sets on exerciseset.SetId = sets.Id\r\nwhere personWorkout.personid = @id";
            using SqlCommand command = new(productQuery, connection);
            command.Parameters.AddWithValue("@Id", athlete.Id);
            command.ExecuteNonQuery();

            using SqlDataReader reader = command.ExecuteReader();

            Exercise previousExercise = new Strength(0, "");
            while (reader.Read())
            {
                int exerciseId = Convert.ToInt32(reader[0]);
                string name = reader[1].ToString();
                string type = reader[2].ToString();




                
                if (type.ToLower() == "strength")
                {
                    if (previousExercise.Id != exerciseId)
                    {
                        Strength strengthExercise = new Strength(exerciseId, name, []);
                        exercises.Add(strengthExercise);
                        previousExercise = strengthExercise;
                    }
                    if (reader[5] != DBNull.Value)
                    {
                        int machineId = Convert.ToInt32(reader[5]);
                    }

                    if (reader[7] != DBNull.Value)
                    {
                        int setId = Convert.ToInt32(reader[6]);
                        int weight = Convert.ToInt32(reader[7]);
                        int amount = Convert.ToInt32(reader[8]);
                        Set set = new Set(setId, amount, weight);

                        Strength previousStrengthExercise = previousExercise as Strength;
                        previousStrengthExercise.Sets.Add(set);

                    }
                }
                else if (type.ToLower() == "cardio")
                {
                    string duration = reader[3].ToString();
                    string distance = reader[4].ToString();
                    if (previousExercise.Id != exerciseId)
                    {
                        Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
                        exercises.Add(cardioExercise);
                        previousExercise = cardioExercise;
                    }
                }

            }

            return exercises;
        }

        public Workout UpdateWorkout(Workout workout)
        {

            //Console.WriteLine("Update workout");
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    string editProductQuery = "UPDATE workout SET id = @Id, price = @price, Description = @description, Length = @length, Width = @width, Height = @height, Duration = @duration, Type = @type WHERE Id = @id;";
            //    using (SqlCommand command = new SqlCommand(editProductQuery, connection))
            //    {
            //        command.Parameters.AddWithValue("@id", product.Id);
            //        command.Parameters.AddWithValue("@name", product.Name);
            //        command.Parameters.AddWithValue("@price", product.Price);
            //        command.Parameters.AddWithValue("@description", product.Description);
            //        command.Parameters.AddWithValue("@productId", product.Id);
            //        if (product is Item)
            //        {
            //            Item item = product as Item;
            //            command.Parameters.AddWithValue("@type", "Item");
            //            command.Parameters.AddWithValue("@width", item.Lengte);
            //            command.Parameters.AddWithValue("@height", item.Breedte);
            //            command.Parameters.AddWithValue("@length", item.Lengte);
            //            command.Parameters.AddWithValue("@duration", "");
            //        }
            //        else if (product is Service)
            //        {
            //            Service service = product as Service;
            //            command.Parameters.AddWithValue("@type", "Service");
            //            command.Parameters.AddWithValue("@width", "");
            //            command.Parameters.AddWithValue("@height", "");
            //            command.Parameters.AddWithValue("@length", "");
            //            command.Parameters.AddWithValue("@duration", service.Duration);
            //        }
            //        command.ExecuteNonQuery();
            //        Console.WriteLine("edited product");
            //        Console.WriteLine(product.Id);
            //    }
            //}
            //Products = RetrieveProducts();
            //return Products;
            return workout;
        }

        

        public bool DeleteWorkout(Workout workout)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE from workout where id = (@Id);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", workout.Id);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"Deleted row with id {workout.Id}");
                return true;
            } catch
            {
                return false;
            }

        }

        public  List<Person> GetPerson()
        {
            List<Person> persons = new List<Person>();

            using (SqlConnection connection = new SqlConnection(connectionString)) 
            { 
                connection.Open();

                string productQuery = "SELECT * FROM Person";

                using (SqlCommand command = new SqlCommand(productQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int personid = Convert.ToInt32(reader[0]);
                            string firstname = reader[1].ToString();
                            string lastname = reader[2].ToString();
                            string streetname = reader[3].ToString();
                            string housenumber = reader[4].ToString();
                            string postalcode = reader[5].ToString();
                            string type = reader[6].ToString();

                            if (type == "Athlete")
                            {
                                int locationid = Convert.ToInt32(reader[7]);
                                int workoutid = Convert.ToInt32(reader[8]);
                                //Person person = new Athlete(personid, firstname, lastname, streetname, housenumber, postalcode, locationid, workoutid);
                                //persons.Add(person);
                            }
                            else if (type == "Coach")
                            {
                                Person person = new Coach(personid, firstname, lastname, streetname, housenumber, postalcode);
                                persons.Add(person);
                            }
                        }
                    }
                }
            }

                return persons;

        }
    }
}
