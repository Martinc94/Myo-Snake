using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSnake.Data;

namespace MyoSnake.Model
{
    public class Organization
    {
        public List<HighScore> People { get; set; }
        public String Name { get; set; }

        public Organization(String databaseName)
        {
            Name = databaseName;
            People = ScoreService.GetScore();
        }

        public void Add(HighScore person)
        {
            if (!People.Contains(person))
            {
                People.Add(person);
                ScoreService.Write(person);
            }
        }

        public void Delete(HighScore person)
        {
            if (People.Contains(person))
            {
                People.Remove(person);
                ScoreService.Delete(person);
            }
        }

        public void Update(HighScore person)
        {
            ScoreService.Write(person);
        }
    }
}