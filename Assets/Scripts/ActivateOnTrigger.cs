using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ActivateOnTrigger : MonoBehaviour {
    //List of object to activate
    public GameObject[] ObjectsToActivate;
    //Deactivate objects instead
    public bool Deactivate = false;
    //Should it only trigger once?
    public bool TriggerOnce = true;
    //has been triggered
    private bool triggered = false;

	// Use this for initialization
	void Awake () {
        //Set collider to trigger if not done already
        GetComponent<BoxCollider>().isTrigger = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        //Check if other is the player
        if (other.tag == "Player")
        {
            //check if tragger once is true and not triggered already
            if (TriggerOnce && !triggered)
            {
                SetObjects();
                //set triggered to true
                triggered = true;
            }
            else if (!TriggerOnce) //else just check if trigger once is false
            {
                SetObjects();
            }
        }
    }

    void SetObjects()
    {
        //Loop through all the objects and set them to active or not
        for (int i = 0; i < ObjectsToActivate.Length; i++)
        {
            ObjectsToActivate[i].SetActive(!Deactivate);
        }
    }
}
