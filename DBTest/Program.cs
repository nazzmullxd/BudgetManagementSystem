using Database.Model;

namespace Database.Context
{
    internal class DBTest
    {
        static void Main(string[] args)
        {
            // Test the database connection first.
            try
            {
                using (var db = new BudgetManagementContext())
                {
                    if (db.Database.CanConnect())
                    {
                        Console.WriteLine("Database connection successful.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }

            // Execute the insert operation test.
            new DBInsertTest().Run();

            // Pause the console so you can view the output.
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    class DBInsertTest
    {
        public void Run()
        {
            try
            {
                using (var db = new BudgetManagementContext())
                {
                    Console.WriteLine("Starting database operation at: " + DateTime.Now);

                    // Create a new user instance.
                    User myUser = new User
                    {
                        Name = "Test User",
                        Email = "admin@admin.admin",
                        PasswordHash = "Hash",
                        IsActive = true,
                        // The CreatedAt and UpdatedAt properties should be set automatically 
                        // if you've configured them accordingly in your model.
                    };

                    // Add the new user and save changes.
                    db.Users.Add(myUser);
                    int rowsAffected = db.SaveChanges();

                    Console.WriteLine(rowsAffected > 0 ? "Data saved successfully." : "No rows affected.");
                    Console.WriteLine("Finished database operation at: " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during database operation: {ex.Message}");
            }
        }
    }
}
