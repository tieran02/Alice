using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FaceCamera))]
public class Portal : MonoBehaviour
{
    //Always face player
    public bool Billboard = true;
    //Destination portal
    public Portal Destination;

    void Awake()
    {
        //make portal face camera
        if (Billboard)
            GetComponent<FaceCamera>().enabled = true;
        else
            GetComponent<FaceCamera>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //check if destination is not null and if it isn't teleport the player
        if(Destination != null)
        {
            other.transform.position  = Destination.transform.position + Destination.transform.forward * 2.0f;
            other.transform.rotation = Destination.transform.rotation;
        }
    }
}
