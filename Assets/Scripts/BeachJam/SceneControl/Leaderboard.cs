using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<Text> names;
    [SerializeField]
    private List<Text> scores;

    private string publicLeaderboardKey =
        "93ee5510ad6b31d1c878d3889c7f0a6470c94228e9c4c7287c1efe1b61139734";

    private void Start()
    {
        GetLeaderboard();
    }
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => { 
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.ResetPlayer();

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username,
            score, ((msg) => {
            GetLeaderboard();
        }));
    }
}
