using Database.Model;

namespace Database.Context
{
    internal class DBTest
    {
        static void Main(string[] args)
        {
            try
            {
                using (var db = new BudgetManagementContext())
                {
                    Console.WriteLine("Database connection initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }
    }

    class DBInsertTest
    {
        public DBInsertTest()
        {
            try
            {
                using (var db = new BudgetManagementContext())
                {
                    Console.WriteLine("Starting database operation at: " + DateTime.Now);

                    User myUser = new User
                    {
                        Name = "Test User",
                        Email = "admin@admin.admin",
                        PasswordHash = "Hash",
                        IsActive = true
                    };

                    db.Users.Add(myUser);
                    int rowsAffected = db.SaveChanges();

                    Console.WriteLine(rowsAffected > 0 ? "Data saved successfully." : "No rows affected.");
                    Console.WriteLine("Finished database operation at: " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
