namespace GameLibrary;

public static class Rand
{
    public static Random rand = new Random();
    public static string RandomString(int lenght = 5)
    {
        char[] chars = new char[lenght];
        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = (char)rand.Next('a', 'z');
        }
        return new string(chars);
    }
}