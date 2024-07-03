using System;
using CMAS_App;

namespace CMAS_App
{
    using System;
    using System.Collections.Generic;

    namespace CMAS_App
    {
        public class Patient : User
        {
            public int PatientID { get; set; }
            public List<CMASMeasurement> Measurements { get; set; }

            // public void ViewProgress()
            // {
            //     Console.WriteLine("Viewing progress...");
            //     foreach (var measurement in Measurements)
            //     {
            //         Console.WriteLine($"Measurement ID: {measurement.MeasurementID}, Date: {measurement.Date}, Feedback: {measurement.Feedback}");
            //     }
            // }

            // public void ViewRewards()
            // {
            //     // Assume rewards are linked to measurements
            //     Console.WriteLine("Viewing rewards...");
            // }

            // public void ViewExercises(DAL dal)
            // {
            //     List<Exercise> exercises = dal.GetExercises();
            //     Console.WriteLine("Available exercises:");
            //     foreach (var exercise in exercises)
            //     {
            //         Console.WriteLine($"Exercise ID: {exercise.ExerciseID}, Name: {exercise.Name}, Description: {exercise.Description}, Video Link: {exercise.VideoLink}");
            //     }
            // }

            public void PerformExercise(Exercise exercise, DAL dal, int measurementID)
            {
                Console.WriteLine($"Performing exercise: {exercise.Name}");
                Console.WriteLine("How well did you perform the exercise? (Score 1–10)");
                int score = int.Parse(Console.ReadLine());
                Console.WriteLine("How long did it take? (in minutes)");
                float duration = float.Parse(Console.ReadLine());
                Console.WriteLine("Any comments?");
                string comments = Console.ReadLine();

                ExerciseResult exerciseResult = new ExerciseResult
                {
                    ExerciseID = exercise.ExerciseID,
                    Score = score,
                    Duration = duration,
                    Comments = comments,
                    MeasurementID = measurementID
                };

                dal.AddExerciseResult(exerciseResult);
            }


            public void PerformMeasurement(DAL dal)
            {
                Console.WriteLine("Starting new measurement...");

                CMASMeasurement measurement = new CMASMeasurement
                {
                    Date = DateTime.Now,
                    PatientID = this.PatientID
                };

                dal.AddMeasurement(measurement);

                List<Exercise> exercises = dal.GetExercises();

                foreach (var exercise in exercises)
                {
                    PerformExercise(exercise, dal, measurement.MeasurementID);
                }

                Console.WriteLine("Measurement completed.");
            }







            // public void RecordMeasurement(CMASMeasurement measurement, DAL dal)
            // {
            //     dal.AddMeasurement(measurement);
            // }

            public List<CMASMeasurement> ViewMeasurements(DAL dal)
            {
                return dal.GetMeasurementsByPatientID(this.PatientID);
            }


        }
    }



}

