using System;
namespace CMAS_App
{
    public class CMASMeasurement
    {
        public int MeasurementID { get; set; }
        public DateTime Date { get; set; }
        public string Feedback { get; set; }
        public int PatientID { get; set; } // Add this property
        public List<ExerciseResult> ExerciseResults { get; set; }

        public CMASMeasurement()
        {
            ExerciseResults = new List<ExerciseResult>();
        }

        public void AddExerciseResult(ExerciseResult result)
        {
            ExerciseResults.Add(result);
        }

     

        public DateTime GetMeasurementDate()
        {
            return Date;
        }
    }


}

