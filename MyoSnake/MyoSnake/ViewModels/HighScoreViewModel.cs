using MyoSnake.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyoSnake.ViewModels
{
    public class HighScoreViewModel : NotificationBase<HighScore>
    {
        public HighScoreViewModel(HighScore score = null) : base(score) { }

        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public string Score
        {
            get { return This.Score; }
            set { SetProperty(This.Score, value, () => This.Score = value); }
        }
    }
}
