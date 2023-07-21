using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneController : MonoBehaviour
{
    private bool canGoToNextScene;

    // Subscriptions
    Subscription<SpaceToContinueEvent> scene_to_continue_subscription;

    private void Start()
    {
        scene_to_continue_subscription = EventBus.Subscribe<SpaceToContinueEvent>(_OnSpaceToContinue);

        if (SceneManager.GetActiveScene().name == "Start")
        {
            canGoToNextScene = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (canGoToNextScene && Input.GetKeyDown(KeyCode.Space))
        {
            canGoToNextScene = false;

            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;

            //Debug.Log("NEXT SCENE: " + nextSceneIndex.ToString());

            EventBus.Publish<SceneTransitionRequest>(new SceneTransitionRequest(nextSceneIndex));
        }
    }

    void _OnSpaceToContinue(SpaceToContinueEvent s)
    {
        canGoToNextScene = true;
    }
}

public class SpaceToContinueEvent
{

    public SpaceToContinueEvent()
    {
    }
}
