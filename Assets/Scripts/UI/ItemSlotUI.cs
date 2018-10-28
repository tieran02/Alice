using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   //The item slot to display
    public InventoryItem slot;
    //Sprite for the item
    public Image Sprite;
    //Quantity of the item
    public Text Amount;
    //Is the hovering over an item?
    public bool IsHovering { get; private set; }

    void Awake()
    {
        //set the item slot to null at start and hide the sprite
        slot = null;
        Sprite.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        //check if item slot is null and if so hide the sprite and return
        if (slot == null)
        {
            Sprite.gameObject.SetActive(false);
            return;
        }
        //slot is not null and we can now display the item sprite and amount in the inventory slot for the UI
        Sprite.gameObject.SetActive(true);
        Sprite.sprite = slot.Item.Sprite;
        Amount.text = "x"+slot.Amount.ToString();
    }

    //On pointer enter set the hovering to true and make the sprite gray
    public void OnPointerEnter(PointerEventData eventData)
    {
        Sprite.color = Color.gray;
        IsHovering = true;
    }

    //On pointer exit set the hovering to false and make the sprite default color again
    public void OnPointerExit(PointerEventData eventData)
    {
        Sprite.color = Color.white;
        IsHovering = false;
    }
}
