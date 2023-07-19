using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
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
            Debug.Log(GetMagnitude());
        }
        Vector3 faceDirection = rb.velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButton(0))
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
            float currentPullForce = tetherPullForce * tetherPullCurve.Evaluate(tetherPullTimer / tetherPullTime);
            rb.AddForce(direction * currentPullForce);
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
}
