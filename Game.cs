using Godot;
public class Game : Node2D
{
    public static bool IsDebug = true;
    public static DynamicFont DebugFont;

    public override void _Ready()
    {
        DebugFont = new DynamicFont();
        DebugFont.FontData = GD.Load<DynamicFontData>("res://Mabinogi_Classic_TTF.ttf");
        DebugFont.Size = 12;
    }
}
