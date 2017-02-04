using System;
using System.Text;

namespace Phoneword
{
    public interface IPhoneTranslator
    {
        string ToNumber(string userInput);
    }
}
