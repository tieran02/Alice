using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileCannon : MonoBehaviour {

    public float FireRate = 1.0f;
    public float Range = 200;

    public bool Cooldown { get; private set; }

    public GameObject ProjectilePrefab;
    public Transform SpawnPos;

    private float currentTime;

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (currentTime >= FireRate)
        {
            Cooldown = false;
            currentTime = 0;
        }

        if (!Cooldown)
        {
            Fire();
        }

        currentTime += Time.fixedDeltaTime;
    }

    public void Fire()
    {
        Cooldown = true;
        Instantiate(ProjectilePrefab, SpawnPos.position, SpawnPos.rotation);
    }
}
