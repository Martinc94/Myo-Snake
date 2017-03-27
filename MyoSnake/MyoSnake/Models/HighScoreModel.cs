using MyoSnake.Data;
using MyoSnake.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace MyoSnake.Models
{
    public class HighScoreModel
    {
        public List<HighScore> highscores = new List<HighScore>();

        public HighScoreModel()
        {
            init();
        } // Constructor

        private async Task init()
        {
            // clear list
            highscores.Clear();

            // get scores from Server
            await getScores();
        }

        public async Task<List<HighScore>> getHighScore()
        {
            // get scores from Server
            await getScores();
            return highscores;
        }

        // get scores from Server
        public async Task getScores()
        {
            List<HighScore> tempScores = new List<HighScore>();

            try
            {
                var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                httpFilter.CacheControl.ReadBehavior =
                    Windows.Web.Http.Filters.HttpCacheReadBehavior.MostRecent;

                //Create an HTTP client object
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient(httpFilter);

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

                    var jList = JsonArray.Parse(httpResponseBody);

                    foreach (var item in jList)
                    {
                        var oneScore = item.GetObject();
                        HighScore nScore = new HighScore();

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
                                    nScore.Score = value.GetString();
                                    break;

                            } // end switch
                        } // end foreach
                        tempScores.Add(nScore);
                    } // end foreach 


                }
                catch (Exception e)
                {
                    //error
                }


                highscores = tempScores;

            }
            catch (Exception e)
            {
                //error
            } 

        } // getScores

    }
}
