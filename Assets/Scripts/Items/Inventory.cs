using System.Collections.Generic;
using System.IO;
using UnityEngine;

// inventory item class is the container to hold an item with a quantity
[System.Serializable]
public class InventoryItem
{
    public InventoryItem(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }
    //The item it is holding
    public Item Item;
    //The amount of the item it is holding
    public int Amount;
}

//Inventory data container holds all the data of the item and equiped items
[System.Serializable]
public struct InventoryData
{
    public List<InventoryItem> items; // list of items within the inventory
    public Weapon[] equiped; //equiped weapons 
}

public class Inventory : MonoBehaviour
{
    //How mand inventory slots should the inventory have
    public int InventorySize = 15;
    //Container to hold all the inventory data
    public InventoryData inventoryData;
    //hand transform postions
    public Transform LeftHand;
    public Transform RightHand;

    //Events to notify the UI and other scripts that may need to know when items have changed
    public delegate void AddItemDelegate(InventoryItem item);
    public event AddItemDelegate AddItemEvent;

    public delegate void RemoveItemDelegate(InventoryItem item);
    public event RemoveItemDelegate RemoveItemEvent;

    public delegate void UpdateItemAmountDelegate(InventoryItem item);
    public event UpdateItemAmountDelegate UpdateItemAmountEvent;

    public delegate void EquipItemDelegate(Weapon weapon, int hand);
    public event EquipItemDelegate EquipItemEvent;

    // Use this for initialization
    void Awake ()
    {
        //set the inventory data equiped to two null weapons
        inventoryData.equiped = new Weapon[2];
        //set the items to a new list
        inventoryData.items = new List<InventoryItem>();
	}

    void Start()
    {
        //load inventory at the start if exists
        Load();
    }

    // Update is called once per frame
    void Update ()
    {
        //Get items the character may be looking at on the ground
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //raycast from the center of the screen
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            //check if item hit was an item pickup
            if(hit.transform.tag == "ItemPickup")
            {
                //get pick up component
                ItemPickUp itemPickup = hit.transform.GetComponent<ItemPickUp>();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    //add item and destroy pickup
                    AddItem(itemPickup.item);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
	}

    public void UseItem(Item item)
    {
        //check if item is a weapon
        if (item.GetType() == typeof(Weapon))
        {
            //cast item to weapon and equip it
            Weapon weapon = item as Weapon;
            EquipWeapon(weapon, 1);
        }
    }

    public void AddItem(Item item)
    {
        //search inventory if item already exists
        for(int i = 0; i < inventoryData.items.Count; i++)
        {
            InventoryItem inventoryItem = inventoryData.items[i];
            //check if item alredy exists
            if (inventoryItem.Item == item)
            {
                //item exists so now check if we can just incrase the amount
                if (inventoryItem.Amount < inventoryItem.Item.StackSize)
                {
                    //add item by increase the amount of the inventory item
                    inventoryItem.Amount++;
                    Debug.Log("Added +1 " + item.name + "to stack");
                    //Call update item event
                    if (UpdateItemAmountEvent != null)
                        UpdateItemAmountEvent(inventoryItem);
                    return;
                }
            }
        }

        //Check if inventory is full before adding a new item 
        if (inventoryData.items.Count >= InventorySize)
            return; // inventory full so return

        //Add new item if it doesnt exist OR stack was full
        InventoryItem newItem = new InventoryItem(item, 1);
        inventoryData.items.Add(newItem);
        //Call add item event
        if (AddItemEvent != null)
            AddItemEvent(newItem);
    }

    public void RemoveItem(Item item)
    {
        //check if item exists in inventory
        for (int i = 0; i < inventoryData.items.Count; i++)
        {
            InventoryItem inventoryItem = inventoryData.items[i];
            //check if item exist in inventory
            if (inventoryItem.Item == item)
            {
                //if the item in the inventory amount is greater than one then just remove one from amount
                if(inventoryItem.Amount > 1 )
                {
                    inventoryItem.Amount--;
                    //call updated item amount event
                    if (UpdateItemAmountEvent != null)
                        UpdateItemAmountEvent(inventoryItem);
                    return; 
                }
                else //if item amount is less than one just remove the whole item from the inventory
                {
                    inventoryData.items.RemoveAt(i);
                    //call remove item event
                    if (RemoveItemEvent != null)
                        RemoveItemEvent(inventoryItem);
                    return;
                }
            }
        }

    }

    public void DropItem(InventoryItem slot)
    {
        //check if item exists in inventory and is not a quest item (Quest items can't be droped)
        if (inventoryData.items.Contains(slot) && !slot.Item.QuestItem)
        {
            //instantiate dropped item infront of the character
            GameObject dropedItem = Instantiate(slot.Item.DropedItem, transform.position + transform.GetChild(0).forward , transform.GetChild(0).rotation);
            //get rigid body of the droped item
            Rigidbody rb = dropedItem.GetComponent<Rigidbody>();
            //add an impulse force to throw the object
            rb.AddForce(transform.GetChild(0).forward * 5, ForceMode.Impulse);
            //remove item from inventory
            RemoveItem(slot.Item);
        }
    }

    public void EquipWeapon(Weapon weapon,int hand)
    {
        //if holding two handed weapon and character tries to equip a one hand then return
        if (hand == 2 && inventoryData.equiped[0] != null && inventoryData.equiped[0].Hands == 2)
            return;


        //if weapon is null unequip item
        if(weapon == null)
        {
            //add item back to inventory
            AddItem(inventoryData.equiped[hand - 1]);

            //depending what hand to equip to destroy the old weapon
            if (hand == 1 && LeftHand.childCount > 0)
                Destroy(LeftHand.GetChild(0).gameObject);
            if (hand == 2 && RightHand.childCount > 0)
                Destroy(RightHand.GetChild(0).gameObject);
            //set the equiped weapon to null and return
            inventoryData.equiped[hand - 1] = null;
            return;
        }

        //if weapon is one handed
        if(weapon.Hands == 1)
        {
            //Add previously equiped item back to inventory
            if (inventoryData.equiped[hand-1] != null)
            {
                EquipWeapon(null, hand);
            }

            inventoryData.equiped[hand-1] = weapon;
            //Instantiate weapon to character hands
            InstantiateWeapon(weapon, hand);
            //Remove new equiped item from inventory
            RemoveItem(weapon);
        }
        else
        {
            //Add previously equiped item back to inventory
            if (inventoryData.equiped[0] != null)
            {
                EquipWeapon(null, 1);
            }
            if (inventoryData.equiped[1] != null)
            {
                EquipWeapon(null, 2);
            }

            //two handed weapons should always be in the first hand
            inventoryData.equiped[0] = weapon;
            InstantiateWeapon(weapon, 1);
            //Remove new equiped item from inventory
            RemoveItem(weapon);
        }
        //call equip item event
        if (EquipItemEvent != null)
            EquipItemEvent(weapon, hand);
    }

    void InstantiateWeapon(Weapon weapon, int hand)
    {
        //instantiate weapon game object by using the weapon prefab from the item
        GameObject weaponObject = Instantiate(weapon.Prefab);

        //set the weapons parent to the correct hand
        if (hand == 1)
            weaponObject.transform.parent = LeftHand.transform;
        else
            weaponObject.transform.parent = RightHand.transform;
        //reset Position and rotation
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;

        //set the owner of the weapon by checking what kind of weapon it is.
        Gun gun = weaponObject.GetComponent<Gun>();
        if (gun != null)
            gun.Owner = gameObject;
        ProjectileWeapon projectileWeapon = weaponObject.GetComponent<ProjectileWeapon>();
        if (projectileWeapon != null)
            projectileWeapon.Owner = gameObject;
        MeleeWeapon meleeWeapon = weaponObject.GetComponent<MeleeWeapon>();
        if (meleeWeapon != null)
        {
            meleeWeapon.Owner = gameObject;
            //set the animator to the characters animator on thier arm
            meleeWeapon.animator = weaponObject.transform.parent.parent.GetComponent<Animator>();
        }
    }
    private void OnDestroy()
    {
        //save the inventory when component gets destroyed
        Save();
    }

    void Save()
    {
        //Serialise object to json and save to file
        string path = Application.persistentDataPath + "/Saves/";
        string json = JsonUtility.ToJson(inventoryData);
        File.WriteAllText(path + transform.name + "Inventory" + ".json", json);
    }

    void Load()
    {
        //load inventory from json
        //get path of the saves
        string path = Application.persistentDataPath + "/Saves/";
        //check if save file exists
        if (File.Exists(path + transform.name + "Inventory" + ".json"))
        {
            //if the save exists deserialise the json file and load it into the inventory data
            string json = File.ReadAllText(path + transform.name + "Inventory" + ".json");
            inventoryData = JsonUtility.FromJson<InventoryData>(json);
        }
    }

    public bool HasItem(string name)
    {
        //check if item exists in equiped items
        foreach (var item in inventoryData.equiped)
        {
            if (item != null && item.Name == name)
                return true;
        }
        //check if item exists in inventory items
        foreach (var item in inventoryData.items)
        {
            if (item.Item.Name == name)
                return true;
        }
        //item does not exist
        return false;
    }

    //Remove all items and add them again (Used to sync with UI after loading inventory)
    public void Refresh()
    {
        //a copy all items in inventoy and equiped items
        List<Item> items = new List<Item>();
        //Get equiped items if any
        Weapon LeftItem = inventoryData.equiped[0];
        Weapon RightItem = inventoryData.equiped[1];

        //Add equiped Items to items list is not null
        if (LeftItem != null)
            items.Add(inventoryData.equiped[0]);
        if (RightItem != null)
            items.Add(inventoryData.equiped[1]);

        //go through all items and add them to the new item list
        for (int i = 0; i < inventoryData.items.Count; i++)
        {
            if (inventoryData.items[i].Amount > 1)
            {
                for (int j = 0; j < inventoryData.items[i].Amount; j++)
                {
                    items.Add(inventoryData.items[i].Item);
                }
            }else
                items.Add(inventoryData.items[i].Item);
        }

        //unequip weapons
        if (inventoryData.equiped[0] != null)
            EquipWeapon(null, 1);
        if (inventoryData.equiped[1] != null)
            EquipWeapon(null, 2);
        //remove items
        inventoryData.items.Clear();
        //readd items
        foreach (var item in items)
        {
            AddItem(item);
        }
        //equip Items
        if (LeftItem != null)
            EquipWeapon(LeftItem, 1);
        if (RightItem != null)
            EquipWeapon(RightItem, 2);
    }
}
