using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private Text inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;
    public void SubmitScore()
    {
        int scoreInSeconds = CalculateScore();
        submitScoreEvent.Invoke(inputName.text, scoreInSeconds);
    }

    private int CalculateScore()
    {
        string score = inputScore.text;
        int minutes = int.Parse(score[0].ToString() + score[1].ToString());
        int seconds = int.Parse(score[3].ToString() + score[4].ToString()) + (minutes * 60);

        return seconds;
    }
}
