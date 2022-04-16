namespace HKMP_SpectatorMod.Utils;

public static class Extensions
{
    public static Color SetAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}