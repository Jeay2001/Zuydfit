using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Zuydfit.DataAccessLayer
{
    public class DAL
    {
        //connection string voor azure database: (werkt alleen op internet van zuyd hogeschool heerlen)
        private readonly string connectionString = "Data Source=sqlserverjeaysnijders.database.windows.net; Initial Catalog = Zuydfit; User ID = Jeay2001; Password=Snijders2208@";

        //connection string voor lokale database:
        //private readonly string connectionString = "Data Source=.;Initial Catalog=Kassasysteem;Integrated Security=true";

        /* Exercise */
        public Exercise CreateExercise(Workout workout, Exercise exercise)
        {
            Console.WriteLine("Creating exercise in database");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "insert into exercise (name, type, distance, duration) values (@name, @type, @distance, @duration); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", exercise.Name);

                        if (exercise is Strength)
                        {
                            Strength strengthExercise = exercise as Strength;
                            command.Parameters.AddWithValue("@type", "Strength");
                            command.Parameters.AddWithValue("@duration", DBNull.Value);
                            command.Parameters.AddWithValue("@distance", DBNull.Value);
                        }
                        else if (exercise is Cardio)
                        {
                            Cardio cardioExercise = exercise as Cardio;
                            command.Parameters.AddWithValue("@type", "Cardio");
                            command.Parameters.AddWithValue("@duration", cardioExercise.Duration);
                            command.Parameters.AddWithValue("@distance", cardioExercise.Distance);
                        }

                        int exerciseId = Convert.ToInt32(command.ExecuteScalar());
                        exercise.Id = exerciseId;

                        if (exercise is Strength)
                        {
                            Strength strengthExercise = exercise as Strength;
                            foreach (Sets set in strengthExercise.Sets)
                            {
                                string setQuery = "INSERT INTO Sets (Reps, Weight) VALUES (@Reps, @Weight); SELECT SCOPE_IDENTITY();";
                                using (SqlCommand setCommand = new SqlCommand(setQuery, connection))
                                {
                                    setCommand.Parameters.AddWithValue("@Reps", set.Reps);
                                    setCommand.Parameters.AddWithValue("@Weight", set.Weight);
                                    int setId = Convert.ToInt32(setCommand.ExecuteScalar());
                                    set.Id = setId;

                                    string exerciseSetQuery = "INSERT INTO ExerciseSet (setId, exerciseId) VALUES (@setId, @exerciseId);";
                                    using (SqlCommand exerciseSetCommand = new SqlCommand(exerciseSetQuery, connection))
                                    {
                                        exerciseSetCommand.Parameters.AddWithValue("@setId", set.Id);
                                        exerciseSetCommand.Parameters.AddWithValue("@exerciseId", exercise.Id);
                                        exerciseSetCommand.ExecuteNonQuery();
                                        Console.WriteLine("inserted into exerciseSet");
                                    }
                                }
                            }
                        }

                        string personWorkoutQuery = "INSERT INTO ExerciseWorkout(exerciseId, workoutId) VALUES(@exerciseId, @workoutId);";
                        using (SqlCommand personWorkoutCommand = new SqlCommand(personWorkoutQuery, connection))
                        {
                            personWorkoutCommand.Parameters.AddWithValue("@exerciseId", exercise.Id);
                            personWorkoutCommand.Parameters.AddWithValue("@workoutId", workout.Id);
                            personWorkoutCommand.ExecuteNonQuery();
                            Console.WriteLine("inserted into exerciseWorkout");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating the exercise: " + ex.Message);
            }
            return exercise;
        }

        public bool DeleteExercise(Exercise exercise)
        {
            Console.WriteLine("Removing exercise");
            Console.WriteLine(exercise.Id);
            Console.WriteLine(exercise.Name);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Exercise WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", exercise.Id);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the exercise: " + ex.Message);
                return false;
            }
        }

        public List<Exercise> ReadExerciseListFromAthlete(Athlete athlete)
        {
            List<Exercise> exercises = new List<Exercise>();

            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "select exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Reps  from personWorkout\r\ninner join workout on workout.id = personWorkout.workoutid\r\ninner join ExerciseWorkout on ExerciseWorkout.workoutid = workout.id\r\ninner join exercise on exercise.id = exerciseWorkout.ExerciseID\r\ninner join exerciseSet on exerciseSet.setId = setid\r\ninner join sets on exerciseset.SetId = sets.Id " +
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

        /* Workout */
        public List<Workout> Workouts { get; set; } = new List<Workout>();

        public Workout CreateWorkout(Workout workout, Athlete athlete)
        {
            Console.WriteLine("Create workout in DAL");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Workout (Date) VALUES (@Date); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", workout.Date);
                    int workoutId = Convert.ToInt32(command.ExecuteScalar());
                    workout.Id = workoutId;

                    string personWorkoutQuery = "INSERT INTO PersonWorkout(personId, workoutId) VALUES(@personId, @workoutId);";
                    using (SqlCommand personWorkoutCommand = new SqlCommand(personWorkoutQuery, connection))
                    {
                        personWorkoutCommand.Parameters.AddWithValue("@personId", athlete.Id);
                        personWorkoutCommand.Parameters.AddWithValue("@workoutId", workout.Id);
                        personWorkoutCommand.ExecuteScalar();
                        Console.WriteLine("inserted into personworkout");

                    }
                }
            }
            return workout;
        }

        public List<Workout> ReadWorkouts(Athlete athlete)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            string workoutsQuery = "select workout.id, workout.date, exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.Id, sets.Weight, sets.Reps from PersonWorkout " +
                "full join workout on workout.id = workoutid " +
                "full join ExerciseWorkout on workout.id = ExerciseWorkout.workoutid " +
                "full join exercise on ExerciseWorkout.ExerciseID = exercise.id " +
                "full join exerciseSet on exerciseSet.exerciseId = exercise.id " +
                "full join sets on exerciseSet.setId = sets.Id " +
                "where personid = @id";
            using SqlCommand command = new(workoutsQuery, connection);
            command.Parameters.AddWithValue("@Id", athlete.Id);
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

                if (reader[2] != DBNull.Value)
                {
                    int exerciseId = Convert.ToInt32(reader[2]);
                    string name = reader[3].ToString();
                    string type = reader[4].ToString();

                    if (type.ToLower() == "strength")
                    {
                        if (previousExercise.Id != exerciseId)
                        {
                            Strength strengthExercise = new Strength(exerciseId, name, []);
                            previousWorkout.Exercises.Add(strengthExercise);
                            previousExercise = strengthExercise;
                        }

                        if (reader[8] != DBNull.Value)
                        {

                            int setId = Convert.ToInt32(reader[8]);
                            int amount = Convert.ToInt32(reader[9]);
                            int weight = Convert.ToInt32(reader[10]);
                            Sets set = new Sets(setId, amount, weight);
                            Strength previousStrengthExercise = previousExercise as Strength;
                            previousStrengthExercise.Sets.Add(set);
                        }
                    }
                    else if (type.ToLower() == "cardio")
                    {
                        string duration = Convert.ToString(reader[5]);
                        string distance = Convert.ToString(reader[6]);
                        if (previousExercise.Id != exerciseId)
                        {
                            Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
                            previousWorkout.Exercises.Add(cardioExercise);
                            previousExercise = cardioExercise;
                        }
                    }
                }
            }

            return Workouts;
        }

        public Workout ReadWorkout(Workout workout)
        {

            using SqlConnection connection = new(connectionString);
            connection.Open();
            string productQuery = "select workout.id, workout.date, exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Reps from PersonWorkout " +
                "join workout on workout.id = workoutid " +
                "join ExerciseWorkout on workout.id = ExerciseWorkout.workoutid " +
                "join exercise on ExerciseWorkout.ExerciseID = exercise.id " +
                "full join exerciseSet on exerciseSet.exerciseId = exercise.id " +
                "full join sets on exerciseSet.setId = sets.Id " +
                "where workout.id = @id";

            using SqlCommand command = new(productQuery, connection);
            command.Parameters.AddWithValue("@Id", workout.Id);
            using SqlDataReader reader = command.ExecuteReader();

            Workout newWorkout = null;

            int previousExerciseId = 0;

            int index = 0;
            while (reader.Read())
            {
                if (index == 0)
                {
                    int id = Convert.ToInt32(reader[0]);
                    DateTime date = Convert.ToDateTime(reader[1]);
                    newWorkout = new Workout(id, date);
                    index++;
                }

                int exerciseId = Convert.ToInt32(reader[2]);
                string name = reader[3].ToString();
                string type = reader[4].ToString();


                if (type.ToLower() == "strength")
                {
                    if (previousExerciseId != exerciseId)
                    {
                        Strength strengthExercise = new Strength(exerciseId, name, []);
                        newWorkout.Exercises.Add(strengthExercise);
                    }

                    if (reader[8] != DBNull.Value)
                    {
                        int setsId = Convert.ToInt32(reader[8]);
                        int amount = Convert.ToInt32(reader[9]);
                        int weight = Convert.ToInt32(reader[10]);
                        Sets set = new Sets(setsId, amount, weight);

                        // Find current exercise and add set
                        Strength currentExercise = (Strength)newWorkout.Exercises.Find(exercise => exercise.Id == exerciseId);

                        if (currentExercise != null)
                        {
                            // Remove current exercise from workout and add it again with the new set
                            currentExercise.Sets.Add(set);
                            newWorkout.Exercises.Remove(currentExercise);
                            newWorkout.Exercises.Add(currentExercise);
                        }
                    }
                }
                else if (type.ToLower() == "cardio")
                {
                    string duration = Convert.ToString(reader[5]);
                    string distance = Convert.ToString(reader[6]);
                    if (previousExerciseId != exerciseId)
                    {
                        Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
                        newWorkout.Exercises.Add(cardioExercise);
                    }
                }
                previousExerciseId = exerciseId;

            }
            return newWorkout;
        }




        //public List<Exercise> ReadExerciseListFromAthlete(Athlete athlete)
        //{
        //    List<Exercise> exercises = new List<Exercise>();

        //    using SqlConnection connection = new(connectionString);
        //    connection.Open();
        //    string productQuery = "select exercise.Id, exercise.Name, exercise.Type, exercise.duration, exercise.Distance, exercise.MachineId, sets.id, sets.Weight, sets.Reps  from personWorkout\r\ninner join workout on workout.id = personWorkout.workoutid\r\ninner join ExerciseWorkout on ExerciseWorkout.workoutid = workout.id\r\ninner join exercise on exercise.id = exerciseWorkout.ExerciseID\r\ninner join exerciseSet on exerciseSet.setId = setid\r\ninner join sets on exerciseset.SetId = sets.Id " +
        //        "where personWorkout.personid = @id";
        //    using SqlCommand command = new(productQuery, connection);
        //    command.Parameters.AddWithValue("@Id", athlete.Id);
        //    command.ExecuteNonQuery();

        //    using SqlDataReader reader = command.ExecuteReader();
        //    Exercise previousExercise = new Strength(0, "");
        //    while (reader.Read())
        //    {
        //        int exerciseId = Convert.ToInt32(reader[0]);
        //        string name = reader[1].ToString();
        //        string type = reader[2].ToString();
        //        if (type.ToLower() == "strength")
        //        {
        //            if (previousExercise.Id != exerciseId)
        //            {
        //                Strength strengthExercise = new Strength(exerciseId, name, []);
        //                exercises.Add(strengthExercise);
        //                previousExercise = strengthExercise;
        //            }
        //            if (reader[5] != DBNull.Value)
        //            {
        //                int machineId = Convert.ToInt32(reader[5]);
        //            }
        //            if (reader[7] != DBNull.Value)
        //            {
        //                int setId = Convert.ToInt32(reader[6]);
        //                int weight = Convert.ToInt32(reader[7]);
        //                int amount = Convert.ToInt32(reader[8]);
        //                Sets set = new Sets(setId, amount, weight);
        //                Strength previousStrengthExercise = previousExercise as Strength;
        //                previousStrengthExercise.Sets.Add(set);
        //            }
        //        }
        //        else if (type.ToLower() == "cardio")
        //        {
        //            string duration = reader[3].ToString();
        //            string distance = reader[4].ToString();
        //            if (previousExercise.Id != exerciseId)
        //            {
        //                Cardio cardioExercise = new Cardio(exerciseId, name, duration, distance);
        //                exercises.Add(cardioExercise);
        //                previousExercise = cardioExercise;
        //            }
        //        }
        //    }
        //    return exercises;
        //}




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

        public Workout DuplicateWorkout(Workout workout, Athlete athlete)
        {
            Workout duplicatedWorkout = workout.CreateWorkout(workout, athlete);
            return duplicatedWorkout;

        }

        /* Activity */
        public Activity CreateActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Activity (Name, Duration) VALUES (@Name, @Duration); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Duration", activity.Duration);
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

        public List<Activity> ReadAllActivities()
        {
            List<Activity> activities = new List<Activity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Duration FROM Activity";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = reader["Name"].ToString();
                            string duration = reader["Duration"].ToString();
                            Activity activity = new Activity(id, name, duration, new List<Athlete>());
                            activities.Add(activity);
                        }
                    }
                }
            }

            return activities;
        }

        public List<Activity> ReadAthleteActivities(Athlete athlete)
        {
            List<Activity> activities = new List<Activity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try 
                {
                    connection.Open();
                    string query = "SELECT activity.Id, Name, Duration FROM Activity " +
                        "full join PersonActivity on Activity.Id = PersonActivity.ActivityId " +
                        "full join Person on PersonActivity.PersonId = Person.Id " +
                        "where person.id = @personId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@personId", athlete.Id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = Convert.ToInt32(reader[0]);
                                string name = reader["Name"].ToString();
                                string duration = reader["Duration"].ToString();
                                Activity activity = new Activity(id, name, duration, new List<Athlete>());
                                activities.Add(activity);
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("You are not signed in for a activity.");
                }
                
            }
            return activities;
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

        public void AddAthleteToActivity(Activity activity, Athlete athlete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO PersonActivity (PersonID, ActivityID) VALUES (@PersonID, @ActivityID)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", athlete.Id);
                        command.Parameters.AddWithValue("@ActivityID", activity.Id);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine("An error occurred while adding the athlete to the activity.");
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

        public List<Activity> ReadActivitieMembers()
        {
            Location location = new Location(1, "locatie 1", "straatnaam", "huisnummer", "1837jd", []);
            List<Activity> activities = new List<Activity>();
            List<Feedback> feedback = new List<Feedback>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT activity.Id, activity.Name, activity.Duration, person.Firstname, person.Lastname FROM Activity full " +
                    "join PersonActivity on Activity.Id = PersonActivity.ActivityId " +
                    "full join Person on PersonActivity.PersonId = Person.Id ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string firstName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            string lastName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                            {
                                Athlete athlete = new Athlete(1, firstName, lastName, "test", "test", "test", location, feedback);
                                if (!reader.IsDBNull(0))
                                {
                                    int activityId = reader.GetInt32(0);
                                    if (activities.Any(activity => activity.Id == activityId))
                                    {
                                        Activity activity = activities.Find(activity => activity.Id == activityId);
                                        activity.Athletes.Add(athlete);
                                    }
                                    else
                                    {
                                        Activity newActivity = new Activity(activityId, reader.GetString(1), reader.GetString(2), new List<Athlete>());
                                        newActivity.Athletes.Add(athlete);
                                        activities.Add(newActivity);
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            return activities;
        }
        
        /* Machines */
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

        /* Location */
        public Machine UpdateMachineLocation(Machine machine) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "UPDATE MachineLocation SET LocationId = @LocationId WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", machine.Id);
                command.Parameters.AddWithValue("@LocationId", machine.Location.Id);

                command.ExecuteNonQuery();
            }

            return machine;
        }

        public List<Location> ReadMachineLocations()
        {
            List<Location> locations = new List<Location>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Location " +
                    "Join MachineLocation on Location.Id = MachineLocation.LocationId " +
                    "Join Machine on MachineLocation.MachineId = Machine.Id";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Machine machine = new Machine(reader.GetInt32(7), reader.GetString(8));

                        if (locations.Any(location => location.Id == reader.GetInt32(0)))
                        {
                            Location location = locations.Find(location => location.Id == reader.GetInt32(0));
                            
                            location.Machines.Add(machine);
                        }
                        else
                        {
                            //locations.Add
                            Location newlocation = new Location(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        );
                            newlocation.Machines.Add(machine);
                            locations.Add(newlocation);
                        }
                        
                    }
                }
            }

            return locations;
        }

        public List<Location> ReadLocations()
        {
            List<Location> locations = new List<Location>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name, Streetname, Housenumber, Postalcode FROM Location"; 
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

        /* Person */
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
                                List<Feedback> feedbacks = new List<Feedback>();

                                Person person = new Athlete(personid, firstname, lastname, streetname, housenumber, postalcode, location, feedbacks);
                                persons.Add(person);
                            }
                            else if (type == "Coach")
                            {
                                List<Feedback> feedbacks = new List<Feedback>();
                                Person person = new Coach(personid, firstname, lastname, streetname, housenumber, postalcode, feedbacks);
                                persons.Add(person);
                            }
                            else if (type == "Administrator")
                            {
                                List<Location> locations = new List<Location>();
                                Person person = new Administrator(personid, firstname, lastname, streetname, housenumber, postalcode, locations);
                                persons.Add(person);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid person type.");
                            }
                        }
                    }
                }
            }

            return persons;
        }

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

                    if (person is Athlete athlete)
                    {
                        command.Parameters.AddWithValue("@LocationId", athlete.Location.Id);
                        command.Parameters.AddWithValue("@Type", "Athlete");
                    }
                    else if (person is Coach coach)
                    {
                        command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                        command.Parameters.AddWithValue("@Type", "Coach");
                    }
                    else if (person is Administrator administrator)
                    {
                        command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                        command.Parameters.AddWithValue("@Type", "Administrator");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid person type.");
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = "UPDATE Person SET Firstname = @Firstname, Lastname = @Lastname, Streetname = @Streetname, " +
                        "Housenumber = @Housenumber, Postalcode = @Postalcode, Type = @Type, LocationId = @locationId WHERE Id = @Id";
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
                        else if (person is Coach coach)
                        {
                            command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                            command.Parameters.AddWithValue("@Type", "Coach");
                        }
                        else if (person is Administrator administrator)
                        {
                            command.Parameters.AddWithValue("@LocationId", DBNull.Value);
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

        public bool DeletePerson(Person person)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete associated orderlines first
                    string deletePersonQuery = "DELETE FROM Person WHERE Id = @id";
                    using (SqlCommand deletePersonCommand = new SqlCommand(deletePersonQuery, connection))
                    {
                        deletePersonCommand.Parameters.AddWithValue("@id", person.Id);
                        int rowsAffected = deletePersonCommand.ExecuteNonQuery();

                        // Check if any rows were affected (person was deleted)
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the person: " + ex.Message);
                return false; // Return false to indicate deletion failure
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

        /* Sets */
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

        /* Feedback */
        public Feedback CreateFeedback(Feedback feedback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Feedback (FeedbackMessage, Date) VALUES (@FeedbackMessage, @Date); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FeedbackMessage", feedback.FeedbackMessage);
                    command.Parameters.AddWithValue("@Date", feedback.Date);
                    int feedbackId = Convert.ToInt32(command.ExecuteScalar());
                    feedback.Id = feedbackId;
                }
            }
            return feedback;
        }

        public Feedback CreatePersonFeedback(int personId, int feedbackId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO PersonFeedback (PersonID, FeedbackID) VALUES (@PersonID, @FeedbackID);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@FeedbackID", feedbackId);
                    command.ExecuteNonQuery();
                }
            }
            return null;
        }

        public List<Feedback> ReadFeedback(int Id)
        {
            try
            {
                List<Feedback> results = new List<Feedback>(); 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT feedback.Id, feedback.FeedbackMessage, feedback.Date FROM Feedback " +
                        "JOIN PersonFeedback ON PersonFeedback.FeedbackID = Feedback.Id " +
                        "JOIN Person ON Person.Id = PersonFeedback.PersonID WHERE Person.Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string message =  reader.GetString(1);
                                DateTime date = reader.GetDateTime(2);
                                Feedback feedback = new Feedback(id, message, date); 
                                results.Add(feedback); 
                            }
                        }
                    }
                    return results; 

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the feedback: " + ex.Message);
                return null;
            }
        }

        public List<Feedback> ReadAllFeedback()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FeedbackMessage, Date FROM Feedback";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Feedback feedback = new Feedback(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.IsDBNull(reader.GetOrdinal("FeedbackMessage")) ? null : reader.GetString(reader.GetOrdinal("FeedbackMessage")),
                                reader.GetDateTime(reader.GetOrdinal("Date"))
                            );
                            feedbacks.Add(feedback);
                        }
                    }
                }
            }
            return feedbacks;
        }

        public Feedback UpdateFeedback(Feedback feedback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Feedback SET FeedbackMessage = @FeedbackMessage, Date = @Date WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FeedbackMessage", feedback.FeedbackMessage);
                    command.Parameters.AddWithValue("@Date", feedback.Date);
                    command.Parameters.AddWithValue("@Id", feedback.Id);

                    command.ExecuteNonQuery();
                }
            }
            return feedback;
        }

        public bool DeleteFeedback(int feedbackId)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Feedback WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", feedbackId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected > 0;
        }
    }
}
