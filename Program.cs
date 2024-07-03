using System;
using System.Collections.Generic;
using CMAS_App.CMAS_App;

namespace CMAS_App
{
    class Program
    {
        static void Main(string[] args)
        {
            DAL dal = new DAL();

            Console.WriteLine("Select role: (1) Patient, (2) Doctor, (3) Guardian");
            string roleInput = Console.ReadLine();
            int role = int.Parse(roleInput);

            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            User user = new User { Name = name };
            user.Login(dal);

            if (user.UserID != 0)
            {
                switch (role)
                {
                    case 2: // Doctor
                        Doctor doctor = new Doctor { Name = user.Name, UserID = user.UserID, DoctorID = user.UserID };
                        DoctorDashboard(dal, doctor);
                        break;
                    case 3: // Guardian
                        Guardian guardian = new Guardian { Name = user.Name, UserID = user.UserID, GuardianID = user.UserID };
                        GuardianDashboard(dal, guardian);
                        break;
                    case 1: // Patient
                        Patient patient = new Patient { Name = user.Name, UserID = user.UserID, PatientID = user.UserID };
                        PatientDashboard(dal, patient);
                        break;
                    default:
                        Console.WriteLine("Invalid role.");
                        break;
                }
            }
        }

        static void DoctorDashboard(DAL dal, Doctor doctor)
        {
            while (true)
            {
                Console.WriteLine($"Welcome, {doctor.Name}. We are glad to see you.");
                Console.WriteLine("1. View all measurements");
                Console.WriteLine("2. Add feedback to a measurement");
                Console.WriteLine("3. View measurement details");
                Console.WriteLine("4. Exit");
                Console.WriteLine("Type your choice:");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    doctor.ViewAllMeasurements(dal);
                }
                else if (input == "2")
                {
                    doctor.AddFeedbackToMeasurement(dal);
                }
                else if (input == "3")
                {
                    doctor.ViewMeasurementDetails(dal);
                }
                else if (input == "4")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }



        static void GuardianDashboard(DAL dal, Guardian guardian)
        {
            while (true)
            {
                Console.WriteLine($"Welcome, {guardian.Name}. We are glad to see you.");
                Console.WriteLine("1. View patient's measurements");
                Console.WriteLine("Type 'exit' to leave the dashboard.");

                string input = Console.ReadLine();
                if (input == "1")
                {
                    guardian.ViewMeasurements(dal);
                }
                else if (input.ToLower() == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }


        static void PatientDashboard(DAL dal, Patient patient)
        {
            while (true)
            {
                Console.WriteLine($"Welcome, {patient.Name}. We are glad to see you <3.");
                Console.WriteLine("1. Do a measurement");
                Console.WriteLine("2. See your measurements and feedback");
                Console.WriteLine("3. Information");
                Console.WriteLine("Type the number that you want to do, press 'x' to log out:");

                string input = Console.ReadLine();

                if (input.ToLower() == "x")
                {
                    break;
                }

                switch (input)
                {
                    case "1":
                        DoMeasurement(dal, patient);
                        break;
                    case "2":
                        ViewPatientMeasurements(dal, patient);
                        break;
                    case "3":
                        Console.WriteLine("Information video YouTube link");
                        Console.WriteLine("Press any key to return");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }


       

        static void DoMeasurement(DAL dal, Patient patient)
        {
            patient.PerformMeasurement(dal);
            Console.WriteLine("Measurement completed and saved.");
        }

      

      

        static void ViewPatientMeasurements(DAL dal, Patient patient)
        {
            Console.WriteLine("Viewing measurements and feedback...");
            List<CMASMeasurement> measurements = patient.ViewMeasurements(dal);

            foreach (var measurement in measurements)
            {
                Console.WriteLine($"Measurement ID: {measurement.MeasurementID}, Date: {measurement.Date}, Feedback: {measurement.Feedback}");
            }
        }
    }
}
