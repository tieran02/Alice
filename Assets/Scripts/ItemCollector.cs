using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemCollector : MonoBehaviour
{
    public List<Item> ItemsToCollect;
    public float EjectForce = 10.0f;
    public GameObject RewardObject;
    public string TextAfterCompletion;

    public List<Item> CurrentItems { get; private set; }
    public bool AllItemsCollected { get; private set; }
    private bool SpawnedReward = false;

    void Awake ()
    {
        //Get the box collider and make sure it is a trigger
        GetComponent<BoxCollider>().isTrigger = true;
        CurrentItems = new List<Item>();

    }

    void OnTriggerEnter(Collider other)
    {
        //check if the collider has an item attached to it
        ItemPickUp pickup = other.GetComponent<ItemPickUp>();

        if (pickup != null)
        {
            AddItem(pickup);
        }
    }

    void AddItem(ItemPickUp pickUp)
    {
        //check if item is an item that needs to be collected
        if (ItemsToCollect.Contains(pickUp.item))
        {
            //check if the item has already been collected
            if (CurrentItems.Contains(pickUp.item))
            {
                //Item alredy exists and therefor we can return
                EjectItem(pickUp);
                return;
            }

            //Item has not been added and therefor add new item to the collected List
            CurrentItems.Add(pickUp.item);
            //Destroy collected Item
            Destroy(pickUp.gameObject);
        }
        else
        {
            EjectItem(pickUp);
        }

        AllItemsCollected = AreItemsCollected();
    }

    void EjectItem(ItemPickUp pickUp)
    {
        //Eject the item in the opposite direction
        Rigidbody rb = pickUp.GetComponent<Rigidbody>();
        Vector3 direction = -rb.velocity.normalized;
        rb.AddForce(direction * EjectForce, ForceMode.Impulse);
    }

    void SpawnReward()
    {
        if (!SpawnedReward)
        {
            Instantiate(RewardObject, transform.position + Vector3.up * 3, Quaternion.identity);
            SpawnedReward = true;
            if (TextAfterCompletion != string.Empty)
                AlertManager.GetInstance().AddAlert(TextAfterCompletion);
        }
    }

    bool AreItemsCollected()
    {
        if (CurrentItems.Count == ItemsToCollect.Count)
        {
            SpawnReward();
            return true;
        }
        return false;
    }
}
