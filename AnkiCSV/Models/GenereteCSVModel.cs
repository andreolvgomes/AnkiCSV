using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiCSV.Models
{
    public class GenereteCSVModel : NotifyPropertyChanged
    {
        private bool _use_senteces_from_audio;

        public bool Use_senteces_from_audio
        {
            get { return _use_senteces_from_audio; }
            set
            {
                if (_use_senteces_from_audio == value) return;
                _use_senteces_from_audio = value;
                OnPropertyChanged();
            }
        }

        private bool _use_senteces_from_title_filemp3;

        public bool Use_senteces_from_title_filemp3
        {
            get { return _use_senteces_from_title_filemp3; }
            set
            {
                if (_use_senteces_from_title_filemp3 == value) return;
                _use_senteces_from_title_filemp3 = value;
                OnPropertyChanged();
            }
        }

        private bool _rename_filemp3_with_sentece_titlefile;

        public bool Rename_filemp3_with_sentece_titlefile
        {
            get { return _rename_filemp3_with_sentece_titlefile; }
            set
            {
                if (_rename_filemp3_with_sentece_titlefile == value) return;
                _rename_filemp3_with_sentece_titlefile = value;
                OnPropertyChanged();
            }
        }

        private string _senteces;

        public string Sentences
        {
            get { return _senteces; }
            set { _senteces = value; }
        }
    }
}