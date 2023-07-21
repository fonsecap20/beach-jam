using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator GoalTransition;
    [SerializeField]
    private Animator DeathTransition;

    [SerializeField]
    private float goalTransitionTime;
    [SerializeField]
    private float deathTransitionTime;

    private bool isTransitioning = false;

    // Subscriptions
    Subscription<SceneTransitionRequest> scene_transition_request_subscription;

    private void Start()
    {
        scene_transition_request_subscription = EventBus.Subscribe<SceneTransitionRequest>(_OnSceneTransitionRequest);
    }

    void _OnSceneTransitionRequest(SceneTransitionRequest s)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;

            EventBus.Publish<SceneTransitionEvent>(new SceneTransitionEvent(s.sceneIndex));


            StartCoroutine(LoadLevel(s.sceneIndex));
        }

    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        string currScene = SceneManager.GetActiveScene().name;
        string scene = SceneManager.GetSceneByBuildIndex(sceneIndex).name;

        DeathTransition.gameObject.SetActive(false);
        GoalTransition.gameObject.SetActive(false);

        // If we are entering a new scene run either the basic or goal animation.
        if (scene != currScene)
        {

            if (currScene == "Start" || scene == "Start")
            {
                DeathTransition.gameObject.SetActive(true);
                DeathTransition.SetTrigger("Start");

                if (PlayerStats.instance)
                {
                    PlayerStats.instance.ResetTimes();
                }


                yield return new WaitForSeconds(deathTransitionTime);
            }
            else
            {
                // Play Goal Animation
                GoalTransition.gameObject.SetActive(true);
                GoalTransition.SetTrigger("Start");

                yield return new WaitForSeconds(goalTransitionTime);

                //Camera.main.gameObject.GetComponent<AudioSource>().clip = Resources.Load("heroTimeup") as AudioClip;
                //Camera.main.gameObject.GetComponent<AudioSource>().Play();

                Time.timeScale = 0;

                while (!Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                }

                Time.timeScale = 1;

                TimeTracker.instance.ResetTimer();
            }
        }
        else
        {
            //Otherwise, the player must have died so play the death animation.
            DeathTransition.gameObject.SetActive(true);

            // Play Death Animation
            DeathTransition.SetTrigger("Start");
            yield return new WaitForSeconds(deathTransitionTime);
        }

        isTransitioning = false;
        EventBus.Publish<SceneChangeEvent>(new SceneChangeEvent());
        SceneManager.LoadScene(sceneIndex);
    }
}

public class SceneTransitionRequest
{
    public int sceneIndex;

    public SceneTransitionRequest(int _sceneIndex)
    {
        sceneIndex = _sceneIndex;
    }
}

public class SceneChangeEvent
{
    public SceneChangeEvent()
    {
    }
}

public class SceneTransitionEvent
{
    public int newSceneIndex;

    public SceneTransitionEvent(int _newSceneIndex)
    {
        newSceneIndex = _newSceneIndex;
    }
}
