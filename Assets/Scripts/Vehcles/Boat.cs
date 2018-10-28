using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Boat : MonoBehaviour
{
    //Speed of the boat
    public float Speed = 5f;
    //the position the player will sit at
    public Transform SeatPos;
    //What item is required to ride if any
    public Item RequiredItem;

    private Transform player;
    private Rigidbody rb;

	// Use this for initialization
	void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<SphereCollider>().isTrigger = true;
	}

    void LateUpdate()
    {
        //if player is not on boat then return
        if (player == null)
            return;
        //else set the players pos to the seat pos
        player.position = SeatPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player is not on boat then return
        if (player == null)
            return;

        //Add force to the boat depending on what direction the user wants to go
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * Speed, ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * Speed, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(transform.up, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(-transform.up, ForceMode.Acceleration);
        }

    }

    void OnTriggerStay(Collider other)
    {
        //check if other is the player and presses E
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {
            if (!other.GetComponent<Inventory>().HasItem(RequiredItem.Name))
            {
                AlertManager.GetInstance().AddAlert("You need some paddles to use the boat");
                return; //does not have the required item to use the boat
            }

            //mount 
            if (player == null)
            {
                player = other.transform;
            }
            else //dismount
            {
                player = null;
            }
        }
    }

}
