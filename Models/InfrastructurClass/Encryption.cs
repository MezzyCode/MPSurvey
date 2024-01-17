using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model.InfrastructurClass
{
    public static class ConstantVariable
    {
        public static string KELURAHANCODE = "Kelurahan";
        public static string KECAMATANCODE = "Kecamatan";
        public static string CHOISE3CODE = "CHOISE3";
        public static string CHOISE2CODE = "CHOISE2";
        public static string CALONCODE = "CALON";
        public static string AGAMACODE = "AGAMA";
        public static string PENDIDIKANCODE = "PENDIDIKAN";
        public static string SUKUCODE = "SUKU";
        public static string RWCODE = "RW";
        public static string KOTACODE = "Kota";

        public static string ANSWEREXPORTFILENAME = "ANSWEREXPORTFILENAME";

        public static string OrderByDefault = "CreatedTime";
        public static string OrderByDirectionDefault = "desc";
    }

    public class Encryption
    {
        public static string passwordEncrypt(string inText, string key)
        {
            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    inText = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return inText;
        }
        //Decrypting a string
        public static string passwordDecrypt(string cryptTxt, string key)
        {
            try
            {
                cryptTxt = cryptTxt.Replace(" ", "+");
                byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Close();
                        }
                        cryptTxt = Encoding.Unicode.GetString(mStream.ToArray());
                    }
                }
                return cryptTxt;
            }
            catch (Exception)
            {
                return "Invalid Token";
            }
        }
    }
}
