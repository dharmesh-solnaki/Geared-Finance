namespace Utilities
{
    public class StringGenerator
    {
        private static Random random = new Random();

        public static string GenerateUniqueString(int length = 3)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
