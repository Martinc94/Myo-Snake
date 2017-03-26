using MyoSnake.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSnake.ViewModels
{
    public class HighScoresViewModel : NotificationBase
    {
        private HighScoreModel highscoreModel = new HighScoreModel();

        ObservableCollection<HighScoreViewModel> _HighScores
           = new ObservableCollection<HighScoreViewModel>();

        public HighScoresViewModel()
        {
            init();

        } // Constructor

        public async Task init()
        {
            // clear collection
            _HighScores.Clear();

            // get 
            var highscore = await highscoreModel.getHighScore();

            // load 
            foreach (var score in highscore)
            {
                var hs = new HighScoreViewModel(score);
                hs.PropertyChanged += HighScore_OnNotifyPropertyChanged;
                _HighScores.Add(hs);
            } // foreach

        } // init

        public ObservableCollection<HighScoreViewModel> HighScores
        {
            get { return _HighScores; }
            set { SetProperty(ref _HighScores, value); }
        }


        void HighScore_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
