using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note - Backups are stored as comments in this script bc making different versions of the script with different names means changing it in other scripts too so...
public class ShipController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;
    public float initialBearing;

    [Header("Movement Settings")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    [Header("Death Settings")]
    public SpriteRenderer shipSprite;

    private ParticleSystem deathExplosion;
    private bool hasStarted;
    private bool isDead;
    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;
    private ShipSounds shipSounds;
    private float orbitDistance;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        deathExplosion = GetComponentInChildren<ParticleSystem>();
        shipSounds = GetComponent<ShipSounds>();
        lineRenderer.enabled = false;
        StartCoroutine(BeginLevel());
    }

    IEnumerator BeginLevel()
    {
        Vector2 initialDirection = new Vector2(Mathf.Cos(initialBearing * Mathf.Deg2Rad), Mathf.Sin(initialBearing * Mathf.Deg2Rad));
        float totalTime = timeToMaxSpeed;
        float timer = 0f;

        while (timer < totalTime)
        {
            float currentSpeed = Mathf.Lerp(startSpeed, defaultSpeed, startSpeedCurve.Evaluate(timer / totalTime));
            rb.velocity = initialDirection * currentSpeed;
            timer += Time.deltaTime;

            yield return null;
        }
        rb.velocity = initialDirection * defaultSpeed;
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !isDead)
        {
            HandleInputs();
            UpdateLineRenderer();
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        Vector3 faceDirection = rb.velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1) && tetheredObject != null)
            {
                orbitDistance = Vector2.Distance(transform.position, tetheredObject.transform.position);
            }
        }
        else if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
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
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                Vector2 tangent = Vector2.Perpendicular(direction).normalized;
                Vector2 tangentVel = Vector3.Project(rb.velocity, tangent);
                Vector2 perpVel = Vector3.Project(rb.velocity, direction);
                Vector2 currentPullForce = direction * ((rb.mass * Mathf.Pow(tangentVel.magnitude, 2)) / Vector2.Distance(tetheredObject.transform.position, transform.position)) - perpVel + Vector2.Perpendicular(perpVel);
                rb.AddForce(currentPullForce);
            }
            else if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                float currentPullForce = tetherPullForce * tetherPullCurve.Evaluate(tetherPullTimer / tetherPullTime);
                rb.AddForce(direction * currentPullForce);
            }
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

    public void PlayerDeath()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        isDead = true;
        deathExplosion.Play();
        shipSounds.PlayDeathSound();
        shipSprite.enabled = false;
    }
}


/*
Version 1
_______________________________________________________________________________________________________________________
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note - Backups are stored as comments in this script bc making different versions of the script with different names means changing it in other scripts too so...

public class ShipController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;
    public float initialBearing;

    //[Header("Movement Settings")]
    //public float minSpeed;
    //public float maxSpeed;

    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    [Header("Death Settings")]
    public SpriteRenderer shipSprite;
    public float spriteFadeTime;
    public float spriteFadeDelay;

    private ParticleSystem deathExplosion;
    private bool hasStarted;
    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;
    private ShipSounds shipSounds;
    

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        deathExplosion = GetComponentInChildren<ParticleSystem>();
        shipSounds = GetComponent<ShipSounds>();
        lineRenderer.enabled = false;
        StartCoroutine(BeginLevel());
    }

    IEnumerator BeginLevel()
    {
        // another sad say of using sin cos and tan
        Vector2 initialDirection = new Vector2(Mathf.Cos(initialBearing * Mathf.Deg2Rad), Mathf.Sin(initialBearing * Mathf.Deg2Rad));
        float totalTime = timeToMaxSpeed;
        float timer = 0f;

        while (timer < totalTime)
        {
            float currentSpeed = Mathf.Lerp(startSpeed, defaultSpeed, startSpeedCurve.Evaluate(timer / totalTime));
            rb.velocity = initialDirection * currentSpeed;
            timer += Time.deltaTime;

            yield return null;
        }
        rb.velocity = initialDirection * defaultSpeed;
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

    public float GetMaxTetherLength()
    {
        return tetherMaxLength;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public void PlayerDeath()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        deathExplosion.Play();
        shipSounds.PlayDeathSound();
        StartCoroutine(FadeOutSprite());
    }

    private IEnumerator FadeOutSprite()
    {
        yield return new WaitForSeconds(spriteFadeDelay);

        float fadeRate = 1f / spriteFadeTime;

        while (shipSprite.color.a > 0f)
        {
            float newAlpha = shipSprite.color.a - fadeRate * Time.deltaTime;
            shipSprite.color = new Color(shipSprite.color.r, shipSprite.color.g, shipSprite.color.b, newAlpha);

            yield return null;
        }
    }
}
_______________________________________________________________________________________________________________________

Version 2
_______________________________________________________________________________________________________________________
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note - Backups are stored as comments in this script bc making different versions of the script with different names means changing it in other scripts too so...
public class ShipController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;
    public float initialBearing;

    [Header("Movement Settings")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    [Header("Death Settings")]
    public SpriteRenderer shipSprite;

    private ParticleSystem deathExplosion;
    private bool hasStarted;
    private bool isDead;
    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;
    private ShipSounds shipSounds;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        deathExplosion = GetComponentInChildren<ParticleSystem>();
        shipSounds = GetComponent<ShipSounds>();
        lineRenderer.enabled = false;
        StartCoroutine(BeginLevel());
    }

    IEnumerator BeginLevel()
    {
        Vector2 initialDirection = new Vector2(Mathf.Cos(initialBearing * Mathf.Deg2Rad), Mathf.Sin(initialBearing * Mathf.Deg2Rad));
        float totalTime = timeToMaxSpeed;
        float timer = 0f;

        while (timer < totalTime)
        {
            float currentSpeed = Mathf.Lerp(startSpeed, defaultSpeed, startSpeedCurve.Evaluate(timer / totalTime));
            rb.velocity = initialDirection * currentSpeed;
            timer += Time.deltaTime;

            yield return null;
        }
        rb.velocity = initialDirection * defaultSpeed;
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !isDead)
        {
            HandleInputs();
            UpdateLineRenderer();
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        Vector3 faceDirection = rb.velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            if (tetheredObject == null)
            {
                TryToTether();
            }
        }
        else if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // Orbit logic here
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

    public float GetMaxTetherLength()
    {
        return tetherMaxLength;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public void PlayerDeath()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        isDead = true;
        deathExplosion.Play();
        shipSounds.PlayDeathSound();
        shipSprite.enabled = false;
    }
}
_______________________________________________________________________________________________________________________

Version 3
_______________________________________________________________________________________________________________________
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note - Backups are stored as comments in this script bc making different versions of the script with different names means changing it in other scripts too so...
public class ShipController : MonoBehaviour
{
    [Header("Starting Sequence Vars")]
    public float startSpeed;
    public float defaultSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve startSpeedCurve;
    public float initialBearing;

    [Header("Movement Settings")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Tether Settings")]
    public LayerMask tetherableLayer;
    public float tetherMaxLength;
    public float tetherPullForce;
    public float tetherPullTime;
    public AnimationCurve tetherPullCurve;

    [Header("Death Settings")]
    public SpriteRenderer shipSprite;

    private ParticleSystem deathExplosion;
    private bool hasStarted;
    private bool isDead;
    private Rigidbody2D rb;
    private GameObject tetheredObject;
    private float tetherPullTimer;
    private LineRenderer lineRenderer;
    private ShipSounds shipSounds;
    private float orbitDistance;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        deathExplosion = GetComponentInChildren<ParticleSystem>();
        shipSounds = GetComponent<ShipSounds>();
        lineRenderer.enabled = false;
        StartCoroutine(BeginLevel());
    }

    IEnumerator BeginLevel()
    {
        Vector2 initialDirection = new Vector2(Mathf.Cos(initialBearing * Mathf.Deg2Rad), Mathf.Sin(initialBearing * Mathf.Deg2Rad));
        float totalTime = timeToMaxSpeed;
        float timer = 0f;

        while (timer < totalTime)
        {
            float currentSpeed = Mathf.Lerp(startSpeed, defaultSpeed, startSpeedCurve.Evaluate(timer / totalTime));
            rb.velocity = initialDirection * currentSpeed;
            timer += Time.deltaTime;

            yield return null;
        }
        rb.velocity = initialDirection * defaultSpeed;
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !isDead)
        {
            HandleInputs();
            UpdateLineRenderer();
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        Vector3 faceDirection = rb.velocity;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDirection);
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1) && tetheredObject != null)
            {
                orbitDistance = Vector2.Distance(transform.position, tetheredObject.transform.position);
            }
        }
        else if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
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
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                Vector2 tangent = Vector2.Perpendicular(direction).normalized;
                Vector2 tangentVel = Vector3.Project(rb.velocity, tangent);
                Vector2 perpVel = Vector3.Project(rb.velocity, direction);
                Vector2 currentPullForce = direction * ((rb.mass * Mathf.Pow(tangentVel.magnitude, 2)) / Vector2.Distance(tetheredObject.transform.position, transform.position)) - perpVel + Vector2.Perpendicular(perpVel);
                rb.AddForce(currentPullForce);
            }
            else if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                float currentPullForce = tetherPullForce * tetherPullCurve.Evaluate(tetherPullTimer / tetherPullTime);
                rb.AddForce(direction * currentPullForce);
            }
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

    public void PlayerDeath()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        isDead = true;
        deathExplosion.Play();
        shipSounds.PlayDeathSound();
        shipSprite.enabled = false;
    }
}
_______________________________________________________________________________________________________________________
*/