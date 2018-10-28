using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public GameObject LaserHitParticles;
    public float MaxRange = 100.0f;
    public int MaxReflections = 1;

    private LineRenderer lineRenderer;
    private Ray ray;
    private Laser reflectedLaser;
    private Laser parentLaser;
    private Quaternion lastHitRotation;
    private GameObject lastHitObject;

	// Use this for initialization
	void Awake () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, Vector3.zero);
    }
	
	// Use fixed update as we're working with raycasts
	void FixedUpdate ()
    {
        //set ray orgin to gameeobject pos and set the direction to the forward vector
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, MaxRange))
        {
            //check if last hit gameobject has changed
            LaserDetector laserDetector = hit.transform.GetComponent<LaserDetector>(); //check if object hit is a laser detector
            if (laserDetector != null && lastHitObject != null && lastHitObject == hit.transform.gameObject)
            {
                //object is a laser detector
                //laserDetector.LaserExit(this);
            }

            //set the line rendered point to ray hit
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));

            //set postion of laser hit particles and enable them
            LaserHitParticles.transform.position = hit.point;
            LaserHitParticles.transform.rotation = Quaternion.LookRotation(hit.normal);
            LaserHitParticles.SetActive(true);

            //Trigger laser detector enter if last object is differnt to the current
            if(laserDetector != null)
            {
                laserDetector.LaserHit(this);
            }

            //check if object hit is reflective
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Reflective"))
            {
                if (reflectedLaser != null && lastHitRotation != hit.transform.rotation) // moved object reset laser
                {
                    reflectedLaser.transform.position = hit.point;
                    reflectedLaser.transform.rotation = Quaternion.LookRotation(hit.normal);
                }

                //reflect Laser
                if (reflectedLaser == null && GetLaserParentCount() < MaxReflections)
                {
                    reflectedLaser = Instantiate(gameObject).GetComponent<Laser>();
                    reflectedLaser.transform.position = hit.point;
                    reflectedLaser.transform.rotation = Quaternion.LookRotation(hit.normal);
                    reflectedLaser.parentLaser = this;
                }
            }
            else if (reflectedLaser != null)
            {
                DestroyLaser(reflectedLaser); //delete reflected laser
                reflectedLaser = null;
            }

            lastHitRotation = hit.transform.rotation;
            lastHitObject = hit.transform.gameObject;
        }
    }

    void DestroyLaser(Laser laser)
    {
        List<Laser> lasers = new List<Laser>();

        Laser currentLaser = laser;
        lasers.Add(currentLaser);

        //Get all child lasers and add them to a list
        while (currentLaser.reflectedLaser != null)
        {
            lasers.Add(currentLaser.reflectedLaser);
            currentLaser = currentLaser.reflectedLaser;
        }   
        //destroy all child lasers in the list
        foreach (var laserObject in lasers)
        {
            Destroy(laserObject.gameObject);
        }
        reflectedLaser = null;
    }

    int GetLaserParentCount()
    {
        Laser currentLaser = this;
        int count = 0;

        //Get all child lasers and add them to a list
        while (currentLaser.parentLaser != null)
        {
            count++;
            currentLaser = currentLaser.parentLaser;
        }

        return count;
    }


}
