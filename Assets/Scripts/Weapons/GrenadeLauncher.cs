using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour {
    //How long you have to wait to throw another grenade
    public float CooldownTime = 10f;
    //Force to apply to grenade
    public float ThrowingForce = 20;
    //The required item to throw a grenade
    public Item RequiredItem;
    //Grenade prefab
    public GameObject GrenadePrefab;
    //Tranform to use for throwing
    public Transform ThrowTransform;
    public bool Cooldown { get; private set; }

    //the characters inventory 
    private Inventory inventory;
    //current time
    private float currentTime = 0;
    //Does the character have the grenade item?
    public bool hasGrenade = false;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        //add check item to add item event
        inventory.AddItemEvent += CheckItem;
    }

	// Update is called once per frame
	void Update ()
    {
        //check if has item and if not return
        if (!hasGrenade)
            return;

        //check if weapon is on cooldown
        if (Cooldown)
        {
            //check if current time is greater than fire rate and if so reset cooldown 
            if (currentTime > CooldownTime)
            {
                Cooldown = false;
                currentTime = 0;
            }
            else // else add delta time to the current time
                currentTime += Time.deltaTime;
        }

        //throw grenade
        if (Input.GetKeyDown(KeyCode.G) && !Cooldown)
        {
            //calculate spawn pos
            Vector3 pos = ThrowTransform.position + ThrowTransform.forward * 2;
            //Instantiate grenade
            GameObject grenade = Instantiate(GrenadePrefab, pos, ThrowTransform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * ThrowingForce,ForceMode.Impulse);

            Cooldown = true;
        }
	}

    public void CheckItem(InventoryItem item)
    {
        //check if item is the RequiredItem
        if (item.Item == RequiredItem)
        {
            //item is RequiredItem so set grenade to true and remove from add item event
            hasGrenade = true;
            inventory.AddItemEvent -= CheckItem;
        }
    }
}
