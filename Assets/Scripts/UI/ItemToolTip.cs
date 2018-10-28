using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    //Item to display 
    public Item item;
    //The Image UI component
    public Image Sprite;
    //The Text UI component for the name
    public Text Name;
    //The Text UI component for the description
    public Text Description;

	// Update is called once per frame
	void Update ()
    {
        //make sure item is not null
        if (item != null)
        {
            //show the item tool tip for the inventory
            transform.GetChild(0).gameObject.SetActive(true);
            //Set the tooltip to display the item data
            Sprite.sprite = item.Sprite;
            Name.text = item.Name;
            Description.text = item.Description;
        }
        else
        {
            //if item is null hide the tooltip
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
