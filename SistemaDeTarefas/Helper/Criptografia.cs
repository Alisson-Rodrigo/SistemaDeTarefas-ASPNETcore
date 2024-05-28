using SistemaDeTarefas.Helper.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SistemaDeTarefas.Helper
{
    public static class Criptografia 
    {
        public static string GerarHash(this string pass)
        {
            var hash = SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var array = encoder.GetBytes(pass);
            array = hash.ComputeHash(array);
            var strHexa = new StringBuilder();

            foreach (byte item in array)
            {
                strHexa.Append(item.ToString("X2"));
            }

            return strHexa.ToString();
        }

    }
}