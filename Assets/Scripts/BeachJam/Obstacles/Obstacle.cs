using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    
    public Vector3 startVelocity;
    public float rotationUpperBound;
    public float rotationLowerBound;
    public Sprite[] sprites;

    private ShipController shipController;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shipController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        rb.velocity = startVelocity;
        rb.angularVelocity = Random.Range(rotationLowerBound, rotationUpperBound);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision With " + gameObject.name);
            shipController.PlayerDeath();

            restartGame();
            // EventBus.Publish<>()
        }
    }
    public void restartGame()
    {
        EventBus.Publish<SceneTransitionRequest>(new SceneTransitionRequest(SceneManager.GetActiveScene().buildIndex));
    }
}
