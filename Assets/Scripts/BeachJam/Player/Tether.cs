using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    [Tooltip("Only tethers to objects on these layers.")]
    public LayerMask anchorLayers;
    public float maxLength = 5f; // The maximum length of the tether
    public float releaseTime;


    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private bool tetherIsEjected;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        tetherIsEjected = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !tetherIsEjected)
        {
            // EjectTether();
            AtatchClosest();
        }

    }

    Vector3 GetCursorDirection()
    {
        // Get the mouse position on the screen
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the camera to the mouse position
        Vector3 direction = (worldMousePosition - mainCamera.transform.position).normalized;

        return direction;
    }

    void EjectTether()
    {
        tetherIsEjected = true;

        Vector3 cursorDirection = GetCursorDirection();
        Vector3 anchorPosition = transform.position + (cursorDirection * maxLength);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, anchorPosition);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, cursorDirection, maxLength, anchorLayers);

        // Check if the raycast hit an AnchorPoint.
        if (hit.collider != null)
        {
            Debug.Log("Raycast hit an AnchorPoint!");

            StartCoroutine(AnchorPlayer(hit.collider.gameObject.transform.position));
        }
        else
        {
            Debug.Log("Raycast DID NOT hit an AnchorPoint.");
            StartCoroutine(RetractTether());
        }
    }

    void AtatchClosest(){
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("Anchor");
        float minDist = float.PositiveInfinity;
        GameObject anchorPoint = null;
        foreach(GameObject anchor in anchors){
            float dist = Vector3.Distance(gameObject.transform.position, anchor.transform.position);
            if(dist < maxLength && dist < minDist){
                minDist = dist;
                anchorPoint = anchor;
            }
        }
        if(anchorPoint!=null){
            StartCoroutine(AnchorPlayer(anchorPoint.transform.position));
        }else{
            Debug.Log("No Anchor In Range");
        }
    }

    IEnumerator AnchorPlayer(Vector3 anchorPoint)
    {
        lineRenderer.SetPosition(1, anchorPoint);

        float radius = Vector2.Distance(transform.position, anchorPoint);
        float rotationSpeed = GetComponent<PlayerController>().defaultSpeed / 2f;
        float currentAngle = FindAngle(anchorPoint);
        
        // Calculate the rotation direction using cross product
        Vector3 anchorToPlayer = transform.position - anchorPoint;
        Vector3 tangent = Vector3.Cross(Vector3.forward, anchorToPlayer);
        bool counterClock = Vector3.SignedAngle(anchorToPlayer, gameObject.transform.right, Vector3.forward) > 0;
            
        while (Input.GetMouseButton(0))
        {
            // Calculate the new position using trigonometry
            float x = anchorPoint.x + radius * Mathf.Cos(currentAngle);
            float y = anchorPoint.y + radius * Mathf.Sin(currentAngle);
            Vector3 newPosition = new Vector3(x, y);

            // Assign the new position to the object's transform
            transform.position = newPosition;

            anchorToPlayer = transform.position - anchorPoint;
            // Set the rotation of the object to face the direction
            

            // Adjust the tether to move with the player.
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, anchorPoint);

            // Increment the angle based on rotation speed and time
            if(counterClock){
                transform.rotation = Quaternion.LookRotation(Vector3.forward, -anchorToPlayer);
                currentAngle += rotationSpeed * Time.deltaTime;
            }else{
                transform.rotation = Quaternion.LookRotation(Vector3.forward, anchorToPlayer);
                currentAngle -= rotationSpeed * Time.deltaTime;
            }


            yield return null;
        }

        // Ensure the tether has full retracted.
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        // Send player off in direction they were facing.
        GetComponent<Rigidbody2D>().velocity = transform.right * GetComponent<PlayerController>().defaultSpeed;

        tetherIsEjected = false;
    }

    
    IEnumerator RetractTether()
    {
        Vector2 endPosition = lineRenderer.GetPosition(1);
        float elapsedTime = 0;
        float t = Mathf.Clamp01(elapsedTime / releaseTime);

        while (t < 1f)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor
            t = Mathf.Clamp01(elapsedTime / releaseTime);

            // Interpolate between the end of the tether and the player.
            Vector2 newPosition = Vector2.Lerp(endPosition, transform.position, t);

            // Assign the new position to the tether's end point.
            lineRenderer.SetPosition(1, newPosition);

            yield return null;
        }

        // Ensure the tether has full retracted.
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        tetherIsEjected = false;
    }

    float FindAngle(Vector3 centerPoint)
    {
        // Calculate the direction vector from the center point to the GameObject
        Vector2 directionToGameObject = transform.position - centerPoint;

        // Calculate the initial angle based on the direction vector
        return Mathf.Atan2(directionToGameObject.y, directionToGameObject.x);
    }
}
