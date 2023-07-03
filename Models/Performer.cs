namespace Models;

/// <summary>
/// Класс Performer представляет информацию об ответственном исполнителе.
/// </summary>
public class Performer
{
    /// <summary>
    /// Полное имя.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Количество РКК.
    /// </summary>
    public int RKKCount { get; set; }

    /// <summary>
    /// Количество обращений.
    /// </summary>
    public int AppealsCount { get; set; }

    /// <summary>
    /// Общее количество документов (РКК + обращения).
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Считает общее количество документов.
    /// </summary>
    public void Sum() {
        this.TotalCount = this.RKKCount + this.AppealsCount;
    }
}