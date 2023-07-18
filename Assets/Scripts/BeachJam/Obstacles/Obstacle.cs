using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    
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
