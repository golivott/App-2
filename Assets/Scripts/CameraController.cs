using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraDistance = -2;
    public float lerpFactor;
    public float verticalOffset = 0.5f;
    public float horizontalOffset = 0f;
    public float horizontalOffsetDistance = 1f;
    public LayerMask cameraBoundsMask;
    public Vector3 targetCameraOffset;
    public Vector3 currCameraOffset;
    
    private Vector3 cameraPos;
    private Camera cameraComp;

    private void Start()
    {
        cameraComp = GetComponent<Camera>();
        targetCameraOffset = new Vector3(0, verticalOffset, cameraDistance);
        currCameraOffset = targetCameraOffset;
    }

    private void Update()
    {
        float horizontalMovement = player.GetComponent<PlayerController>().horizontalInput;

        if (horizontalMovement > 0)
        {
            horizontalOffset = 1;
        }
        else if (horizontalMovement < 0)
        {
            horizontalOffset = 0;
        }
        
        targetCameraOffset = new Vector3(horizontalOffset * horizontalOffsetDistance, verticalOffset, cameraDistance);
        currCameraOffset = Vector3.Lerp(currCameraOffset, targetCameraOffset, lerpFactor);
        cameraPos = player.transform.position + currCameraOffset;
        
        // If we can find a cameraBounds object apply the boundary
        if (GetCameraBounds() != null)
        {
            Bounds worldBounds = GetCameraBounds().bounds;
            float height = Mathf.Tan(Mathf.Deg2Rad * cameraComp.fieldOfView / 2f) * Mathf.Abs(cameraDistance) * 2;
            float width = cameraComp.aspect * height;

            float minX = worldBounds.min.x + width/2f;
            float maxX = worldBounds.max.x - width/2f;
            float minY = worldBounds.min.y + height/2f;
            float maxY = worldBounds.max.y - height/2f;
            Bounds cameraBounds = new Bounds();
            cameraBounds.SetMinMax(new Vector3(minX,minY,0f),new Vector3(maxX,maxY,0f));
            cameraPos = new Vector3(
                Mathf.Clamp(cameraPos.x, cameraBounds.min.x, cameraBounds.max.x),
                Mathf.Clamp(cameraPos.y, cameraBounds.min.y, cameraBounds.max.y),
                cameraPos.z
            );
        }
        
        transform.position = cameraPos;
    }

    private Collider GetCameraBounds()
    {
        Physics.Raycast(player.transform.position + Vector3.back, Vector3.forward, out RaycastHit hitInfo, Mathf.Abs(cameraDistance), cameraBoundsMask);
        return hitInfo.collider;
    }
}
