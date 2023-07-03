using Models;
using Parents;
using System.Diagnostics;
using System.Text;

class Program
{
    /// <summary>
    /// Объединяет данные из двух документов и возвращает список исполнителей.
    /// </summary>
    /// <param name="rkk">Документ РКК.</param>
    /// <param name="appeals">Документ обращений.</param>
    /// <returns>Список исполнителей.</returns>
    static List<Performer> MergeData(Document rkk, Document appeals)
    {
        List<Performer> performers = new List<Performer>();

        List<string> rkkPersons = rkk.GetResponsibleExecutors();
        List<string> appealsPersons = appeals.GetResponsibleExecutors();

        List<string> uniquePersons = rkkPersons.Union(appealsPersons).ToList();

        Dictionary<string, int> _rkkPersons = rkk.GetDocumentsCount(uniquePersons);
        Dictionary<string, int> _appealsPersons = appeals.GetDocumentsCount(uniquePersons);

        // Добавляем исполнителей из списка РКК
        foreach (string person in _rkkPersons.Keys)
        {
            Performer? existingPerformer = performers.FirstOrDefault(p => p.FullName == person);
            if (existingPerformer != null)
            {
                existingPerformer.RKKCount = _rkkPersons[person];
            }
            else
            {
                performers.Add(new Performer
                {
                    FullName = person,
                    RKKCount = _rkkPersons[person]
                });
            }
        }


        // Добавляем исполнителей из списка обращений
        foreach (string person in _appealsPersons.Keys)
        {
            Console.WriteLine(person);
            Performer? existingPerformer = performers.FirstOrDefault(p => p.FullName == person);
            if (existingPerformer != null)
            {
                existingPerformer.AppealsCount = _appealsPersons[person];
            }
            else
            {
                performers.Add(new Performer
                {
                    FullName = person,
                    AppealsCount = _appealsPersons[person]
                });
            }
        }

        // Вычисляем общую сумму обработанных документов
        foreach (Performer performer in performers)
        {
            performer.Sum();
        }

        // Удаляем элементы с TotalCount = 0
        performers.RemoveAll(p => p.TotalCount == 0);

        return performers;
    }

    static List<Performer> NameSort(List<Performer> performers) {
        performers.Sort((p1, p2) =>
        {
            int nameComparison = p2.TotalCount.CompareTo(p1.TotalCount);
            int totalComparison = p2.TotalCount.CompareTo(p1.TotalCount);
            int rkkComparions = p2.RKKCount.CompareTo(p1.RKKCount);

            if (nameComparison != 0)
                return nameComparison;
            else if (totalComparison != 0) {
                return totalComparison;
            }
            else if (rkkComparions != 0) {
                return rkkComparions;
            }

            return p2.AppealsCount.CompareTo(p1.AppealsCount);
        });

        return performers;
    }

    static List<Performer> RkkSort(List<Performer> performers) {
        performers.Sort((p1, p2) =>
        {
            int totalComparison = p2.TotalCount.CompareTo(p1.TotalCount);
            int rkkComparions = p2.RKKCount.CompareTo(p1.RKKCount);
            int appealsComparions = p2.AppealsCount.CompareTo(p1.AppealsCount);

            if (rkkComparions != 0)
                return rkkComparions;
            else if (rkkComparions != 0) {
                return rkkComparions;
            }
            else if (appealsComparions != 0) {
                return totalComparison;
            }

            if (p1.FullName == null && p2.FullName == null)
            {
                return 0;
            }
            else if (p1.FullName == null)
            {
                return 1;
            }
            else if (p2.FullName == null)
            {
                return -1;
            }

            return p1.FullName.CompareTo(p2.FullName);
        });

        return performers;
    }

    static List<Performer> AppealsSort(List<Performer> performers) {
        performers.Sort((p1, p2) =>
        {
            int totalComparison = p2.TotalCount.CompareTo(p1.TotalCount);
            int rkkComparions = p2.RKKCount.CompareTo(p1.RKKCount);
            int appealsComparions = p2.AppealsCount.CompareTo(p1.AppealsCount);

            if (appealsComparions != 0)
                return appealsComparions;
            else if (rkkComparions != 0) {
                return rkkComparions;
            }
            else if (appealsComparions != 0) {
                return totalComparison;
            }

            if (p1.FullName == null && p2.FullName == null)
            {
                return 0;
            }
            else if (p1.FullName == null)
            {
                return 1;
            }
            else if (p2.FullName == null)
            {
                return -1;
            }

            return p1.FullName.CompareTo(p2.FullName);
        });

        return performers;
    }

    static List<Performer> TotalSort(List<Performer> performers) {
        performers.Sort((p1, p2) =>
        {
            int totalComparison = p2.TotalCount.CompareTo(p1.TotalCount);
            int rkkComparions = p2.RKKCount.CompareTo(p1.RKKCount);
            int appealsComparions = p2.AppealsCount.CompareTo(p1.AppealsCount);

            if (totalComparison != 0)
                return totalComparison;
            else if (rkkComparions != 0) {
                return rkkComparions;
            }
            else if (appealsComparions != 0) {
                return appealsComparions;
            }

            if (p1.FullName == null && p2.FullName == null)
            {
                return 0;
            }
            else if (p1.FullName == null)
            {
                return 1;
            }
            else if (p2.FullName == null)
            {
                return -1;
            }

            return p1.FullName.CompareTo(p2.FullName);
        });

        return performers;
    }

    /// <summary>
    /// Сортирует список исполнителей.
    /// </summary>
    /// <param name="performers">Список исполнителей.</param>
    /// <returns>Отсортированный список исполнителей.</returns>
    static List<Performer> SortPerformers(List<Performer> performers, string? sortOption)
    {
        switch (sortOption)
        {
            case "1":
                NameSort(performers);
                break;
            case "2":
                RkkSort(performers);
                break;
            case "3":
                AppealsSort(performers);
                break;
            case "4":
                TotalSort(performers);
                break;
        }

        return performers;
    }

    /// <summary>
    /// Записывает список исполнителей в текстовый файл.
    /// </summary>
    /// <param name="performers">Список исполнителей.</param>
    /// <param name="filePath">Путь к файлу.</param>
    /// <param name="executionTime">Время выполнения программы.</param>
    static void WritePerformersToText(List<Performer> performers, string filePath, TimeSpan executionTime)
    {
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine("Отчет о выполненной работе исполнителей");
            writer.WriteLine("--------------------------------------");
            writer.WriteLine($"Дата составления отчета: {DateTime.Now}");
            writer.WriteLine($"Время выполнения программы: {executionTime}");
            writer.WriteLine();

            writer.WriteLine("Исполнители:");
            writer.WriteLine("----------------------------------------------------------------");
            writer.WriteLine("| ФИО                            | РКК | Обращения | Всего |");
            writer.WriteLine("----------------------------------------------------------------");

            foreach (Performer performer in performers)
            {
                string line = $"| {performer.FullName,-30} | {performer.RKKCount,3} | {performer.AppealsCount,9} | {performer.TotalCount,5} |";
                writer.WriteLine(line);
            }

            writer.WriteLine("------------------------------------------------------------------");
        }
    }

    static bool IsValidSortOption(string? sortOption)
    {
        return sortOption != null && (sortOption == "1" || sortOption == "2" || sortOption == "3" || sortOption == "4");
    }

    static void Main(string[] args)
    {
        Rkk rkk = new Rkk();
        Appeals appeals = new Appeals();

        Console.WriteLine("Выберите параметр сортировки:");
        Console.WriteLine("1. ФИО (в алфавитном порядке)");
        Console.WriteLine("2. Количество РКК (по убыванию)");
        Console.WriteLine("3. Количество обращений (по убыванию)");
        Console.WriteLine("4. Общее количество (по убыванию)");

        string? sortOption = Console.ReadLine();

        while (!IsValidSortOption(sortOption))
        {
            Console.WriteLine("Неверный выбор параметра сортировки. Пожалуйста, выберите снова.");
            sortOption = Console.ReadLine();
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        rkk.ReadData();
        appeals.ReadData();

        List<Performer> performers = MergeData(rkk, appeals);

        List<Performer> sortedPerformers = SortPerformers(performers, sortOption);
        stopwatch.Stop();

        Console.Write("Введите путь к файлу для сохранения (прим. performers.txt): ");
        string? filePath = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Путь к файлу не может быть пустым. Пожалуйста, введите путь заново.");
            filePath = Console.ReadLine();
        }

        WritePerformersToText(sortedPerformers, filePath, stopwatch.Elapsed);

        Console.WriteLine($"Отчет сохранен в файле {filePath}");
    }
}