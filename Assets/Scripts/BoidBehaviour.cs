﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoidBehaviour : MonoBehaviour {

    private Material material;
    private Vector3 velocity;
    private Vector3 perceivedCenterOfMass;
    private Global stats;
    private List<Transform> closeObjects = new List<Transform>();

    public float ObjectDistance;

	private void Start ()
    {
        stats = GameObject.Find("Scripts").GetComponent<Global>();
        material = GetComponent<MeshRenderer>().material;
        material.color = Color.cyan;
	}
	
	private void FixedUpdate ()
    {
        ResetVelocity();
        FindCenter();

        //Boids rules
        GravitateTowardsCenter(); // Rule 1
        KeepDistance(); // Rule 2

        MoveObject();
	}

    private void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    private void GravitateTowardsCenter()
    {
        velocity += perceivedCenterOfMass - transform.position;
    }

    private void KeepDistance()
    {
        Vector3 c = Vector3.zero;

        foreach(Transform otherTransform in closeObjects)
        {
            c -= (otherTransform.position - this.transform.position) * 10;
        }

        velocity += c;
    }

    private void MoveObject()
    {
        GetComponent<Rigidbody>().AddForce(velocity);
    }

    private void FindCenter()
    {
        perceivedCenterOfMass = Vector3.zero;

        foreach (GameObject boidObject in stats.GetAllObjects())
        {
            if(boidObject != this.gameObject) //Find center of all boid objects that aren't this object
                perceivedCenterOfMass += boidObject.transform.position;
        }

        perceivedCenterOfMass /= stats.GetAllObjects().Count - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != this.transform && !closeObjects.Contains(other.transform) && !other.isTrigger)
        {
            closeObjects.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        closeObjects.Remove(other.transform);
    }
}
