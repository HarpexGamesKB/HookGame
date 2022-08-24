using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyLine : MonoBehaviour
{
    [SerializeField] private float Power = 10f;
    [SerializeField] private PhysicMaterial ZeroFriction;
    //[SerializeField] private float timer;
    
    private void OnTriggerStay(Collider other)
    {
        /*
        timer+=Time.deltaTime;
        if (timer > 4f)
        {*/
            Brick brick = other.GetComponent<Brick>();
            if (brick)
            {
                //brick.Rigidbody.AddForce(Vector3.left*Power);
                brick.GetComponent<Collider>().material = ZeroFriction;
                brick.Rigidbody.velocity += Vector3.left * Power;
            }/*
            timer = 0;
        }*/
        
    }
}
