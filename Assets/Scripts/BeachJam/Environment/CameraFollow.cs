using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        transform.position = desiredPosition;
    }
}
