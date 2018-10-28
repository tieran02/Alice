using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour {
    //Game state instance
    private GameStateManager gameState;
    //The start sqr magnitude of the object
    public float startSqrMag;

    void Awake()
    {
        //get the gamestate instance
        gameState = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
        //calculate the start sqr magnitude of the object
        startSqrMag = transform.position.sqrMagnitude;
        //add the item to the gamestate
        gameState.AddSavedObject(startSqrMag);
    }

    void OnDestroy()
    {
        //Remove object from gamestate on destroy
        gameState.RemoveSavedObject(startSqrMag);
    }
}
