namespace CPD_Lab_01;

public static class HexUtility
{
    public const string HexadecimalCharacters = "0123456789ABCDEF";
    public static char AsHexChar(int value)
    {
        return HexadecimalCharacters[value % HexadecimalCharacters.Length];
    }
}
