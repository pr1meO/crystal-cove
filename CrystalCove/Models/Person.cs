namespace CrystalCove.Models;

public abstract class Person
{
    abstract public int Id { get; set; } // Личный номер

    public string FirstName { get; set; } = string.Empty; // Имя

    public string LastName { get; set; } = string.Empty; // Фамилия

    public string Patronymic { get; set; } = string.Empty; // Отчество
}
