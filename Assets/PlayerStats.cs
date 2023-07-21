using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    private float currentLevelTime = 0f;
    private float totalTime = 0f;

    // Subscriptions
    Subscription<LevelFinishedEvent> level_finished_subscription;

    public void ResetTimes()
    {
        currentLevelTime = 0f;
        totalTime = 0f;
    }

    private void Awake()
    {
        // Typical singleton initialization code.
        if (instance != null && instance != this)
        {
            // If there already exists PlayerStats, we need to go away.
            Destroy(gameObject);
            return;
        }
        else
        {
            // If we are the first PlayerStats, we claim the "instance" variable so others go away.
            instance = this;
            DontDestroyOnLoad(gameObject); // Survive scene changes
        }
    }

    private void Start()
    {
        level_finished_subscription = EventBus.Subscribe<LevelFinishedEvent>(_OnLevelFinish);
        ResetTimes();
    }

    void _OnLevelFinish(LevelFinishedEvent l)
    {
        currentLevelTime = l.timeTaken;
        totalTime += l.timeTaken;

        EventBus.Publish<TimeCalculatedEvent>(new TimeCalculatedEvent(currentLevelTime, totalTime));
    }
}

public class LevelFinishedEvent
{
    public float timeTaken;

    public LevelFinishedEvent(float _timeTaken)
    {
        timeTaken = _timeTaken;
    }
}
