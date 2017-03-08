using MyoSnake.Data;
using MyoSnake.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyoSnake.ViewModel.Helpers;

namespace MyoSnake.ViewModel
{

    public class HighScoreVM : Helper.NotificationBase
    {
        Scores myScore;

        public HighScoreVM()
        {
            myScore = new Scores();
            _SelectedIndex = -1;
            // Load the database
            foreach (var myScore in myScore.HighScores)
            {
                var np = new HighScoreViewModel(myScore);
                np.PropertyChanged += Score_OnNotifyPropertyChanged;
                _Score.Add(np);
            }
        }

        ObservableCollection<HighScoreViewModel> _Score = new ObservableCollection<HighScoreViewModel>();
        public ObservableCollection<HighScoreViewModel> highScores
        {
            get { return _Score; }
            set { SetProperty(ref _Score, value); }
        }

        public String ScoreName
        {
            get { return myScore.name; }
        }

        int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (SetProperty(ref _SelectedIndex, value))
                { RaisePropertyChanged(nameof(Selected)); }
            }
        }

        public HighScoreViewModel Selected
        {
            get { return (_SelectedIndex >= 0) ? _Score[_SelectedIndex] : null; }
        }

        void Score_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
         
        }
    }

}
