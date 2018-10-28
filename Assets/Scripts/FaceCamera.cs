using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        //check if main camera is null and if so return
        if (Camera.main == null)
            return;
        //Get the forward vector of the camera
        Vector3 forward = Camera.main.transform.forward;
        //set the y to zero 
        forward.y = 0.0f;
        //make the object face the forward vector
        transform.rotation = Quaternion.LookRotation(forward);
	}
}
