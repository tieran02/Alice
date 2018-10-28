using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour {
    //Left hand game object
    public GameObject LeftHand;
    //Right hand game object
    public GameObject RightHand;

    //players inventory
    private Inventory inventory;
    private bool LeftHandHolstered = true;
    private bool RightHandHolstered = true;

    // Use this for initialization
    void Awake ()
    {
        //get and set inventory
        inventory = GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If no weapons equiped make hands holstered
        if (inventory.inventoryData.equiped[0] == null)
            LeftHandHolstered = true;
        if (inventory.inventoryData.equiped[1] == null)
            RightHandHolstered = true;

        //If first equiped item doesn't not equal null and Alpha1 is pressed then toggle weapon
        if (inventory.inventoryData.equiped[0] != null && Input.GetKeyDown(KeyCode.Alpha1))
            LeftHandHolstered = !LeftHandHolstered;
        //If second equiped item doesn't not equal null and Alpha2 is pressed then toggle weapon
        if (inventory.inventoryData.equiped[1] != null && Input.GetKeyDown(KeyCode.Alpha2))
            RightHandHolstered = !RightHandHolstered;

        LeftHand.SetActive(!LeftHandHolstered);
        RightHand.SetActive(!RightHandHolstered);

    }
}
