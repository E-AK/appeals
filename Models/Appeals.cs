using Interfaces;
using Parents;

namespace Models;

public class Appeals : Document, DataReader {
    public void ReadData()
    {
        Console.Write("Введите путь к файлу с данными об обращениях (прим. appeals.txt): ");
        string? filePath = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Путь к файлу не может быть пустым. Пожалуйста, введите путь заново.");
            filePath = Console.ReadLine();
        }

        this.ReadDataFromFile(filePath);
    }
}
