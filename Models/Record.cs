namespace Models;

/// <summary>
/// Класс Record представляет запись.
/// </summary>
public class Record
{
    /// <summary>
    /// Руководитель.
    /// </summary>
    public string? Supervisor { get; set; }

    /// <summary>
    /// Исполнитель.
    /// </summary>
    public string? Performer { get; set; }
}