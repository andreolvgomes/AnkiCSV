using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace System
{
    public static class CheckBoxExtensions
    {
        public static bool IsCheck(this CheckBox check)
        {
            return (bool)check.IsChecked;
        }
    }
}