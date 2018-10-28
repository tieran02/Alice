using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AlertOnTrigger : MonoBehaviour {
    //Message to alert the player
    public string AlertMessage;

    void Awake()
    {
        //set the collider to trigger if not already
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        //if other is a player add the alert
        if(other.tag == "Player")
            AlertManager.GetInstance().AddAlert(AlertMessage);
    }
}
