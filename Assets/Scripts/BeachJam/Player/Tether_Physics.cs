using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether_Physics : MonoBehaviour
{
    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetMouseButtonUp(0))
        {
            tetheredObject = null;
            lineRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (tetheredObject != null)
        {
            Vector2 direction = (tetheredObject.transform.position - transform.position).normalized;
            float currentPullForce = tetherPullForce * tetherPullCurve.Evaluate(tetherPullTimer / tetherPullTime);
            rb.AddForce(direction * currentPullForce);
            tetherPullTimer += Time.fixedDeltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, tetheredObject.transform.position);
        }
    }
}
