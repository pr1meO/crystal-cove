using Microsoft.EntityFrameworkCore;

namespace CrystalCove.Models;

public class AppDbContext : DbContext
{
    // Определение сущностей в базе данных
    public DbSet<Client> Clients { get; set; } // Клиент

    public DbSet<Room> Rooms { get; set; } // Номер

    public DbSet<Staff> Staffs { get; set; } // Сотрудники

    public DbSet<Booking> Bookings { get; set; } // Бронирование

    public DbSet<Role> Roles { get; set; } // Роль

    // Конструктор
    public AppDbContext()
    {
        Database.EnsureCreated();
        SeedInitialData();
    }

    // Переопределение метода для подключения к БД
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=crystal_cove_db;Username=postgres;Password=admin");
    }

    // Метод для добавления первоначальных данных
    private void SeedInitialData()
    {
        if (!Roles.Any())
        {
            List<Role> initialRoles = 
            [
                new() 
                { 
                    Post = "Администратор гостиницы" 
                },
            ];

            Roles.AddRange(initialRoles);
            SaveChanges();
        }

        if (!Staffs.Any())
        {
            List<Staff> initialStaffs = 
            [
                new()
                {
                    FirstName = "Иванов",
                    LastName = "Иван",
                    Patronymic = "Иванович",
                    UserName = "admin",
                    Password = "111",
                    RoleId = 1
                },
            ];

            Staffs.AddRange(initialStaffs);
            SaveChanges();
        }
    }
}
