using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct GameObjectStateData
{
    // alist of savable objects (stroes the sqr magnitude)
    public List<float> SavableObjects;
}

public class GameStateManager : MonoBehaviour
{
    //The players camera
    public Camera PlayerCamera;
    //Death camera to display when dead
    public Camera DeathCamera;
    //saved gameobjects state
    private GameObjectStateData gameStateData;
    //Players gameobject
    private GameObject player;

    //On game load clear all saves
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        ClearSaves();
    }

    void Awake ()
    {
        //set savable object to a new lists of floats
        gameStateData.SavableObjects = new List<float>();
        //Find the player gameobject
        player = GameObject.FindGameObjectWithTag("Player");
        //Get the players character data and add OnPlayerDie method to the death event
        player.GetComponent<CharacterData>().OnDeathEvent += OnPlayerDie;
        //Check if player has a saved postion and if so move the player to that pos
        if(PlayerPrefs.HasKey("PlayerX"))
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));

    }

    private void Start()
    {
        //path to the saves
        string path = Application.persistentDataPath + "/Saves/";
        //check is save exits and if so load saved objects
        if (File.Exists(path + SceneManager.GetActiveScene().name + ".json"))
        {
            //Load level data 
            LoadSavedObjects(LoadGameObjects());
        }
    }

    public void AddSavedObject(float startSqrMag)
    {
        //check if saved object already contaisn the sqr magnitude and if it doesnt then add it
        if(!gameStateData.SavableObjects.Contains(startSqrMag))
            gameStateData.SavableObjects.Add(startSqrMag);
    }
    //Removed a saved object 
    public void RemoveSavedObject(float startSqrMag)
    {
        gameStateData.SavableObjects.Remove(startSqrMag);
    }

    //Loads all the saved game objects and returns it
    GameObjectStateData LoadGameObjects()
    {
        string path = Application.persistentDataPath + "/Saves/" + SceneManager.GetActiveScene().name + ".json";
        string json = File.ReadAllText(path);
        gameStateData = JsonUtility.FromJson<GameObjectStateData>(json);
        return gameStateData;

    }
    //Saves the current state of gameStateData to a json file
    public void SaveGameObjects()
    {
        string path = Application.persistentDataPath + "/Saves/" + SceneManager.GetActiveScene().name + ".json";
        string json = JsonUtility.ToJson(gameStateData);
        File.WriteAllText(path, json);
    }
    //Clears all the saves and PlayerPrefs
    static void ClearSaves()
    {
        string path = Application.persistentDataPath + "/Saves/";
        if(Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);
        PlayerPrefs.DeleteAll();
    }

    void LoadSavedObjects(GameObjectStateData data)
    {
        //gets a list of all objects with the Savable script attached
        List<Saveable> savableObjects = new List<Saveable>(FindObjectsOfType<Saveable>());
        //A list to hold all the objects from the save fole
        List<Saveable> objsFromSave = new List<Saveable>();

        //loop through all savableObjects
        foreach (var obj in savableObjects)
        {
            //check is savableObject is in saved data and if soo add it to the objects from save list
            if (data.SavableObjects.Contains(obj.startSqrMag))
                objsFromSave.Add(obj);
        }
        //A list of objects to remove that are not both in savableObjects AND objsFromSave
        var objsToRemove = savableObjects.Except(objsFromSave);
        foreach (var obj in objsToRemove)
        {
            //destroy object
            Destroy(obj.gameObject);
        }
    }

    public void OnPlayerDie()
    {
        //DisableMainCamera
        PlayerCamera.enabled = false;
        //Enable death game
        DeathCamera.enabled = true;
    }

    public void Restart()
    {
        //delete all saves
        string path = Application.persistentDataPath + "/Saves/";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);

        Time.timeScale = 1; //set the time scale to one in case the game was paused
        //load first scene
        SceneManager.LoadSceneAsync("Game");
    }

    public void LoadFromLastCheckpoint()
    {
        //check if checkpoint exists
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            //get player transform
            Vector3 checkpointPos = new Vector3(PlayerPrefs.GetFloat("CheckpointX"), PlayerPrefs.GetFloat("CheckpointY"), PlayerPrefs.GetFloat("CheckpointZ"));
            player.transform.position = checkpointPos;
            player.SetActive(true);

            //set player health
            player.GetComponent<CharacterData>().SetHealth(PlayerPrefs.GetFloat("PlayerHealth"));

            //reset camera
            DeathCamera.enabled = false;
            PlayerCamera.enabled = true;

            //hide death menu
            FindObjectOfType<UIManager>().HideDeathMenu();
        }
        else
        {
            //no checkpoint, restart from beginning
            Restart();
        }
    }

    public void Quit()
    {
        Time.timeScale = 1; //set the time scale to one in case the game was paused
        SceneManager.LoadSceneAsync(0);
    }
}
