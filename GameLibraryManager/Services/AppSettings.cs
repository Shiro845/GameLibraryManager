using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryManager.Services;
public class AppSettings
{
    public string Language { get; set; } = "en";
    public string Resolution { get; set; } = "1280x720";
    public double WindowOpacity { get; set; } = 1.0;
    public bool Fullscreen { get; set; } = false;
}
