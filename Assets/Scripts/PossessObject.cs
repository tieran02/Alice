using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(SphereCollider))]
public class PossessObject : MonoBehaviour
{
    public Camera ObjectCamera;
    public Camera MainCamera;
    public float Sensitivity = 20.0f;

    private GameObject player;
    private float cameraDistance;
    private float cameraHeight;


    void Awake ()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        //calculate offset
        cameraDistance = Vector3.Distance(ObjectCamera.transform.position, transform.position);
        cameraHeight = ObjectCamera.transform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //check if the player is controlling the object
		if(player != null)
        {
            //player is controlling the object
            float roatateZAmount = Input.GetAxis("Vertical") * Sensitivity * Time.deltaTime;
            float roatateYAmount = Input.GetAxis("Horizontal") * Sensitivity * Time.deltaTime;
            transform.Rotate(0, roatateYAmount, 0, Space.World);
            transform.Rotate(0, 0, roatateZAmount, Space.Self);
        }
	}

    void LateUpdate()
    {
        //Calculate the camera pos
        Vector3 camPos = transform.position + transform.right * cameraDistance; 
        ObjectCamera.transform.position = new Vector3(camPos.x, cameraHeight, camPos.z);
        ObjectCamera.transform.LookAt(transform);
        //Calcualte the camera angle
        Vector3 eulerAngles = ObjectCamera.transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0, eulerAngles.y,0);
        //set cam rot
        ObjectCamera.transform.rotation = Quaternion.Euler(eulerAngles);
        //set cam pos
        ObjectCamera.transform.position += transform.forward * cameraDistance;
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {
            if (player == null)
            {
                player = other.gameObject;

                //disable player controller and tooltip
                GetComponent<TooltipOnTigger>().enabled = false;
                player.GetComponent<AudioSource>().enabled = false;
                player.GetComponent<FirstPersonController>().enabled = false;

                //swap camera
                MainCamera.enabled = false;
                ObjectCamera.enabled = true;

            }
            else
            {
                //enable player controller and tooltip
                GetComponent<TooltipOnTigger>().enabled = true;
                player.GetComponent<AudioSource>().enabled = true;
                player.GetComponent<FirstPersonController>().enabled = true;

                //swap camera
                MainCamera.enabled = true;
                ObjectCamera.enabled = false;

                player = null;
            }
        }
    }
}
