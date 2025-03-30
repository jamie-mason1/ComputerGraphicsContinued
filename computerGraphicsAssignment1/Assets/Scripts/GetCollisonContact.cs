using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollisionContact : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Vector3 collisionCentre;
    [SerializeField] private float collisionRange;
    float vel = 0;
    [SerializeField] private float RedHotChange;
    bool collidingWithLava = false;

    private void Start()
    {
        collisionCentre = Vector3.zero;

    }

    private void Update()
    {
        if (mat != null)
        {
            if (mat.HasProperty("_VectorBounds"))
            {
                mat.SetVector("_VectorBounds", collisionCentre);
            }
            if (mat.HasProperty("_VectorRange"))
            {
                mat.SetFloat("_VectorRange", collisionRange);
            }
            
        }
    }
    private void FixedUpdate()
    {
        if (collidingWithLava)
        {
            collisionRange = Mathf.SmoothDamp(collisionRange, 0, ref vel, RedHotChange);
        }
        else
        {
            collisionRange = Mathf.SmoothDamp(collisionRange, 30, ref vel, RedHotChange);

        }
        collisionRange = Mathf.Clamp(collisionRange, 0, 30);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "LavaFloorDetect")
        {
            collidingWithLava = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "LavaFloorDetect")
        {
            // Calculate average contact point
            Vector3 averageContactPoint = Vector3.zero;

            foreach (ContactPoint contact in collision.contacts)
            {
                averageContactPoint += contact.point;

                // Update max range based on contact points
                float distance = Vector3.Distance(collisionCentre, contact.point);

            }

            averageContactPoint /= collision.contacts.Length;
            collisionCentre = averageContactPoint;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "LavaFloorDetect")
        {
            collidingWithLava = false;
        }
    }
}
