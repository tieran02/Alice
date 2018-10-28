using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HordeSpawner : MonoBehaviour {
    //The enemy prefab that should be spawned
    public GameObject EnemyToSpawn;
    //How many enemies to spawn
    public int MaxEnemyCount = 20;
    //Have enemies been spawned?
    private bool spawned = false;

	// Use this for initialization
	void Awake ()
    {
        //set collider to trigger
        GetComponent<SphereCollider>().isTrigger = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        //on trigger enter check if horde has been spawned and if not spawn them
        if(!spawned)
            SpawnHorde();
    }

    void SpawnHorde()
    {
        //set spawned to true and spawn enemies
        spawned = true;
        for (int i = 0; i < MaxEnemyCount; i++)
        {
            //spawn enemy and set parent to this transform
            Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}
