using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float maxZoom;
    public float minZoom;
    public float speedZoomCoefficient;
    private float targetZoom;
    public Vector3 offset; // as long as the Z value is under 0 it can be litteraly anything but keep at 0,0,-1 for ease of use

    private Transform playerTransform;
    private Camera playerCamera;
    private ShipController playerScript;

    private void Start()
    {
        playerCamera = GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        targetZoom = minZoom;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = desiredPosition;
        CameraZoom();
    }

    private void CameraZoom()
    {
        targetZoom = playerScript.GetMagnitude() * speedZoomCoefficient;
        playerCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(playerCamera.orthographicSize, targetZoom, Time.deltaTime), minZoom, maxZoom);
    }
}