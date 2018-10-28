using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class AIMount : MonoBehaviour {
    //Targets for the Mount to head towards
    public Transform[] Targets;
    //The position where the player will sit
    public Transform SeatPos;
    //Mount animation
    public Animator MountAnimation;

    //Player currently on the mount
    private Transform player;
    //is the mount currently travling
    private bool travling;
    //navmesh agent of the mount
    private NavMeshAgent agent;
    //current target to head towards
    private int currentTarget = 0;

    void Awake()
    {
        //set the navmesh agent and make sphere collider a trigger if not already
        agent = GetComponent<NavMeshAgent>();
        GetComponent<SphereCollider>().isTrigger = true;
    }

	
	// Update is called once per frame
	void Update ()
    {
        //If the player is not null and the mount is currently travling set the player positon to the seat pos
        if(player != null && travling)
            player.position = SeatPos.position;

        //Check if the mount has another target
        if(player != null && currentTarget + 1 < Targets.Length && travling)
        {
            //Get the distacne to the next target and if it is less than 10 set the current target to the next one
            float distanceToNextTarget = Vector3.Distance(Targets[currentTarget].position, transform.position);
            if(distanceToNextTarget < 10 )
            {
                currentTarget++;
                agent.destination = Targets[currentTarget].position;
            }
        }
        //check if the mount is within 1 unit of the final positon and if so set travling to false
        float distanceToDestination = Vector3.Distance(transform.position, Targets[Targets.Length - 1].position);
        if (distanceToDestination <= 1.0f)
            travling = false;
        //Set the running animation to what ever travling is
        MountAnimation.SetBool("Running", travling);
    }

    void OnTriggerStay(Collider other)
    {
        //check player presses E and is withing trigger and mount is not travling
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player" && !travling)
        {
            //if player is null then mount the AI mount
            if (player == null)
            {
                player = other.transform;
                player.position = SeatPos.position;
                agent.destination = Targets[currentTarget].position;
                travling = true;
                GetComponent<TooltipOnTigger>().enabled = false;
                other.GetComponent<AudioSource>().enabled = false;
            }
            else //dismount
            {
                player = null;
                GetComponent<TooltipOnTigger>().enabled = true;
                other.GetComponent<AudioSource>().enabled = true;
            }
        }
    }
}
