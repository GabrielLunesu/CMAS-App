using System;
using CMAS_App;
using CMAS_App.CMAS_App;

namespace CMAS_App
{
    public class Doctor : User
{
    public int DoctorID { get; set; }
    public string Specialization { get; set; }
    public List<Patient> Patients { get; set; }

    public void ViewAllMeasurements(DAL dal)
    {
        List<CMASMeasurement> measurements = dal.GetAllMeasurements();

        Console.WriteLine("All Measurements:");
        foreach (var measurement in measurements)
        {
            Console.WriteLine($"Measurement ID: {measurement.MeasurementID}, Date: {measurement.Date}, Feedback: {measurement.Feedback}, Patient ID: {measurement.PatientID}");
        }
    }

    public void AddFeedbackToMeasurement(DAL dal)
    {
        Console.WriteLine("Enter the Measurement ID to add feedback:");
        int measurementId = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the feedback:");
        string feedback = Console.ReadLine();

        dal.AddFeedback(measurementId, feedback);

        Console.WriteLine("Feedback added successfully.");
    }

    public void ViewMeasurementDetails(DAL dal)
    {
        Console.WriteLine("Enter the Measurement ID to view details:");
        int measurementId = int.Parse(Console.ReadLine());

        var measurementDetails = dal.GetDetailedMeasurementById(measurementId);

        if (measurementDetails != null && measurementDetails.Count > 0)
        {
            foreach (var (patientName, measurement, exercises) in measurementDetails)
            {
                Console.WriteLine($"Measurement ID: {measurement.MeasurementID}");
                Console.WriteLine($"Date: {measurement.Date}");
                Console.WriteLine($"Feedback: {measurement.Feedback}");
                Console.WriteLine($"Patient ID: {measurement.PatientID}");
                Console.WriteLine($"Patient Name: {patientName}");

                if (exercises != null && exercises.Count > 0)
                {
                    Console.WriteLine("Exercises:");
                    foreach (var exercise in exercises)
                    {
                        Console.WriteLine($"  Exercise ID: {exercise.Exercise.ExerciseID}, Name: {exercise.Exercise.Name}");
                        Console.WriteLine($"    Score: {exercise.Score}, Duration: {exercise.Duration}, Comments: {exercise.Comments}");
                    }
                }
                else
                {
                    Console.WriteLine("No exercises recorded for this measurement.");
                }
            }
        }
        else
        {
            Console.WriteLine("Measurement not found.");
        }
    }
}
}