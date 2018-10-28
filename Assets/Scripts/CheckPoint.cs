using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    private Transform playerTransform;

	void Awake () {
        GetComponent<BoxCollider>().isTrigger = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

    void OnTriggerEnter(Collider other)
    {
        //check if the player triggered the trigger
        if(other.transform.tag == "Player")
            Save();
    }

    void Save()
    {
        //check if checkpoint is already reached
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("CheckpointX"), PlayerPrefs.GetFloat("CheckpointY"), PlayerPrefs.GetFloat("CheckpointZ"));
            if(pos == transform.position)
            {
                return; // don't save as the checkpoint has already been met
            }
        }

        //alert the player they have reached a checkpoint
        AlertManager.GetInstance().AddAlert("Checkpoint Reached!");
        //save pos
        PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
        PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
        PlayerPrefs.SetFloat("CheckpointZ", transform.position.z);
        //save health
        PlayerPrefs.SetFloat("PlayerHealth", playerTransform.GetComponent<CharacterData>().CurrentHealth);
        PlayerPrefs.Save();
    }
}
