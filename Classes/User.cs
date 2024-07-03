using System;
using CMAS_App;

namespace CMAS_App
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }


        public void Login(DAL dal)
        {
            User user = dal.GetUserByName(this.Name);
            if (user != null)
            {
                this.UserID = user.UserID;
                this.Age = user.Age;
                this.Gender = user.Gender;
               
                this.Role = user.Role;
                Console.WriteLine($"Welcome {user.Name}, you are logged in as {user.Role}.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }


}

