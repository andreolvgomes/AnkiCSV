﻿using System;
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

                        if ((bool)chk.IsChecked)
                            fileName = listSentences[i].Replace("?", "").Replace("!", "") + ".mp3";

                        fileName = System.IO.Path.Combine(txtSaveTo.Text, fileName);

                        client.DownloadFile(item, fileName);
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