using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CMAS_App.CMAS_App;

namespace CMAS_App
{
    public class DAL
    {
        private string connectionString = "Server=localhost;Database=CMAS;User ID=sa;Password=Lunesu21;";

       

        


        public List<Exercise> GetExercises()
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT ExerciseID, Name, Description, VideoLink FROM Exercises", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Exercise exercise = new Exercise
                            {
                                ExerciseID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                VideoLink = reader.GetString(3)
                            };
                            exercises.Add(exercise);
                        }
                    }
                }
            }

            return exercises;
        }



        public List<CMASMeasurement> GetMeasurementsByPatientID(int patientId)
        {
            List<CMASMeasurement> measurements = new List<CMASMeasurement>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT MeasurementID, Date, Feedback, PatientID FROM CMASMeasurements WHERE PatientID = @PatientID", connection))
                {
                    command.Parameters.AddWithValue("@PatientID", patientId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CMASMeasurement measurement = new CMASMeasurement
                            {
                                MeasurementID = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Feedback = reader.IsDBNull(2) ? null : reader.GetString(2),
                                PatientID = reader.GetInt32(3)
                            };
                            measurements.Add(measurement);
                        }
                    }
                }
            }

            return measurements;
        }


        public CMASMeasurement GetMeasurementById(int measurementId)
        {
            CMASMeasurement measurement = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT MeasurementID, Date, Feedback, PatientID FROM CMASMeasurements WHERE MeasurementID = @MeasurementID", connection))
                {
                    command.Parameters.AddWithValue("@MeasurementID", measurementId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            measurement = new CMASMeasurement
                            {
                                MeasurementID = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Feedback = reader.IsDBNull(2) ? null : reader.GetString(2),
                                PatientID = reader.GetInt32(3)
                            };
                        }
                    }
                }
            }

            return measurement;
        }


        public void UpdateFeedback(int measurementId, string feedback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE CMASMeasurements SET Feedback = @Feedback WHERE MeasurementID = @MeasurementID", connection))
                {
                    command.Parameters.AddWithValue("@Feedback", feedback);
                    command.Parameters.AddWithValue("@MeasurementID", measurementId);
                    command.ExecuteNonQuery();
                }
            }
        }







        public User GetUserByName(string name)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserID, Name, Age, Gender, Role FROM Users WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Age = reader.GetInt32(2),
                                Gender = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return user;
        }

        public List<Patient> GetPatientsByDoctorID(int doctorId)
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT p.PatientID, p.UserID, u.Name, u.Age, u.Gender FROM DoctorPatient dp JOIN Patients p ON dp.PatientID = p.PatientID JOIN Users u ON p.UserID = u.UserID WHERE dp.DoctorID = @DoctorID", connection))
                {
                    command.Parameters.AddWithValue("@DoctorID", doctorId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Patient patient = new Patient
                            {
                                PatientID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Age = reader.GetInt32(3),
                                Gender = reader.GetString(4)
                            };
                            patients.Add(patient);
                        }
                    }
                }
            }

            return patients;
        }

      

        public void AddExerciseResult(ExerciseResult result)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO ExerciseResults (Score, Duration, Comments, ExerciseID, MeasurementID) VALUES (@Score, @Duration, @Comments, @ExerciseID, @MeasurementID)", connection))
                {
                    command.Parameters.AddWithValue("@Score", result.Score);
                    command.Parameters.AddWithValue("@Duration", result.Duration);
                    command.Parameters.AddWithValue("@Comments", result.Comments);
                    command.Parameters.AddWithValue("@ExerciseID", result.ExerciseID);
                    command.Parameters.AddWithValue("@MeasurementID", result.MeasurementID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddMeasurement(CMASMeasurement measurement)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verify that the PatientID exists
                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Patients WHERE PatientID = @PatientID", connection))
                {
                    checkCommand.Parameters.AddWithValue("@PatientID", measurement.PatientID);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count == 0)
                    {
                        throw new Exception($"PatientID {measurement.PatientID} does not exist.");
                    }
                }

                // Insert the measurement
                using (SqlCommand command = new SqlCommand("INSERT INTO CMASMeasurements (Date, Feedback, PatientID) OUTPUT INSERTED.MeasurementID VALUES (@Date, @Feedback, @PatientID)", connection))
                {
                    command.Parameters.AddWithValue("@Date", measurement.Date);
                    command.Parameters.AddWithValue("@Feedback", string.IsNullOrEmpty(measurement.Feedback) ? (object)DBNull.Value : measurement.Feedback);
                    command.Parameters.AddWithValue("@PatientID", measurement.PatientID);

                    measurement.MeasurementID = (int)command.ExecuteScalar(); // Get the new MeasurementID
                }
            }
        }


        public List<CMASMeasurement> GetAllMeasurements()
        {
            List<CMASMeasurement> measurements = new List<CMASMeasurement>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT MeasurementID, Date, Feedback, PatientID FROM CMASMeasurements", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CMASMeasurement measurement = new CMASMeasurement
                            {
                                MeasurementID = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Feedback = reader.IsDBNull(2) ? null : reader.GetString(2),
                                PatientID = reader.GetInt32(3)
                            };
                            measurements.Add(measurement);
                        }
                    }
                }
            }

            return measurements;
        }

        public void AddFeedback(int measurementId, string feedback)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE CMASMeasurements SET Feedback = @Feedback WHERE MeasurementID = @MeasurementID", connection))
                {
                    command.Parameters.AddWithValue("@Feedback", feedback);
                    command.Parameters.AddWithValue("@MeasurementID", measurementId);
                    command.ExecuteNonQuery();
                }
            }
        }



        // for detailed measurements!
      public List<(string, CMASMeasurement, List<ExerciseResult>)> GetDetailedMeasurementById(int measurementId)
{
    var measurementDetails = new List<(string, CMASMeasurement, List<ExerciseResult>)>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand(@"
            SELECT 
                u.Name AS PatientName,
                m.MeasurementID,
                m.Date,
                m.Feedback,
                m.PatientID,
                e.ExerciseID,
                e.Name AS ExerciseName,
                r.Score,
                r.Duration,
                r.Comments
            FROM 
                CMASMeasurements m
            JOIN 
                Patients p ON m.PatientID = p.PatientID
            JOIN 
                Users u ON p.UserID = u.UserID
            LEFT JOIN 
                ExerciseResults r ON m.MeasurementID = r.MeasurementID
            LEFT JOIN 
                Exercises e ON r.ExerciseID = e.ExerciseID
            WHERE 
                m.MeasurementID = @MeasurementID", connection))
        {
            command.Parameters.AddWithValue("@MeasurementID", measurementId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                string patientName = null;
                CMASMeasurement measurement = null;
                List<ExerciseResult> exerciseResults = new List<ExerciseResult>();

                while (reader.Read())
                {
                    if (measurement == null)
                    {
                        patientName = reader.GetString(0);
                        measurement = new CMASMeasurement
                        {
                            MeasurementID = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            Feedback = reader.IsDBNull(3) ? null : reader.GetString(3),
                            PatientID = reader.GetInt32(4)
                        };
                    }

                    if (!reader.IsDBNull(5))
                    {
                        var exerciseResult = new ExerciseResult
                        {
                            Exercise = new Exercise
                            {
                                ExerciseID = reader.GetInt32(5),
                                Name = reader.GetString(6)
                            },
                            Score = reader.GetInt32(7),
                            Duration = (float)reader.GetDouble(8), // Corrected this line
                            Comments = reader.IsDBNull(9) ? null : reader.GetString(9)
                        };
                        exerciseResults.Add(exerciseResult);
                    }
                }

                if (measurement != null)
                {
                    measurementDetails.Add((patientName, measurement, exerciseResults));
                }
            }
        }
    }

    return measurementDetails;
}



// method to get patients linked to guardion
public List<Patient> GetPatientsByGuardianId(int userId)
{
    List<Patient> patients = new List<Patient>();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(
            "SELECT p.PatientID, u.Name, u.Age, u.Gender FROM Patients p " +
            "INNER JOIN Users u ON p.UserID = u.UserID " +
            "INNER JOIN GuardianPatients gp ON p.PatientID = gp.PatientID " +
            "WHERE gp.GuardianID = (SELECT GuardianID FROM Guardians WHERE UserID = @UserID)", connection))
        {
            command.Parameters.AddWithValue("@UserID", userId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Patient patient = new Patient
                    {
                        PatientID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Gender = reader.GetString(3)
                    };
                    patients.Add(patient);
                }
            }
        }
    }
    return patients;
}



public int GetGuardianIdByUserId(int userId)
{
    int guardianId = -1;
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(
            "SELECT GuardianID FROM Guardians WHERE UserID = @UserID", connection))
        {
            command.Parameters.AddWithValue("@UserID", userId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    guardianId = reader.GetInt32(0);
                }
            }
        }
    }
    return guardianId;
}





    }
}
