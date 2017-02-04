using System;
using System.Text;

namespace Phoneword
{
    public class PhoneTranslator : IPhoneTranslator
    {
        public string ToNumber(string userInput)
        {
            StringBuilder phoneNumber = new StringBuilder();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                return string.Empty;
            }

            string helper = userInput.ToUpperInvariant();
            string validCharacters = " -1234567890";
            foreach (char character in helper)
            {
                if (validCharacters.IndexOf(character) >= 0)
                {
                    phoneNumber.Append(character);
                }
                else
                {
                    string number = this.TranslateToNumber(character);
                    phoneNumber.Append(number);
                }
            }

            return phoneNumber.ToString();
        }

        private string TranslateToNumber(char character)
        {
            if ("ABC".IndexOf(character) >= 0)
            {
                return 2.ToString();
            }
            else if ("DEF".IndexOf(character) >= 0)
            {
                return 3.ToString();
            }
            else if ("GHI".IndexOf(character) >= 0)
            {
                return 4.ToString();
            }
            else if ("JKL".IndexOf(character) >= 0)
            {
                return 5.ToString();
            }
            else if ("MNO".IndexOf(character) >= 0)
            {
                return 6.ToString();
            }
            else if ("PQRS".IndexOf(character) >= 0)
            {
                return 7.ToString();
            }
            else if ("TUV".IndexOf(character) >= 0)
            {
                return 8.ToString();
            }
            else if ("WXYZ".IndexOf(character) >= 0)
            {
                return 9.ToString();
            }

            return string.Empty;
        }
    }
}
