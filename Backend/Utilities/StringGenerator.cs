using System.Security.Cryptography;

namespace Utilities;

public class StringGenerator
{
    private readonly static Random random = new();

    public static string GenerateUniqueString(int length = 3)
    {
        const string chars = Constants.CAPITAL_LETTERS;
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GenerateOtp(int length, bool isOnlyNumbers)
    {
        char[] Numbers = Constants.NUMBERS_ARRAY.ToCharArray();
        char[] Letters = Constants.LETTERS_ARRAY.ToCharArray();
        char[] charSet = isOnlyNumbers ? Numbers : Letters;
        byte[] randomNumber = new byte[length];
        char[] otp = new char[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        for (int i = 0; i < length; i++)
        {
            otp[i] = charSet[randomNumber[i] % charSet.Length];
        }
        return new string(otp);
    }
}

