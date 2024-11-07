using System.Security.Cryptography;
using System.Text;

namespace WFConfin.Services
{
    public class MD5Hash //criptografia
    {
        public static string CalcHash(string valor)
        {
            try
            {
                MD5 mD5 = MD5.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(valor); //transformar para bytes
                byte[] hash = mD5.ComputeHash(inputBytes); //gerar hash apartir sdo bytes
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2")); //transformar a informação de volta em texto
                }
                return sb.ToString();

            }
            catch (Exception e)
            {

                return null;
            }
        }
    }
}
