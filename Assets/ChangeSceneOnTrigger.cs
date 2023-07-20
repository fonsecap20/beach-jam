using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventBus.Publish<SceneTransitionRequest>(new SceneTransitionRequest(sceneIndex));
        }
    }
}
