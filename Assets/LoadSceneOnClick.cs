using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour
{
    public int newSceneIndex;

    public void LoadNewScene()
    {
        EventBus.Publish<SceneTransitionRequest>(new SceneTransitionRequest(newSceneIndex));
    }
}
