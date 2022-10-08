using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Diagnostics;


//the namespace must be PAT.Lib, the class and method names can be arbitrary
namespace PAT.Lib
{
    /// <summary>
    /// You can use static library in PAT model.
    /// All methods should be declared as public static.
    /// 
    /// The parameters must be of type "int", "bool", "int[]" or user defined data type
    /// The number of parameters can be 0 or many
    /// 
    /// The return type can be void, bool, int, int[] or user defined data type
    /// 
    /// The method name will be used directly in your model.
    /// e.g. call(max, 10, 2), call(dominate, 3, 2), call(amax, [1,3,5]),
    /// 
    /// Note: method names are case sensetive
    /// </summary>
    /// 


    public class ClassLibrary1
    {
        //use interop services to import c dll. The orginal c source code can be found under Lib folder
        [DllImport(@"Lib\random.dll")]
        public static extern int RAND(int max, int min, int n);

        public static string IntToString(int[] arr)
        {
            int num = arr.Length;
            string str = "";
            for (int i = 0; i < num - 1; i++)
                str = str + arr[i] + ",";
            str = str + arr[num - 1];
            return str;
        }

        public static string SaveContent(string data, string name)
        {
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + @"SaveDir\";
            if (!System.IO.Directory.Exists(CurDir))
                System.IO.Directory.CreateDirectory(CurDir);
            string filePath = CurDir + name + ".txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false);
            file.Write(data);
            file.Close();
            file.Dispose();
            return filePath;
        }

        public static string ReadContent(string name)
        {
            string CurDir = System.AppDomain.CurrentDomain.BaseDirectory + @"SaveDir\";
            string filePath = CurDir + name + ".txt";
            string str = "";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                str = file.ReadToEnd();
                file.Close();
                file.Dispose();
            }
            return str;
        }

        public static void Sign(int[] origin, int k)
        {
            string plaintext = IntToString(origin);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string privateKey = RSA.ToXmlString(true);
            string publicKey = RSA.ToXmlString(false);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(plaintext);
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAalg.FromXmlString(privateKey);
            byte[] encryptedData = RSAalg.SignData(dataToEncrypt, new SHA1CryptoServiceProvider());
            string data = Convert.ToBase64String(encryptedData);
            SaveContent(privateKey, k + "_pri");
            SaveContent(publicKey, k + "_pub");
            SaveContent(data, plaintext + "_" + k + "_signed");
        }

        public static bool CheckSigned(int[] origin, int[] sign, int k_ori, int k_sig)
        {
            string plaintext = IntToString(origin);
            string signtext = IntToString(sign);
            string SignedData = ReadContent(signtext + "_" + k_sig + "_signed");
            string publicKey = ReadContent(k_ori + "_pub");
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAalg.FromXmlString(publicKey);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToVerifyBytes = ByteConverter.GetBytes(plaintext);
            byte[] signedDataBytes = Convert.FromBase64String(SignedData);
            return RSAalg.VerifyData(dataToVerifyBytes, new SHA1CryptoServiceProvider(), signedDataBytes);
        }
    }
}
