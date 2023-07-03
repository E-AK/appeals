using System.Text.RegularExpressions;
using Models;

namespace Parents;

public abstract class Document
{
    /// <summary>
    /// Список записей
    /// </summary>
    public List<Record> Records { get; set; }

    public Document()
    {
        Records = new List<Record>();
    }

    /// <summary>
    /// Читает данные из файла и заполняет список записей.
    /// </summary>
    /// <param name="filePath">Путь к файлу с данными.</param>
    public void ReadDataFromFile(string filePath)
    {
        // Строки
        string[] lines = File.ReadAllLines(filePath);
        Console.WriteLine(lines.Length);

        // Перебираем строки
        foreach(string line in lines)
        {
            // Разделяем колонки
            string[] columns = line.Split('\t');

            // Руководитель
            string supervisor = columns[0];

            // Исполнители
            string performer = columns[1].Split("; ")[0].Replace(" (Отв.)", "").Replace(". ", ".");

            // Фильтрация исполнителей по шаблону "Фамилия И.О."
            //performers = FilterPerformers(performer);

            // Создаем новую запись и добавляем ее в список
            Record record = new Record
            {
                Supervisor = supervisor,
                Performer = performer
            };

            Records.Add(record);
        };

        Console.WriteLine(Records.Count);
    }

    /// <summary>
    /// Получает список ответственных исполнителей
    /// </summary>
    public List<string> GetResponsibleExecutors()
    {
        List<string> performers = new List<string>();

        // Перебираем записи
        foreach (Record record in Records)
        {
            if (record.Supervisor != null)
            {
                if (record.Performer != null)
                {
                    string performer = record.Performer;
                    if (!IsDuplicatePerformer(performers, performer))
                    {
                        performers.Add(performer);
                    }
                }
                else
                {
                    string supervisor = record.Supervisor;
                    if (!IsDuplicatePerformer(performers, supervisor))
                    {
                        string[] supervisorParts = supervisor.Split(' ');
                        char supervisorFirstName = supervisorParts[1][0];
                        char supervisorLastName = supervisorParts[2][0];

                        string supervisorInitials = supervisorParts[0] + " " + supervisorFirstName + "." + supervisorLastName + ".";

                        performers.Add(supervisorInitials);
                    }
                }
            }
        }

        return performers;
    }

    /// <summary>
    /// Получает список ответственных исполнителей
    /// </summary>
    public Dictionary<string, int> GetDocumentsCount(List<string> performers)
    {
        Dictionary<string, int> docsPerformers = new Dictionary<string, int>();

        // Перебираем записи
        foreach (string performer in performers)
        {
            int count = Records.Count(record => record.Supervisor != null && ArePerformersSame(performer, record.Supervisor));
            docsPerformers.Add(performer, count);
        }

        return docsPerformers;
    }

    private bool IsDuplicatePerformer(List<string> performers, string performer)
    {
        foreach (string existingPerformer in performers)
        {
            if (ArePerformersSame(performer, existingPerformer))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Сравнивает 2 вида записи исполнителя
    /// </summary>
    private bool ArePerformersSame(string performer1, string performer2)
    {
        string[] performer1Parts = performer1.Split(' ');
        string[] performer2Parts = performer2.Split(' ');

        char performer1FirstName = ' ';
        char performer1LastName = ' ';
        char performer2FirstName = ' ';
        char performer2LastName = ' ';

        if (performer1Parts.Length == 2) {
            string[] io = performer1Parts[1].Split('.');

            performer1FirstName = io[0][0];
            performer1LastName = io[1][0];
        }
        else {
            performer1FirstName = performer1Parts[1][0];
            performer1LastName = performer1Parts[2][0];
        }

        if (performer2Parts.Length == 2) {
            string[] io = performer2Parts[1].Split('.');

            performer2FirstName = io[0][0];
            performer2LastName = io[1][0];
        }
        else {
            performer2FirstName = performer2Parts[1][0];
            performer2LastName = performer2Parts[2][0];
        }

        // Сравниваем фамилию и первые буквы имени и отчества
        return performer1Parts[0] == performer2Parts[0] &&
            performer1FirstName == performer2FirstName &&
            performer1LastName == performer2LastName;
    }
}