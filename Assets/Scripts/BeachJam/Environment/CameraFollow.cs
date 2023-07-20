using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float maxZoom;
    public float minZoom;
    public float speedZoomCoefficient;
    private float targetZoom;
    public Transform playerTransform;
    public Vector3 offset;
    private Camera playerCamera;
    private ShipController playerScript;

    private void Start()
    {
        playerCamera = GetComponent<Camera>();
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