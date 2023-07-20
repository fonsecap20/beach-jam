using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector3 startVelocity = new Vector3(0, 0, 0);

    private void Start() {
        rb.velocity = startVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Crunch-Boom!");
            restartGame();       
            // EventBus.Publish<>()
        }
    }
    public void restartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
