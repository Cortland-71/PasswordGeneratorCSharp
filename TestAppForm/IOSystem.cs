using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestAppForm
{
    class IOSystem
    {
        private string path = @"C:\PWG\Passwords.txt";
        private string dir = @"C:\PWG";

        //View------------------------------------------------------------------------
        public string View()
        {
            try
            {
                File.WriteAllText(path, Decrypt(File.ReadAllText(path), "Password"));

                string[] input = File.ReadAllLines(path);

                File.WriteAllText(path, Encrypt(File.ReadAllText(path), "Password"));

                return TurnRawTextIntoString(input);
            }
            catch (Exception)
            {
                return "No file was detected." +
                       "Please generate and SAVE a new " +
                       "password before attempting to view the file.";
            }
        }

        private string TurnRawTextIntoString(string[] rawText)
        {
            string output = string.Empty;

            foreach (string line in rawText)
                output += line + Environment.NewLine;
            return output;
        }

        //Save------------------------------------------------------------------------
        public string Save(string password, string comment)
        {
            string check = string.Empty;

            try
            {
                check = CheckIfDirExists();
                string dectrypted = Decrypt(File.ReadAllText(path), "Password");
                File.WriteAllText(path, dectrypted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            File.AppendAllLines(path, FormatOuput(password, comment));
            File.WriteAllText(path, Encrypt(File.ReadAllText(path), "Password"));

            return check;
        }

        private string CheckIfDirExists()
        {
            
            if (Directory.Exists(dir))
                return "Save was successfull!";
            else
                Directory.CreateDirectory(dir);

            return "Directory was made, save was successfull!";
        }

        private string[] FormatOuput(string password, string comment)
        {
            string[] formattedOutput = {
            $"---------------------------------------------------------------",
            $"Date time: {DateTime.Now}",
            $"Comment: {comment}",
            $"Password: {password}",
            $"---------------------------------------------------------------" };

            return formattedOutput;
        }

        //----------------------------------------------------------------------------
        public void WriteUpdatedPasswords()
        {
            File.WriteAllLines(path, RemoveLatestEntry(path));
           
            File.WriteAllText(path, Encrypt(File.ReadAllText(path), "Password"));
        }

        private string[] RemoveLatestEntry(string path)
        {
            File.WriteAllText(path, Decrypt(File.ReadAllText(path), "Password"));

            string[] allLines = File.ReadAllLines(path);
            
            List<string> lines = allLines.ToList();

            int end = Array.LastIndexOf(allLines, 
                "---------------------------------------------------------------");

            lines.RemoveAt(end);

            int start = lines.LastIndexOf(
                "---------------------------------------------------------------");

            int linesToRemove = lines.Count - start;

            lines.RemoveRange(start, linesToRemove);

            allLines = lines.ToArray();

            return allLines;
        }

        public void DeleteDir()
        {
            File.Delete(path);
            Directory.Delete(dir);
        }

        //----------------------------------------------------------------------------
        //Pass in the password to generate a key
        private TripleDES CreateDES(string  key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        private string Encrypt(string plainText, string password)
        {
            byte[] plainTextBytes = Encoding.Unicode.GetBytes(plainText);

            MemoryStream myStream = new MemoryStream();

            TripleDES des = CreateDES(password);

            CryptoStream cryptStream = new CryptoStream(myStream, des.CreateEncryptor(),CryptoStreamMode.Write);

            cryptStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptStream.FlushFinalBlock();

            return Convert.ToBase64String(myStream.ToArray());
        }

        private string Decrypt(string encryptedText, string password)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

            MemoryStream myStream = new MemoryStream();

            TripleDES des = CreateDES(password);

            CryptoStream decryptStream = new CryptoStream(myStream, des.CreateDecryptor(), CryptoStreamMode.Write);

            decryptStream.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);
            decryptStream.FlushFinalBlock();

            return Encoding.Unicode.GetString(myStream.ToArray());
        }
    }
}
