using System.Security.Cryptography;
using System.Text;

namespace TodoList.Server.Helpers
{
    public static class Hash
    {
        public static string GetHash(string password)
        {
            var algorythm = SHA256.Create();

            StringBuilder sb = new StringBuilder();

            foreach (var item in algorythm.ComputeHash(Encoding.UTF8.GetBytes(password)))
            {
                sb.Append(item.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
