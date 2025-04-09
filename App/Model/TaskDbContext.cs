using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace App.Model;

public class TaskDbContext : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "server=localhost;database=your_database_name;user=root;password=rootpassword;",
            new MySqlServerVersion(new Version(10, 5, 0)) // Remplacez par la version de votre MariaDB
        );
    }
}