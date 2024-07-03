using System;
using CMAS_App.CMAS_App;
using CMAS_App;

namespace CMAS_App
{
    public class Guardian : User
    {
        public int GuardianID { get; set; }
        public List<Patient> Patients { get; set; }

        public void ViewMeasurements(DAL dal)
        {
            List<Patient> patients = dal.GetPatientsByGuardianId(UserID); // Use UserID
                    if (patients.Count == 0)
                    {
                        Console.WriteLine("No patients found for this guardian.");
                        // continue;
                    }

                    foreach (var patient in patients)
                    {
                        Console.WriteLine($"Patient: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
                        List<CMASMeasurement> measurements = dal.GetMeasurementsByPatientID(patient.PatientID);
                        foreach (var measurement in measurements)
                        {
                            Console.WriteLine($"Measurement ID: {measurement.MeasurementID}, Date: {measurement.Date}, Feedback: {measurement.Feedback}");
                        }
                    }
        }



       
    }



}

