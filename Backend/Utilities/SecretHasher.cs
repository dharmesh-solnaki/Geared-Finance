using System.Text;
namespace Utilities;
public static class SecretHasher
{

    public static string EnryptString(string strEncrypted)
    {
        byte[] b = ASCIIEncoding.ASCII.GetBytes(strEncrypted);
        string encrypted = Convert.ToBase64String(b);
        return encrypted;
    }
    public static string DecryptString(string strEncrypted)
    {
        byte[] bytes = Convert.FromBase64String(strEncrypted);
        string decrypted = ASCIIEncoding.ASCII.GetString(bytes);
        return decrypted;
    }

}
