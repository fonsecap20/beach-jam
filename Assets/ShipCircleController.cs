using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCircleController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;

    [Header("Movement Settings")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    private bool hasStarted;
    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
            HandleInputs();
            UpdateLineRenderer();
            //Debug.Log(GetMagnitude());
        }
        Vector3 faceDirection = rb.velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButton(1))
        {
            if (tetheredObject == null)
            {
                TryToTether();
            }
        }
        else
        {
            Untether();
        }
        if(Input.GetKey(KeyCode.Space)){
            rb.AddForce(transform.up * 1);
        }
    }

    private void TryToTether()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePosition - transform.position, tetherMaxLength, tetherableLayer);

        if (hit.collider != null)
        {
            tetheredObject = hit.collider.gameObject;
            tetherPullTimer = 0f;
            lineRenderer.enabled = true;
        }
    }

    private void Untether()
    {
        tetheredObject = null;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (tetheredObject != null)
        {
            Vector2 direction = (tetheredObject.transform.position - transform.position).normalized;
            Vector2 tangent = Vector2.Perpendicular(direction).normalized;
            Vector2 tangentVel = Vector3.Project(rb.velocity, tangent);
            Vector2 perpVel = Vector3.Project(rb.velocity, direction);
            // float currentPullForce = tetherPullForce * tetherPullCurve.Evaluate(tetherPullTimer / tetherPullTime);
            Vector2 currentPullForce = direction * ((rb.mass * Mathf.Pow(tangentVel.magnitude, 2) ) / Vector2.Distance(tetheredObject.transform.position, transform.position)) - perpVel;
            rb.AddForce(currentPullForce);
            tetherPullTimer += Time.fixedDeltaTime;
        }
    }

    private void UpdateLineRenderer()
    {
        if (tetheredObject != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, tetheredObject.transform.position);
        }
    }

    public float GetMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public float GetMaxTetherLength()
    {
        return tetherMaxLength;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    
}
