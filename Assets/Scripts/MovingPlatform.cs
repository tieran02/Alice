using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //Points to move between
    public List<Vector3> Points;
    //How long should it take to reach a target?
    public float TimeToTarget = 2.0f;
    //the current point to head towards
    private int headingTorwards = 1;
    //is the platform moving in reverse?
    private bool reverse = false;
    private Vector3 lastPos;


    // Use this for initialization
    void Start ()
    {
        lastPos = Points[0];
    }

    // Update is called once per frame
    void Update ()
    {
        //get the target pos
        Vector3 target = Points[headingTorwards];
        //calculate speed
        float Speed = Vector3.Distance(target, lastPos) / TimeToTarget;
        //Move towrads targer
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        //if position is equal to target then goto next point
        if (transform.position == target)
            NextPoint();
	}

    void NextPoint()
    {
        lastPos = Points[headingTorwards];
        if (!reverse)
        {
            if (headingTorwards + 1 < Points.Count)
            {
                headingTorwards++;
            }
            else
            {
                reverse = true;
            }
        }
        else
        {
            if(headingTorwards > 0)
            {
                headingTorwards--;
            }
            else
            {
                reverse = false;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //set the player parent to the platform
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //set the player platfrom to the world (null)
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    void OnDrawGizmos()
    {
        foreach (var point in Points)
        {
            Gizmos.DrawSphere(point, .5f);
        }
    }
}
