using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zuydfit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zuydfit.DataAccessLayer
{
    public class DAL
    {
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
            string productQuery = "SELECT " +
                "workout.id, " +
                "workout.date," +
                "exercise.id," +
                "exercise.name," +
                "exercise.duration," +
                "exercise.MachineId," +
                "exercise.type," +
                "sets.id," +
                "sets.amount," +
                "sets.weight " +
                "FROM workout " +
                "JOIN exercise ON workout.id = exercise.WorkoutId " +
                "LEFT JOIN ExerciseSet on exercise.id = ExerciseSet.ExerciseId " +
                "LEFT JOIN Sets on ExerciseSet.SetId = Sets.id";
            using SqlCommand command = new(productQuery, connection);
            using SqlDataReader reader = command.ExecuteReader();

            Workout previousWorkout = new Workout(0, new DateTime());
            Exercise previousExercise = new Strength(0, "");
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader[0]);
                if (previousWorkout.Id != id)
                {
                    DateTime date = Convert.ToDateTime(reader[1]);
                    Workout workout = new Workout(id, date);
                    previousWorkout = workout;
                    Workouts.Add(workout);
                }

                int exerciseId = Convert.ToInt32(reader[2]);
                string name = reader[3].ToString();
                string type = reader[6].ToString();

                if (type.ToLower() == "strength")
                {
                    if (previousExercise.Id != exerciseId)
                    {
                        Strength strengthExercise = new Strength(exerciseId, name, []);
                        previousWorkout.Exercises.Add(strengthExercise);
                        previousExercise = strengthExercise;
                    }

                    if (reader[7] != DBNull.Value)
                    {
                        int setsId = Convert.ToInt32(reader[7]);
                        int amount = Convert.ToInt32(reader[8]);
                        int weight = Convert.ToInt32(reader[9]);
                        Sets set = new Sets(setsId, amount, weight);

                        Strength previousStrengthExercise = previousExercise as Strength;
                        previousStrengthExercise.Sets.Add(set);
                    }
                }
                else if (type.ToLower() == "cardio")
                {
                    string duration = Convert.ToString(reader[4]);
                    string distance = Convert.ToString(reader[5]);
                    if (previousExercise.Id != exerciseId)
                    {
                        Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
                        previousWorkout.Exercises.Add(cardioExercise);
                        previousExercise = cardioExercise;
                    }
                }
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

        public List<Person> GetPerson()
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
                                Person person = new Athlete(personid, firstname, lastname, streetname, housenumber, postalcode, locationid, workoutid);
                                persons.Add(person);
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

        public Activity CreateActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Activity (Name, Duration) VALUES (@Name, @Duration); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Duration", activity.Duration); // Sla de duur op als totaal aantal seconden
                    int activityId = Convert.ToInt32(command.ExecuteScalar());
                    activity.Id = activityId;
                    return activity;
                }
            }
        }

        public Activity ReadActivity(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Duration FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string duration = reader["Duration"].ToString();
                            Activity activity = new Activity(id, name, duration, new List<Athlete>());
                            return activity;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public Activity UpdateActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Activity SET Name = @Name, Duration = @Duration WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Duration", activity.Duration);
                    command.Parameters.AddWithValue("@Id", activity.Id);
                    command.ExecuteNonQuery();
                    return activity;
                }
            }
        }

        public void DeleteActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", activity.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Sets CreateSet(Sets sets)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO [Sets] (Reps, Weight) VALUES (@Reps, @Weight); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Reps", sets.Reps);
                    command.Parameters.AddWithValue("@Weight", sets.Weight);
                    int setId = Convert.ToInt32(command.ExecuteScalar());

                    // Update de id van het sets-object
                    sets.Id = setId;

                    return sets;
                }
            }
        }

        public Sets ReadSet(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Reps, Weight FROM [Sets] WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int reps = Convert.ToInt32(reader["Reps"]);
                            double weight = Convert.ToDouble(reader["Weight"]);
                            Sets sets = new Sets(id, reps, weight);
                            return sets;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public Sets UpdateSet(Sets sets)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE [Sets] SET Reps = @Reps, Weight = @Weight WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Reps", sets.Reps);
                    command.Parameters.AddWithValue("@Weight", sets.Weight);
                    command.Parameters.AddWithValue("@Id", sets.Id);
                    command.ExecuteNonQuery();
                    return sets;
                }
            }
        }

        public bool DeleteSet(int setsId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM [Sets] WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", setsId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}