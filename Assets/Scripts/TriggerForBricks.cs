using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForBricks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody contactBody = other.GetComponent<Rigidbody>();
        if(contactBody)
        {
            contactBody.freezeRotation = false;
            contactBody.drag = 5f;
        }
    }
}
