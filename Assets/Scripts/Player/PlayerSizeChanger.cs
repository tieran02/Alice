using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSizeChanger : MonoBehaviour {
    //Max size the character can grow
    public float MaxScale = 2.0f;
    //Min scale the player can shrink
    public float MinScale =  0.2f;
    //Has the item to shrink
    public bool HasItem = false;
    //Inventory to check for item
    private Inventory inventory;
    //Get the first person conntroller to change the speed
    private FirstPersonController firstPersonController;
    //Get the character controller to get the height of the character
    private CharacterController cc;


    // Use this for initialization
    void Awake () {
        //Get components
        firstPersonController = GetComponent<FirstPersonController>();
        cc = GetComponent<CharacterController>();
        //Find inventory of the player
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if item is in the characters inventory
        if (!HasItem && inventory.HasItem("Drink Me Potion"))
            HasItem = true;
        if (!HasItem)
            return;

        //Calculate the head pos and cast a ray to make sure the player does not grow inside a object above
        Vector3 headPos = new Vector3(transform.position.x, transform.position.y + cc.height/2 * transform.localScale.y, transform.position.z);
        Ray ray = new Ray(headPos, Vector3.up);
        Debug.DrawRay(headPos, Vector3.up);

        //shrink character
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * MinScale, Time.deltaTime * 5f);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f) // grow character
        {
            if (!Physics.Raycast(ray, 0.1f))
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * MaxScale, Time.deltaTime * 5f);
        }
        else if (Input.GetMouseButtonDown(2)) //reset size on scrool wheel click
        {
            if (!Physics.Raycast(ray, 0.1f))
                transform.localScale = Vector3.one;
        }
        //change the speed depending on size
        firstPersonController.m_WalkSpeed = 7 * Mathf.Clamp(transform.localScale.x, .25f, 1);
        firstPersonController.m_RunSpeed = 12 * Mathf.Clamp(transform.localScale.x, .25f, 1);
    }
}
