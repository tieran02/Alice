using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour {

    public GameObject SpawnObject;
    public int LasersToDetect;
    public bool DestroyOnSpawn = true;
    private HashSet<Laser> currentHitLasers;

    void Awake()
    {
        currentHitLasers = new HashSet<Laser>();
    }

    void Update () {
        //check if all lasers are hitting the object
        if(currentHitLasers.Count >= LasersToDetect)
        {
            spawn();
        }
    }

    private void LateUpdate()
    {
        //clear lasers hit after update using lateupdate
        currentHitLasers.Clear();
    }

    public void LaserHit(Laser laser)
    {
        //add laser that hit to the hashset
        if(!currentHitLasers.Contains(laser))
            currentHitLasers.Add(laser);
    }

    private void spawn()
    {
        //spawn/enable object and destroy or disable laser detector
        if (SpawnObject != null)
            SpawnObject.SetActive(true);

        if (DestroyOnSpawn)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
