using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker instance;

    private Text clockText;
    private float timer = 0f;
    private bool trackTime = false;

    // Subscriptions
    Subscription<SceneTransitionEvent> scene_transition_event_subscription;
    Subscription<SceneChangeEvent> scene_change_subscription;

    public void ResetTimer()
    {
        timer = 0;
    }

    private void Awake()
    {
        // Typical singleton initialization code.
        if (instance != null && instance != this)
        {
            // If there already exists a TimeTracker, we need to go away.
            Destroy(gameObject);
            return;
        }
        else
        {
            // If we are the first TimeTracker, we claim the "instance" variable so others go away.
            instance = this;
            DontDestroyOnLoad(gameObject); // Survive scene changes
        }
    }

    private void Start()
    {
        scene_transition_event_subscription = EventBus.Subscribe<SceneTransitionEvent>(_OnSceneTransition);
        scene_change_subscription = EventBus.Subscribe<SceneChangeEvent>(_OnSceneChange);

        clockText = transform.GetComponentInChildren<Text>();
        trackTime = true;
        ResetTimer();
    }

    private void Update()
    {
        if (trackTime)
        {
            timer += Time.deltaTime;
            UpdateClockText();
        }
    }

    private void UpdateClockText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void _OnSceneTransition(SceneTransitionEvent s)
    {
        trackTime = false;

        if (s.newSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            EventBus.Publish<LevelFinishedEvent>(new LevelFinishedEvent(timer));

            ResetTimer();
        }

        if (s.newSceneIndex == 0)
        {
            Destroy(gameObject);
        }
    }

    void _OnSceneChange(SceneChangeEvent s)
    {
        trackTime = true;
    }
}
