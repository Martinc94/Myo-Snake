using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSnake.Data;

namespace MyoSnake.ViewModel
{
    public class PersonViewModel : NotificationBase<HighScore>
    {
        public PersonViewModel(HighScore person = null) : base(person) { }
        public String Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }
        public int Score
        {
            get { return This.Score; }
            set { SetProperty(This.Score, value, () => This.Score = value); }
        }
    }
}
