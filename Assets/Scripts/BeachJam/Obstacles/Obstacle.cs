using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector3 startVelocity = new Vector3(0, 0, 0);
    public float rotationUpperBound = 10;
    public float rotationLowerBound = -10;

    private void Start() {
        rb.velocity = startVelocity;
        rb.angularVelocity = Random.Range(rotationLowerBound, rotationUpperBound);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Crunch-Boom!");
            restartGame();       
            // EventBus.Publish<>()
        }
    }
    public void restartGame(){
        EventBus.Publish<SceneTransitionRequest>(new SceneTransitionRequest(SceneManager.GetActiveScene().buildIndex));
    }
}
