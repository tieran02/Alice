using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Gate : MonoBehaviour {
    //Is the door currently opend
    public bool Opened = false;
    //Is the door locked (can't open by player)
    public bool Locked = false;
    private Animator _animator;


	// Use this for initialization
	void Awake () {
        //get the animator and set opened to the current Opened variable
        _animator = GetComponent<Animator>();
        _animator.SetBool("Opened", Opened);
        GetComponent<SphereCollider>().isTrigger = true;
    }
	
    void OnTriggerStay(Collider other)
    {
        //check if other is the player and pressed E
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {
            //Toggle the gate
            ToggleGate();
        }
    }

    //Closes the Gate
    public void Close(bool force = false)
    {
        Opened = false;
        _animator.SetBool("Opened", Opened);
    }
    //Opens the Gate
    public void Open(bool force = false)
    {
        Opened = true;
        _animator.SetBool("Opened", Opened);
    }

    public void ToggleGate(bool forceOpen = false)
    {
        if (!Locked || forceOpen)
        {
            Opened = !Opened;
            _animator.SetBool("Opened", Opened);
        }
    }
}
