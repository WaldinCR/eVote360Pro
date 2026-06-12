using System.Security.Cryptography;
using System.Text;


namespace eVote360Pro.Core.Application.Helpers
{
  public static class PasswordEncryptation
    {
        public static string ComputeSha256Hash(string password)
        {
            //create a sha256
            using SHA256 sha256Hash = SHA256.Create();
            //computehash
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            //convert byte array to a string
             StringBuilder sb = new();

             foreach (var item in bytes)
             {
                sb.Append(item.ToString("x2"));
             }

                return sb.ToString();  

        }
            
    }
}
