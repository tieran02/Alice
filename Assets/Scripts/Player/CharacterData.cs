using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    //private fields for charcter data to make sure it can only be changed within this class only
    [SerializeField]
    private string characterName;
    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private bool immortal = false;
    [SerializeField]
    private bool DestroyOnDeath = true;

    public float CurrentHealth { get; private set; }
    public bool Alive { get; private set; }
    //Starting Items of the character
    public Item[] StartingItem;
    public MeshRenderer meshRenderer;

    //Event to be called on death
    public delegate void OnDeath();
    public event OnDeath OnDeathEvent;

    private Color origionalColor;

    void Awake()
    {
        //set current health to max health
        CurrentHealth = MaxHealth;
        //set orginal color
        origionalColor = meshRenderer.material.color;
    }

    void Start()
    {
        //path to the inventory saves
        string path = Application.persistentDataPath + "/Saves/" + transform.name + "Inventory" + ".json";

        //if there is no saved inventory add the start items to the players inventory
        if (!System.IO.File.Exists(path))
        {
            Inventory inventory = GetComponent<Inventory>();
            if (inventory != null)
            {
                foreach (var item in StartingItem)
                {
                    inventory.AddItem(item);
                }
            }
        }
    }

    //Get Max health
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    //Set max health
    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void TakeDamage(float amount)
    {
        //check if current health is greater than 0 and reduce health by an amount
        if(CurrentHealth > 0)
            CurrentHealth -= amount;
        //Set the matieral to red 
        meshRenderer.material.color = Color.red;
        //Reset the color back to the start colour after 1/10 of a second
        Invoke("ResetColor", .1f);

        //check if current health is less than 0 and not imortal
        if (CurrentHealth <= 0 && !immortal)
        {
            //kill the character
            Kill();
        }
    }

    public void Heal(float amount)
    {
        //heal the character but clamp the amount to not overheal
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }

    public void SetHealth(float amount)
    {
        //Set the health to a certain amount but clamp the amount to not set health above max health
        CurrentHealth = Mathf.Clamp(amount, 0, MaxHealth);

        //if current health is greater than 0 set the character to alive in case it died
        if (CurrentHealth > 0)
            Alive = true;
    }

    public void Kill()
    {
        //Kill the character and call the death event
        Alive = false;
        if(OnDeathEvent != null)
            OnDeathEvent();
        //If destroy on death is true than destroy the character ELSE just disable it
        if (DestroyOnDeath)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    void ResetColor()
    {
        //reset the material color back to the orginal color
        meshRenderer.material.color = origionalColor;
    }
}
