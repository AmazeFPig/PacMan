using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // TeleportPosition that when get out of this portal
    public Transform TeleportPosition;
    [SerializeField]
    private Portal otherPortal;

    private void Start()
    {
        if (TeleportPosition == null)
        {
            TeleportPosition = transform.GetChild(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (otherPortal != null && otherPortal.TeleportPosition != null)
        {
            collision.GetComponent<GridMovement>().TeleportTo(otherPortal.TeleportPosition.position);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: OtherPortal or its teleportPosion Missing! ");
        }
    }
}
