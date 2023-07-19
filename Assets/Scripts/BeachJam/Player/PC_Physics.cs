using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Physics : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;

    [Header("Movement Settings")]
    public float minSpeed;
    public float maxSpeed;

    private bool hasStarted;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BeginLevel());     
    }

    IEnumerator BeginLevel()
    {
        //Accelerate ship to the left
        yield return null;
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            Debug.Log(GetMagnitude());
        }
    }

    public float GetMagnitude()
    {
        return rb.velocity.magnitude;
    }
}
