using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchWeapon : MonoBehaviour
{
    //Speed up amount
    public float IncreaseModifier = 2.0f;
    //slow down amount
    public float DecreaseModifier = 0.25f;
	
	// Update is called once per frame
	void Update ()
    {
        //Speed up time on left click
		if(Input.GetMouseButton(0))
        {
            Time.timeScale = IncreaseModifier;
        }
        else if (Input.GetMouseButton(1)) //slow down time on right click
        {
            Time.timeScale = DecreaseModifier;
        }
        else // reset time
            Time.timeScale = 1;
    }

    void OnDisable()
    {
        //reset time on disable (holster)
        Time.timeScale = 1;
    }
}
