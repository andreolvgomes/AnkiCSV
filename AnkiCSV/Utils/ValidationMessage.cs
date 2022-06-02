using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnkiCSV
{
    public class ValidationMessage
    {
        public bool IsValid
        {
            get
            {
                return Message.IsNullOrEmpty();
            }
        }
        public string Message { get; set; }

        public ValidationMessage()
            : this("")
        {
        }
        public ValidationMessage(string message)
        {
            Message = message;
        }

        public bool CheckIsValid()
        {
            if (IsValid) return true;

            MessageBox.Show(Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            return false;
        }
    }
}