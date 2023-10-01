using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public bool showPlatforms = false;
    public bool addPlatformEdges = false;
    
    public GameObject floorEdge;
    public GameObject platformEdge;

    private SpriteRenderer[] childrenRenderers;

    private void Start()
    {
        childrenRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in childrenRenderers)
        {
            r.enabled = true;
            
            for (int i = 0; i < r.gameObject.transform.childCount; i++)
            {
                Transform child = r.gameObject.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        
        if (addPlatformEdges)
        {
            foreach (SpriteRenderer r in childrenRenderers)
            {
                GameObject edge = r.gameObject.GetComponent<PlatformEffector2D>() != null ? platformEdge : floorEdge;
                float oldAngle = r.gameObject.transform.eulerAngles.z;
                r.gameObject.transform.rotation = Quaternion.Euler(0f,0f,0f);
                
                GameObject leftEdge = Instantiate(edge, r.gameObject.transform);
                GameObject rightEdge = Instantiate(edge, r.gameObject.transform);
                leftEdge.transform.position = r.gameObject.transform.position;
                rightEdge.transform.position = r.gameObject.transform.position;
                float length = r.gameObject.transform.localScale.x;
                float edgeTrans = edge.transform.localScale.x * length;
                float xTrans = length / 2f + edgeTrans / 2f;
                leftEdge.transform.Translate(-xTrans,0,0);
                rightEdge.transform.Translate(xTrans,0,0);
                leftEdge.gameObject.GetComponent<SpriteRenderer>().enabled = showPlatforms;
                rightEdge.gameObject.GetComponent<SpriteRenderer>().enabled = showPlatforms;
                
                Vector3 scale = r.gameObject.transform.localScale;
                r.gameObject.transform.localScale = new Vector3(scale.x - 2 * edge.transform.localScale.x * length,scale.y,scale.z);
                r.gameObject.transform.rotation = Quaternion.Euler(0f,0f,oldAngle);
            }
        }

        if (!showPlatforms)
        {
            foreach (SpriteRenderer r in childrenRenderers)
            {
                r.enabled = false;
            }
        }
    }
}
