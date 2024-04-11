﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
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

        public List<Workout> ReadWorkouts(Athlete athlete)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "select workout.id, workout.date, exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Amount  from personWorkout inner join workout on workout.id = personWorkout.workoutid inner join ExerciseWorkout on ExerciseWorkout.workoutid = workout.id inner join exercise on exercise.id = exerciseWorkout.ExerciseID inner join exerciseSet on exerciseSet.setId = setid inner join sets on exerciseset.SetId = sets.Id " +
                "where personWorkout.personid = @id";
            using SqlCommand command = new(productQuery, connection);
            using SqlDataReader reader = command.ExecuteReader();
            command.Parameters.AddWithValue("@Id", athlete.Id);
            command.ExecuteNonQuery();


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


        public List<Exercise> ReadExerciseListFromAthlete(Athlete athlete)
        {
            List<Exercise> exercises = new List<Exercise>();

            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "select exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Amount  from personWorkout\r\ninner join workout on workout.id = personWorkout.workoutid\r\ninner join ExerciseWorkout on ExerciseWorkout.workoutid = workout.id\r\ninner join exercise on exercise.id = exerciseWorkout.ExerciseID\r\ninner join exerciseSet on exerciseSet.setId = setid\r\ninner join sets on exerciseset.SetId = sets.Id " +
                "where personWorkout.personid = @id";
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
                        Sets set = new Sets(setId, amount, weight);
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
        

        
        public List<Machine> Machines { get; set; } = new List<Machine>();
        public List<Machine> ReadMachines()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name FROM Machine"; 
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Machines.Add(new Machine(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            return Machines;
        }

        public Machine ReadMachine(int machineId)
        {
            Machine machine = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name FROM Machine WHERE Id = @machineId";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@machineId", machineId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        machine = new Machine(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }

            return machine;
        }

        public Machine CreateMachine(Machine machine)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Machine (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", machine.Name);

                // Uitvoeren van het commando en ophalen van de nieuwe ID
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    machine.Id = Convert.ToInt32(result);
                }
            }

            return machine;
        }

        public Machine UpdateMachine(Machine machine)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE Machine SET Name = @Name WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", machine.Id);
                command.Parameters.AddWithValue("@Name", machine.Name);

                command.ExecuteNonQuery();
            }

            return machine;
        }

        public bool DeleteMachine(Machine machine)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Machine WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", machine.Id);

                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }


        public List<Location> ReadLocations()
        {
            List<Location> locations = new List<Location>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name, Streetname, Housenumber, Postalcode FROM Location"; // Verwijderd WHERE Id = @locationId
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add(new Location(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        ));
                    }
                }
            }

            return locations;
        }

        public Location ReadLocation(Location location)
        {
            Location newLocation = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Location (Name, Streetname, Housenumber, Postalcode) VALUES (@Name, @Streetname, @Housenumber, @Postalcode); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", location.Name);
                command.Parameters.AddWithValue("@Streetname", location.StreetName);
                command.Parameters.AddWithValue("@Housenumber", location.HouseNumber);
                command.Parameters.AddWithValue("@Postalcode", location.PostalCode);
                command.Parameters.AddWithValue("@locationId", location);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        newLocation = new Location(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                    }
                }
            }

            return newLocation;
        }

        public Location CreateLocation(Location location)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Zorg ervoor dat de SQL-instructie alle benodigde kolommen bevat.
                string sql = @"
                    INSERT INTO Location (Name, StreetName, HouseNumber, PostalCode) 
                    VALUES (@Name, @StreetName, @HouseNumber, @PostalCode); 
                    SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", location.Name);
                command.Parameters.AddWithValue("@StreetName", location.StreetName ?? (object)DBNull.Value); // Voorbeeld voor als StreetName NULL mag zijn.
                command.Parameters.AddWithValue("@HouseNumber", location.HouseNumber ?? (object)DBNull.Value); // Idem voor HouseNumber.
                command.Parameters.AddWithValue("@PostalCode", location.PostalCode ?? (object)DBNull.Value); // Idem voor PostalCode.

                int newId = Convert.ToInt32(command.ExecuteScalar());
                location.Id = newId;
            }

            return location;
        }
        public bool DeleteLocation(int locationId)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlDeleteRelated = "DELETE FROM Person WHERE LocationId = @LocationId";
                SqlCommand commandDeleteRelated = new SqlCommand(sqlDeleteRelated, connection);
                commandDeleteRelated.Parameters.AddWithValue("@LocationId", locationId);

                // Verwijder eerst alle gerelateerde personen
                commandDeleteRelated.ExecuteNonQuery();

                // Probeer daarna de locatie te verwijderen
                string sqlDeleteLocation = "DELETE FROM Location WHERE Id = @Id";
                SqlCommand commandDeleteLocation = new SqlCommand(sqlDeleteLocation, connection);
                commandDeleteLocation.Parameters.AddWithValue("@Id", locationId);

                rowsAffected = commandDeleteLocation.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Get a list of persons from the database.
        /// </summary>
        /// <returns></returns>
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
                                Location location = new Location(locationid,"Zuyd Heerlen","Straatnaam","10","1212ab");

                                Person person = new Athlete(personid, firstname, lastname, streetname, housenumber, postalcode, location);
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

        /// <summary>
        /// Create a person in the database.
        /// </summary>
        public void CreatePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Person (Firstname, Lastname, Streetname, Housenumber, Postalcode, Type, LocationId) " +
                    "VALUES (@Firstname, @Lastname, @Streetname, @Housenumber, @Postalcode, @Type, @LocationId) SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Firstname", person.FirstName);
                    command.Parameters.AddWithValue("@Lastname", person.LastName);
                    command.Parameters.AddWithValue("@Streetname", person.StreetName);
                    command.Parameters.AddWithValue("@Housenumber", person.HouseNumber);
                    command.Parameters.AddWithValue("@Postalcode", person.PostalCode);

                    //indicate in the program whether it is a coach or an athlete
                    if (person is Athlete athlete)
                    {
                        command.Parameters.AddWithValue("@LocationId", athlete.Location.Id);
                        command.Parameters.AddWithValue("@Type", "Athlete");
                    }
                    else if (person is Coach)
                    {
                        command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                        command.Parameters.AddWithValue("@Type", "Coach");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid person type.");
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update a person in the database.
        /// </summary>
        /// <param name="person"></param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdatePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = "UPDATE Person SET Firstname = @Firstname, Lastname = @Lastname, Streetname = @Streetname, Housenumber = @Housenumber, Postalcode = @Postalcode, Type = @Type, LocationId = @locationId WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", person.FirstName);
                        command.Parameters.AddWithValue("@Lastname", person.LastName);
                        command.Parameters.AddWithValue("@Streetname", person.StreetName);
                        command.Parameters.AddWithValue("@Housenumber", person.HouseNumber);
                        command.Parameters.AddWithValue("@Postalcode", person.PostalCode);
                        if (person is Athlete athlete)
                        {
                            command.Parameters.AddWithValue("@LocationId", athlete.Location.Id);
                            command.Parameters.AddWithValue("@Type", "Athlete");
                        }
                        else if (person is Coach)
                        {
                            command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                            command.Parameters.AddWithValue("@Type", "Coach");
                        }
                        else
                        {
                            throw new ArgumentException("Invalid person type.");
                        }
                        command.Parameters.AddWithValue("@Id", person.Id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while updating the product: " + ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a person from the database.
        /// </summary>
        public void DeletePerson(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated orderlines first
                    string DeletePersonquery = "DELETE FROM Person WHERE Id = @id";
                    using (SqlCommand deletepersoncommand = new SqlCommand(DeletePersonquery, connection))
                    {
                        deletepersoncommand.Parameters.AddWithValue("@id", person.Id);
                        deletepersoncommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the product: " + ex.Message);
                throw;
            }
        }

        public Location UpdateLocation(Location location)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", location.Id);
                    command.ExecuteNonQuery();
                }
                return location;
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