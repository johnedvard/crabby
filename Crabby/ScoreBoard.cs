using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

namespace Crabby
{
    public class ScoreBoard
    {
        private string scoreboard = "scoreboard";
        Dictionary<String, int> scores;
        public static int score = 0;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public ScoreBoard()
        {
            //retrieve the scoreboard if it exist
            if(settings.Contains(scoreboard)){
                scores = (Dictionary<string,int>)settings[scoreboard];
            }else{
                scores = new Dictionary<string,int>();
                settings.Add(scoreboard,scores);
            }
        }
        public int getScore(string name)
        {
            if (scores.ContainsKey(name))
            {
                return scores[name];
            }
            return 0;
        }
        public void saveScore(string name, int score)
        {
            if (score > getScore(name))
            {
                scores[name] = score;
                settings.Add(scoreboard, scores);
            }
            
        }
    }
}
