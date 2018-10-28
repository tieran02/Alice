using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class KnockBackObject : MonoBehaviour
{
    //Force to be applied
    public float Force = 1f;
    //Is object locked(Cant be pushed)
    public bool LockObject = true;
    private Rigidbody rb;

    void Awake()
    {
        //set collider to trigger if not already and get rigidybody
        GetComponent<SphereCollider>().isTrigger = true;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerStay(Collider other)
    {
        //check if other is player
        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {
            //Get the direction to push
            Vector3 dir = transform.position - other.transform.position;
            Push(dir);
        }
    }


    private void Push(Vector3 direction)
    {
        //set kinematic to false and apply force
        rb.isKinematic = false;
        rb.AddForce((direction.normalized * Force),ForceMode.Impulse);
    }
}
