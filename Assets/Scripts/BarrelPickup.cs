using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelPickup : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController controller = col.gameObject.GetComponent<PlayerController>();
            if (!controller.isRolling)
            {
                controller.hasBarrel = true;
                Destroy(gameObject);
            }
        }
    }
}
