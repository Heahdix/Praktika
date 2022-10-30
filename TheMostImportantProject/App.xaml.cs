using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TheMostImportantProject
{

    public partial class App : Application
    {
        
    }
    public  class Emp
    {
        public Emp(int employeeID, string login, string password, string lastName, string firstName, string dateOfBirth, string phoneNumber)
        {
            EmployeeID = employeeID;
            Login = login;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
        }

        public int EmployeeID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }
    public static class CurrentUser
    {
        public static int Id { get; set; }
    }

    public static class BasketItems
    {
        public static List<BasketLine> Lines { get; set; } = new List<BasketLine>();
        public static void AddItem(Position position, int quantity = 1)
        {
            BasketLine line = Lines.Where(p => p.Position.PositionID == position.PositionID).FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new BasketLine
                {
                    Position = position,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }   
        }
    }

    public class BasketLine
    {
        public Position Position { get; set; }
        public int Quantity { get; set; }
    }
}
