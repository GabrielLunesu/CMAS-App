using System;
namespace CMAS_App
{
    public class ExerciseResult
    {
        public int ResultID { get; set; }
        public int ExerciseID { get; set; } // Adding ExerciseID instead of Exercise object

        public Exercise Exercise { get; set; } // Add this property

        public int Score { get; set; }
        public float Duration { get; set; }
        public string Comments { get; set; }
        public int MeasurementID { get; set; } // Adding MeasurementID to link with CMASMeasurement

        // public void RecordResult()
        // {
        //     // Logic to record result
        // }

        // public void GetResultDetails()
        // {
        //     // Logic to get result details
        // }
    }

}

