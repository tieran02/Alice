using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ToggleGateOnTrigger : MonoBehaviour {
    //Gate to toggle
    public Gate Gate;
    //Lock the cate
    public bool LockGate;
    //Open or close the cate
    public bool Open = false;

    void OnTriggerEnter(Collider other)
    {
        //Open the gate
        if (Open && !Gate.Opened)
        {
            Gate.Open();
        }
        //close the gate
        else if(!Open && Gate.Opened)
            Gate.Close();
        //lock gate is set to
        Gate.Locked = LockGate;
    }
}
