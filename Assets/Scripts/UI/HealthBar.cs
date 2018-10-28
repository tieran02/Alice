using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    //Character data to get the health from
    public CharacterData characterData;
    //UI stat bar to display the health to
    public StatBar statBar;

	
	// Update is called once per frame
	void Update ()
    {
        //set stat bar values
        statBar.MaxValue = characterData.MaxHealth;
        statBar.SetValue(characterData.CurrentHealth);
	}
}
