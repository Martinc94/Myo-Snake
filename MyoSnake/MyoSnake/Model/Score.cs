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
            //loads scores from server
            LoadGlobal();

            if (scoreList.Count != 0)
            {    
                await LoadData();
                //scoreList = new List<Score>();
                HighScores = scoreList;
            }
            else
            {
                await LoadData();
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
            Debug.WriteLine(scoreFile.Path);

            var jScoreList = JsonArray.Parse(Json);
            CreateList(jScoreList);
        }

        //get scores from Server and save to file
        public static async Task LoadGlobal()
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri("http://52.169.18.184:3000/api/getScores");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                //save score to file
                Save(httpResponseBody);


            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
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
                        case "name":
                            nScore.Name = value.GetString();
                            break;
                        case "score":
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

        public static async void Save(String jsonScoreArray)
        {
            //save json to txt file
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file;

            //get file 
            file = await storageFolder.CreateFileAsync("HighScores.txt", CreationCollisionOption.OpenIfExists);

            Debug.WriteLine("Response: " + jsonScoreArray);
            await FileIO.WriteTextAsync(file, jsonScoreArray);

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

