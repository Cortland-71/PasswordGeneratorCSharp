using System;
using System.Collections.Generic;

namespace TestAppForm
{
    public class Generator
    {
        Random rand = new Random();
        
        //(Part 2) Asignes random characters to the password deppending on
        //the checkboxes that are checked.

        public string CreateFinalPassword(List<object> checkedBoxes, int passwordLength, string included)
        {
            string rawPassword;
            string finalPassword;

            rawPassword = MakeRawPassword(checkedBoxes, passwordLength);

            rawPassword = ShufflePassword(rawPassword);

            rawPassword = IncludeUserString(included, rawPassword);

            finalPassword = CheckPasswordLength(rawPassword, passwordLength);

            return finalPassword;
        }

        private string MakeRawPassword(List<object> checkedBoxes, int passwordLength)
        {
            string rawPassword = string.Empty;
            
            int randomNumber = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                if (checkedBoxes[i].Equals(true))
                {
                    for (int j = 0; j < passwordLength; j++)
                    {
                        randomNumber = rand.Next(0, elements[i].Length);
                        rawPassword += elements[i][randomNumber];
                    }
                }
            }
            return rawPassword;
        }

        private string ShufflePassword(string password)
        {
            char[] passwordArr = password.ToCharArray();
            int passwordLength = passwordArr.Length;

            while (passwordLength > 1)
            {
                int randomIndex = rand.Next(passwordLength--);
                char temp = passwordArr[passwordLength];

                passwordArr[passwordLength] = passwordArr[randomIndex];
                passwordArr[randomIndex] = temp;
            }
            return string.Join("", passwordArr);
        }

        private string IncludeUserString(string included, string rawPassword)
        {
            return included.Replace(" ", "") + rawPassword;
        }

        private string CheckPasswordLength(string password, int passwordLength)
        {
            if (password.Length > passwordLength)
                password = password.Remove(passwordLength);
            return password;
        }

        char[][] elements =
        {
            new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                         'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                         'u', 'v', 'w', 'x', 'y', 'z' },
            new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                         'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                         'U', 'V', 'W', 'X', 'Y', 'Z' },
            new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' },
            new char[] { '!', '@', '#', '$', '%', '^', '&', '*'},
            new char[] { '<', '>', '?', '{', '}', '[', ']', '(', ')', '-', '+', '=' }
        };

        //-----------------------------------------------------------------------------
        public string ConvertToLeetSpeak(string includedString, bool check)
        {
            char[,] vowels = { { 'a', 'e', 'i', 'o', 'u', 'y' },
                               { '@', '3', '!', '0', 'V', '7' } };

            if (check.Equals(true))
                foreach (char element1 in includedString)
                    for (int i = 0; i < vowels.GetLength(1); i++)
                        if (element1.Equals(vowels[0, i]))
                            includedString = includedString.Replace(element1, vowels[1, i]);

            if (check.Equals(false))
                foreach (char element in includedString)
                    for (int j = 0; j < vowels.GetLength(1); j++)
                        if (element.Equals(vowels[1, j]))
                            includedString = includedString.Replace(element, vowels[0, j]);

            return includedString;
        }


    }
}