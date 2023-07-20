using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Auto Zoom Settings")]
    public float maxZoom;
    public float minZoom;
    public float speedZoomCoefficient; //How much magnitude effects zoom

    private float targetZoom;

    [Header("Player Follow Settings")]
    public Vector3 baseOffset; // This should only have the z value changed.
    public float maxLeadingDistance; // Max value for how far the camera can lead infront of the players current direction
    public float speedLeadingCoefficient; // How much the magnitude effects the distance the camera leads by
    public float smoothingSpeed; // How fast the camera lerps to its desired location

    private Transform playerTransform;
    private Camera playerCamera;
    private ShipController playerScript;

    private void Start()
    {
        playerCamera = GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        targetZoom = minZoom;
        transform.position = playerTransform.position + baseOffset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + baseOffset;
        Vector3 leadingOffset = playerScript.GetVelocity().normalized * Mathf.Clamp(playerScript.GetMagnitude() * speedLeadingCoefficient, 0, maxLeadingDistance);
        desiredPosition += leadingOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothingSpeed * Time.deltaTime); // Use Time.deltaTime instead of Time.unscaledDeltaTime
        CameraZoom();
    }


    private void CameraZoom()
    {
        targetZoom = playerScript.GetMagnitude() * speedZoomCoefficient;
        playerCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(playerCamera.orthographicSize, targetZoom, Time.deltaTime), minZoom, maxZoom);
    }
}
