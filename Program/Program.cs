using Database.Context;
using Database.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Database.Context.BudgetManagementContext;

namespace Database.Context
{
    internal class DBTest
    {
        static void main(string[] args)
        {
            var db = new BudgetManagementContext();
        }
    }
    class DBInsertTest
    {
        public DBInsertTest()
        {
            var db = new CarParkingContext();
            Console.WriteLine(DateTime.Now);

            User myUser = new User();
            myUser.Name = null;
            myUser.Email = "admin@admin.admin";
            myUser.PasswordHash = "Hash";
            myUser.IsActive = true;

            db.User.Add(myUser);
            int row = db.SaveChanges();



            //for (int i = 0; i < 10; i++)
            //{
            //    db.UserInfo.Add(new UserInfo
            //    {
            //        FullName = "Manager-" + i,
            //        Email = "manager" + i + "@manager.manager",
            //        PasswordHash = "Hash" + i,
            //        IsActive = i % 2 == 0,
            //        RoleId = 2
            //    });
            //}
            //for (int i = 0; i < 1000; i++)
            //{
            //    db.UserInfo.Add(new UserInfo
            //    {
            //        FullName = "Client-" + i,
            //        Email = "client" + i + "@client.client",
            //        PasswordHash = "Hash" + i,
            //        IsActive = i % 2 == 0,
            //        RoleId = 3
            //    });
            //}
            //db.SaveChanges();
            Console.WriteLine(DateTime.Now);
        }
    }

}
