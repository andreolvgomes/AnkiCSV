using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
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

        private void Frases_Click(object sender, RoutedEventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "(*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txtPhrases.Text = openFileDialog.FileName;
            }
        }

        private void Audios_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = true;
                var result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    txtAudios.Text = dlg.SelectedPath;
            }
        }

        private void SaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (var saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.Filter = "(*.csv)|*.csv";
                saveFileDialog.FileName = $"Import Anki {DateTime.Now.ToString("ddMMyyyy HHmmss")}.csv";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txtSaveTo.Text = saveFileDialog.FileName;
            }
        }

        private void GerarCSV_Click(object sender, RoutedEventArgs e)
        {
            //https://www.youtube.com/watch?v=BwGNP3GXmxg
            //https://www.youtube.com/watch?v=02vwciRkRYg
            //C:\Users\André\AppData\Roaming\Anki2\André\collection.media
            try
            {
                var phrases = System.IO.File.ReadAllLines(txtPhrases.Text).Where(c => string.IsNullOrEmpty(c) == false).ToList();
                var audios = new System.IO.DirectoryInfo(txtAudios.Text).GetFiles().Where(w => w.Extension.Equals(".mp3")).ToList();
                if (!Validacao(phrases, audios)) return;

                //[sound:Did I hurt you.mp3]
                var list = new List<string>();
                foreach (var audio in audios)
                {
                    var audioName = audio.Name.Replace(".mp3", "");
                    var phrase = phrases.FirstOrDefault(c => c.StartsWith(audioName));
                    list.Add($"{phrase}[sound:{audio.Name}];");
                }

                System.IO.File.WriteAllLines(txtSaveTo.Text, list);
                MessageBox.Show("CSV salvo com sucesso!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validacao(List<string> phrases, List<FileInfo> audios)
        {
            if (string.IsNullOrEmpty(txtPhrases.Text))
            {
                MessageBox.Show("Informe o caminho do arquivo com as Frases", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtAudios.Text))
            {
                MessageBox.Show("Informe o caminho do diretóro com os audios .mp3", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtSaveTo.Text))
            {
                MessageBox.Show("Informe o caminho onde deseja salvar o CSV", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (phrases.Count > audios.Count)
            {
                MessageBox.Show("A quantidade de Frases é maior do que Áudios", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (audios.Count > phrases.Count)
            {
                MessageBox.Show("A quantidade de Áudios é maior do que Frases", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}