using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position,new Vector3(Target.position.x+3.9f,transform.position.y,transform.position.z) , Time.deltaTime*0.2f);
    }
}
