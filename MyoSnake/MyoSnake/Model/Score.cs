using MyoSnake.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace MyoSnake.Model
{
    public class Scores
    {
        public static List<Score> scoreList = new List<Score>();
        public List<Score> HighScores { get; set; }
        public string name { get; set; }

        public Scores()
        {
            HighScores = scoreList;
            init(HighScores);
        }

        public static async Task init(List<Score> HighScores)
        {
            if (scoreList.Count != 0)
            {
                scoreList = new List<Score>();
                LoadData();
                HighScores = scoreList;
            }
            else
            {
                LoadData();
                HighScores = scoreList;
            }
        }

        public static async Task LoadData()
        {
            await LoadLocal();
        }

        public static async Task LoadLocal()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile scoreFile;

            //get
            scoreFile = await storageFolder.CreateFileAsync("HighScores.txt", CreationCollisionOption.OpenIfExists);
            string Json = await Windows.Storage.FileIO.ReadTextAsync(scoreFile);

            //find localStorage
            //Debug.WriteLine(scoreFile.Path);

            var jScoreList = JsonArray.Parse(Json);
            CreateList(jScoreList);
        }

        public static async Task LoadGlobal()
        {
            //get scores from Server and save to file














            /*StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile scoreFile;

            //get
            scoreFile = await storageFolder.CreateFileAsync("HighScores.txt", CreationCollisionOption.OpenIfExists);
            string Json = await Windows.Storage.FileIO.ReadTextAsync(scoreFile);

            //find localStorage
            //Debug.WriteLine(scoreFile.Path);

            var jScoreList = JsonArray.Parse(Json);
            CreateList(jScoreList);*/
        }

        //adds JsonArray to a List
        private static void CreateList(JsonArray jList)
        {
            foreach (var item in jList)
            {
                var oneScore = item.GetObject();
                Score nScore = new Score();

                foreach (var key in oneScore.Keys)
                {
                    IJsonValue value;
                    if (!oneScore.TryGetValue(key, out value))
                        continue;

                    switch (key)
                    {
                        case "Name":
                            nScore.Name = value.GetString();
                            break;
                        case "Highscore":
                            nScore.Highscore = value.GetString();
                            break;
                        
                    } // end switch
                } // end foreach
                scoreList.Add(nScore);
            } // end foreach 
        }//end CreateList

        public void Add(Score hs)
        {
            if (!HighScores.Contains(hs))
            {
                HighScores.Add(hs);
            }
        }//end add

        public void Delete(Score hs)
        {
            if (HighScores.Contains(hs))
            {
                HighScores.Remove(hs);
            }
        }//end delete

        public void Update(Score hs)
        {

        }//end update

        public async void Save(Score hs)
        {

        }//end add

        public async void SaveAll()
        {
            Debug.WriteLine("[IN SaveAll]: ");
            await saveScores();
        }//end saveAll

        //saves To Local Storage
         public async Task saveScores()
         {
             StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
             StorageFile file;

             //get file 
             file = await storageFolder.CreateFileAsync("HighScores.txt", CreationCollisionOption.OpenIfExists);

             string output;

             StringBuilder sb = new StringBuilder();
             //start of array
             sb.Append("[");

             int count = 0;
             foreach (var item in HighScores)
             {
                 if (count == 0)
                 {
                     output = JsonConvert.SerializeObject(item);
                     sb.Append(output);

                 }
                 //, needed when not first in array
                 else
                 {
                     output = JsonConvert.SerializeObject(item);
                     sb.Append("," + output);
                 }//end else

                 count++;

             }//end foreach

             sb.Append("]");

             //Debug.WriteLine("[Serialized Object]: " + sb.ToString());

             //save to file 
             await FileIO.WriteTextAsync(file, sb.ToString());

         }//end saveScores

    }//end Scores
}

