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
    private PC_Physics playerScript;

    private void Start()
    {
        playerCamera = GetComponent<Camera>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PC_Physics>();
        targetZoom = minZoom;
    }

    private void Update()
    {
        CameraZoom();
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = desiredPosition;
    }

    private void CameraZoom()
    {
        targetZoom = playerScript.GetMagnitude() * speedZoomCoefficient;
        playerCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(playerCamera.orthographicSize, targetZoom, Time.deltaTime), minZoom, maxZoom);

    }
}