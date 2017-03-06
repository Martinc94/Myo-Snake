using MyoSnake.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyoSnake.ViewModel.Helpers;

namespace MyoSnake.ViewModel
{
   public class HighScoreViewModel : Helper.NotificationBase<Score>
    {
        public HighScoreViewModel(Score score = null) : base(score) { }
        public String Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }
        public String HighScore
        {
            get { return This.Highscore; }
            set { SetProperty(This.Highscore, value, () => This.Highscore = value); }
        }
    }
}
