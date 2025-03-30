using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPosition : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    TrailRenderer trail;
    Vector3 lastPosition;
    Vector3 currentVelocity;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        transform.position = followObject.transform.position;
        lastPosition = followObject.transform.position;

    }
    private void Update()
    {
        currentVelocity = (followObject.transform.position - lastPosition) / Time.deltaTime;
        if(followObject.GetComponent<Rigidbody>() != null && followObject.GetComponent<Rigidbody>().velocity.magnitude >0.3f){
            trail.emitting = true;
        }
        else if(currentVelocity.magnitude > 0.3f){
            trail.emitting = true;
        }
        else{
            trail.emitting = false;
        }
        transform.position = followObject.transform.position;

        lastPosition = followObject.transform.position;

    }

    
}
