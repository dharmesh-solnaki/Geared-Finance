using System.Security.Cryptography;

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

        private static readonly char[] Numbers = "0123456789".ToCharArray();
        private static readonly char[] Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public static string GenerateOtp(int length,bool isOnlyNumbers)
        {
            char[] charSet = isOnlyNumbers? Numbers:Letters;
            byte[] randomNumber = new byte[length];
            char[] otp = new char[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            for (int i = 0; i < length; i++) {
                otp[i]=charSet[ randomNumber[i] % charSet.Length ];
            }
            return new string(otp);
        }
    }
}
