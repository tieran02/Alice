using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUI : MonoBehaviour
{
    //Inventory data to display
    public Inventory inventory;
    //Item tool tip component to show hovering items
    public ItemToolTip toolTip;
    //The item slot prefab to fill the inventoy UI with
    public GameObject ItemSlotPrefab;
    //The Grid to hold the item slots in
    public Transform Grid;
    //The Item slot for the Left equiped Item
    public ItemSlotUI LeftItemSlot;
    //The Item slot for the Right equiped Item
    public ItemSlotUI RightItemSlot;
    //Is the Inventroy visable
    public bool Visable { get; private set; }

    // Use this for initialization
    void Start ()
    {
        //Subscribe to the inventory events to update the UI when the state of the inventory has changed
        inventory.AddItemEvent += OnItemPickup;
        inventory.RemoveItemEvent += OnItemRemove;
        inventory.UpdateItemAmountEvent += OnUpdateItemAmount;
        inventory.EquipItemEvent += OnEquipItem;

        //Add the required amount of inventory slots
        for (int i = 0; i < inventory.InventorySize; i++)
        {
            Instantiate(ItemSlotPrefab, Grid).GetComponent<ItemSlotUI>();
        }

        //Refresh the inventory to sync it when loaded
        inventory.Refresh();
        Refresh();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //set the tooltip item to null
        toolTip.item = null;

        //loop through all the item slots in the inventory grid
        for (int i = 0; i < Grid.childCount; i++)
        {
            //Get the item slot UI of the current child game object
            ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
            //check if the slot is null and if so continue to the next item slot in the grid
            if (slotUI.slot == null)
                continue;

            //check if the player is hovering over a item and the inventory is visable
            if (slotUI.IsHovering && Visable)
            {
                //set the tooltip item to the hovering itemslot
                toolTip.item = slotUI.slot.Item;

                //Drop the item 
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    inventory.DropItem(slotUI.slot);
                    continue;
                }
                //Check if the item is a Weapon
                if (slotUI.slot.Item.GetType() == typeof(Weapon))
                {
                    //Cast the item to a weapon
                    Weapon weapon = slotUI.slot.Item as Weapon;
                    //Equip the weapon to the left hand
                    if (Input.GetMouseButtonDown(0))
                    {
                        inventory.EquipWeapon(weapon, 1);
                    }
                    //Equip the weapon to the right hand
                    else if (Input.GetMouseButtonDown(1))
                    {
                        inventory.EquipWeapon(weapon, 2);
                    }
                }
            }
        }

        //check if the player is hovering of the left equiped item and left clicks
        if (Input.GetMouseButtonDown(0) && LeftItemSlot.IsHovering && Visable)
        {
            //Unequip left weapon
            inventory.EquipWeapon(null, 1);
            LeftItemSlot.slot = null;
        }
        //check if the player is hovering of the right equiped item and left clicks
        else if (Input.GetMouseButtonDown(0) && RightItemSlot.IsHovering && Visable)
        {
            //Unequip right weapon
            inventory.EquipWeapon(null, 2);
            RightItemSlot.slot = null;
        }
    }

    //Gets called when the character picks up an item
    public void OnItemPickup(InventoryItem item)
    {
        //loop through all the item slot UI components in the grid and checks if one of them is empty to add a new item
        for (int i = 0; i < Grid.childCount; i++)
        {
            ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
            if (slotUI.slot == null)
            {
                slotUI.slot = item;
                break;
            }
        }
    }
    //Gets called when the character removes an item
    public void OnItemRemove(InventoryItem item)
    {
        for(int i = 0; i < Grid.childCount; i++)
        {
            //loop through all the item slot UI components in the grid and removes the item from the inventory view
            ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
            if (item == slotUI.slot)
                slotUI.slot = null;
        }
    }
    //Gets called when the character updates an item amount
    public void OnUpdateItemAmount(InventoryItem item)
    {
        //loop through all the item slot UI components in the grid and update the quantity of an item
        for (int i = 0; i < Grid.childCount; i++)
        {
            ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
            if (item == slotUI.slot)
                slotUI.slot.Amount = item.Amount;
        }
    }
    //Gets called when the character equips an item
    public void OnEquipItem(Weapon weapon, int hand)
    {
        //check what hand to equip the weapon to and then equip the item through the inventory
        if (hand == 1)
            LeftItemSlot.slot = new InventoryItem(weapon,1);
        else
            RightItemSlot.slot = new InventoryItem(weapon, 1);
    }

    //Toggle the inventory view
    public void Toggle()
    {
        //Get the attached canvas group
        CanvasGroup cg = GetComponent<CanvasGroup>();
        //Find the players gameobject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) // Player is null and has died
            return;
        //get the players FPS controller
        FirstPersonController fpsController = player.GetComponent<FirstPersonController>();

        //check if inventory is visable
        if (Visable == true)
        {
            //Hide the inventory if already visable
            cg.interactable = false;
            cg.alpha = 0;
            Visable = false;
            //set the mouse look sensitivy back to normal
            fpsController.m_MouseLook.XSensitivity = 2;
            fpsController.m_MouseLook.YSensitivity = 2;
            //lock the cursor 
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //show the inventory if it was hidden before 
            cg.interactable = true;
            cg.alpha = 1;
            Visable = true;
            //set the mouse look sensitivy to 0 so the player cant look around
            fpsController.m_MouseLook.XSensitivity = 0;
            fpsController.m_MouseLook.YSensitivity = 0;
            //unlock the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Refresh()
    {
        //Remove all items from ui
        for (int i = 0; i < Grid.childCount; i++)
        {
            ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
            slotUI.slot = null;
        }
        //go through all the items in the inventory and add them back to the UI
        foreach (var item in inventory.inventoryData.items)
        {
            for (int i = 0; i < Grid.childCount; i++)
            {
                ItemSlotUI slotUI = Grid.GetChild(i).GetComponent<ItemSlotUI>();
                if (slotUI.slot == null)
                {
                    slotUI.slot = item;
                    break;
                }
            }
        }
    }
}
