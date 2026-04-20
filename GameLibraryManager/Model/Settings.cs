namespace GameLibraryManager.Model;

/// <summary>
/// Модель налаштувань програми, включаючи мову, роздільну здатність, прозорість вікна та режим повноекранного відображення.
/// </summary>
public class Settings
{
    public string Language { get; set; } = "en";
    public string Resolution { get; set; } = "1280x720";
    public double WindowOpacity { get; set; } = 1.0;
    public bool Fullscreen { get; set; } = false;
}
