namespace Utilities
{
    public static class SecretHasher
    {

        public static string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        public static string DecryptString(string strEncrypted)
        {
            byte[] bytes = Convert.FromBase64String(strEncrypted);
            string decrypted = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            return decrypted;
        }

    }
}