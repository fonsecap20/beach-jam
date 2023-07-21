using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFinalResult : MonoBehaviour
{
    private float finalTime;

    void Start()
    {
        finalTime = PlayerStats.instance.GetTotalTime();

        int minutes = Mathf.FloorToInt(finalTime / 60f);
        int seconds = Mathf.FloorToInt(finalTime % 60f);

        GetComponent<Text>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
