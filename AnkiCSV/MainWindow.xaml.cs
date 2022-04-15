using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnkiCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BaixarMP3_Click(object sender, RoutedEventArgs e)
        {
            new DownloadMP3() { Owner = this }.ShowDialog();
        }

        private void GerarCSV_Click(object sender, RoutedEventArgs e)
        {
            new GenereteCSV() { Owner = this }.ShowDialog();
        }
    }
}
