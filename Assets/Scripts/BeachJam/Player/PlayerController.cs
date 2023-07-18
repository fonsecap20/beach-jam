using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed; //The starting velocity the player enters the scene with to accelerate from
    public float defaultSpeed; //The speed the player will be at when the game finishes the starting sequence - this is not the default, min, or max speed
    public float timeToMaxSpeed; //The amount of time in seconds it takes to reach endSpeed
    public AnimationCurve startSpeedCurve; //Curve the acceleration will follow

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BeginLevel());
    }

    IEnumerator BeginLevel()
    {
        //Add WaitForSeconds part here if we want to do anything before the game starts
        float elapsedTime = 0f; //Time the graph evaluation starts at
        while (elapsedTime < timeToMaxSpeed)
        {
            float currentSpeed = Mathf.Lerp(startSpeed, defaultSpeed, startSpeedCurve.Evaluate(elapsedTime / timeToMaxSpeed)); //Scales by taking the value every frame until the X value of the graph is 1 (complete)
            rb.velocity = Vector3.right * currentSpeed;
            elapsedTime += Time.deltaTime; //Keeping this as unscaled
            yield return null;
        }
        rb.velocity = Vector3.right * defaultSpeed; //This can probably be changed to Vector2
    }

    // Update is called once per frame
    void Update()
    {

    }
}
