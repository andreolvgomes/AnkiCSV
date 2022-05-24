using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for DownloadMP3.xaml
    /// </summary>
    public partial class DownloadMP3 : Window
    {
        public DownloadMP3()
        {
            InitializeComponent();
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var listSentences = txtSenteces.Text.Replace("\r", "").Split('\n').ToList();
                    var listUrls = txtUrls.Text.Replace("\r", "").Split('\n').ToList();
                    if (Validate(listSentences, listUrls) == false) return;

                    var di = new DirectoryInfo(txtSaveTo.Text);
                    foreach (FileInfo file in di.GetFiles())
                        file.Delete();

                    for (int i = 0; i < listUrls.Count; i++)
                    {
                        var item = listUrls[i];
                        var splitUrl = item.Split('/');
                        var fileName = splitUrl[splitUrl.Length - 1];

                        // só captura a Frase da caixa de texto se não for capturar do Title do mp3
                        if ((bool)chkGetNameFromTitle.IsChecked == false)
                        {
                            if ((bool)chk.IsChecked)
                                fileName = listSentences[i].Replace("?", "").Replace("!", "") + ".mp3";
                        }

                        fileName = System.IO.Path.Combine(txtSaveTo.Text, fileName);

                        client.DownloadFile(item, fileName);

                        // captura a frase do title do mp3
                        if ((bool)chkGetNameFromTitle.IsChecked)
                        {
                            var file = TagLib.File.Create(fileName);
                            if (file.Tag.Title != null)
                            {
                                var newFile = file.Tag.Title.Replace("?", "").Replace("!", "") + ".mp3";
                                //System.IO.File.Move(fileName, System.IO.Path.Combine(txtSaveTo.Text, newFile));
                                System.IO.File.Copy(fileName, System.IO.Path.Combine(txtSaveTo.Text, newFile), true);
                                System.IO.File.Delete(fileName);
                            }
                        }
                    }
                }
                MessageBox.Show("Audios baixados com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate(List<string> listSentences, List<string> listUrls)
        {
            if (string.IsNullOrEmpty(txtUrls.Text))
            {
                MessageBox.Show("Informe as URLs", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if ((bool)chk.IsChecked)
            {
                if (string.IsNullOrEmpty(txtSenteces.Text))
                {
                    MessageBox.Show("Informe as frases", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (listSentences.Count > listUrls.Count)
                {
                    MessageBox.Show("A quantidade de Frases é maior do que URLs", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (listUrls.Count > listSentences.Count)
                {
                    MessageBox.Show("A quantidade de URLs é maior do que Frases", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtSaveTo.Text))
            {
                MessageBox.Show("Informe o local onde salvar os audios", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if ((bool)chkGetNameFromTitle.IsChecked && (bool)chk.IsChecked)
            {
                MessageBox.Show("Os dois CheckBox não podem estar marcados ao mesmo tempo", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if ((bool)chkGetNameFromTitle.IsChecked == false)
            {
                var sentencesEquals = listSentences.GroupBy(c => c).Where(w => w.Count() > 1).Select(s => s.Key).ToList();
                var urlsEquals = listUrls.GroupBy(c => c).Where(w => w.Count() > 1).Select(s => s.Key).ToList();

                if (sentencesEquals.Count > 0 || urlsEquals.Count > 0)
                {
                    MessageBox.Show($"Frases repetidas:\n\n" + string.Join("\n", sentencesEquals) + "\n\nURLs repetidas:\n\n" + string.Join("\n", urlsEquals), "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }

        private void SaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = true;
                var result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    txtSaveTo.Text = dlg.SelectedPath;
            }
        }
    }
}