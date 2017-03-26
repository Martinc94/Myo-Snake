using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyoSnake.Data
{
    public class HighScore
    {
        public String Name { get; set; }
        public int Score { get; set; }
    }

    public class ScoreService
    {
        public static String Name = "Score Service.";

        public static List<HighScore> GetScore()
        {
            return new List<HighScore>()
                {
                    new HighScore() { Name="Chris Cole", Score=10 },
                    new HighScore() { Name="Kelly Kale", Score=32 },
                    new HighScore() { Name="Dylan Durbin", Score=18 }

                    //read from file and make new highscores
                };
        }

        public static void Write(HighScore score)
        {
            
        }

        public static void Delete(HighScore score)
        {
           
        }
    }
}
