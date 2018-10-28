using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    //Force field object
    public GameObject ForceFieldObject;
    //How long the shield would last
    public float Uptime = 5f;
    //How long it takes the shield to recharge
    public float RechargeTime = 20f;
    //Is the shield currently on cooldown?
    public bool Cooldown { get; private set; }
    //current time since till recharge
    private float currentTime;
    //is the shield active?
    public bool Active { get; private set; }

	// Use this for initialization
	void Awake () {
        currentTime = 0;
        Active = false;
        ForceFieldObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //cooldown for forcefield to recharge
        if (Active)
        {
            if(currentTime > Uptime)
            {
                //shield has exceeded the uptime and is now disabled
                Active = false;
                ForceFieldObject.SetActive(false);
            }
        }


        if (Cooldown)
        {
            if (currentTime > RechargeTime)
            {
                //shield has exceeded the recharge and is now recharged
                Cooldown = false;
                currentTime = 0;
            }
            else
                currentTime += Time.deltaTime;
        }
    }

    public void Use()
    {
        //check whether the shield is on cooldown
        if (!Cooldown)
        {
            //if not on cooldown use the shield 
            Cooldown = true;
            Active = true;
            ForceFieldObject.SetActive(true);
        }
    }
}
