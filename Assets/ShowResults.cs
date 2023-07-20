using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowResults : MonoBehaviour
{
    [SerializeField]
    private Text TimeOnLevel;
    [SerializeField]
    private Text TimeOverall;

    // Subscriptions
    Subscription<TimeCalculatedEvent> time_calculated_subscription;

    private void Start()
    {
        time_calculated_subscription = EventBus.Subscribe<TimeCalculatedEvent>(_OnTimeCalculated);
    }

    void _OnTimeCalculated(TimeCalculatedEvent t)
    {
        int minutes = Mathf.FloorToInt(t.timeOnLevel / 60f);
        int seconds = Mathf.FloorToInt(t.timeOnLevel % 60f);

        TimeOnLevel.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        minutes = Mathf.FloorToInt(t.timeOverall / 60f);
        seconds = Mathf.FloorToInt(t.timeOverall % 60f);

        TimeOverall.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

public class TimeCalculatedEvent
{
    public float timeOnLevel;
    public float timeOverall;

    public TimeCalculatedEvent(float _timeOnLevel, float _timeOverall)
    {
        timeOnLevel = _timeOnLevel;
        timeOverall = _timeOverall;
    }
}
