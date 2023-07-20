using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker instance;

    private Text clockText;
    private float timer = 0f;

    // Subscriptions
    Subscription<SceneTransitionRequest> scene_transition_request_subscription;

    public void ResetTimer()
    {
        scene_transition_request_subscription = EventBus.Subscribe<SceneTransitionRequest>(_OnSceneTransitionRequest);
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
        clockText = transform.GetComponentInChildren<Text>();
        ResetTimer();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        UpdateClockText();
    }

    private void UpdateClockText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void _OnSceneTransitionRequest(SceneTransitionRequest s)
    {
        EventBus.Publish<LevelFinishedEvent>(new LevelFinishedEvent(timer));

        if (s.sceneIndex == 0)
        {
            Destroy(gameObject);
        }
    }
}
