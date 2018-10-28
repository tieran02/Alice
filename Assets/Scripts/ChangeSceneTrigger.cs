using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SphereCollider))]
public class ChangeSceneTrigger : MonoBehaviour {

    public string SceneName;
    public Vector3 PositionInScene;
    public Item RequiredItem; //You may need an item such a key to change sceene

    void Awake() {
        //Set collider to trigger
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            if (RequiredItem == null || other.GetComponent<Inventory>().HasItem(RequiredItem.Name))
                ChangeScene(other.transform);
            else if (RequiredItem != null && !other.GetComponent<Inventory>().HasItem(RequiredItem.Name))
                AlertManager.GetInstance().AddAlert("Requires an item to enter!");
        }
    }

    void ChangeScene(Transform player)
    {
        //Save the position to start the player in new scene
        PlayerPrefs.SetFloat("PlayerX", PositionInScene.x);
        PlayerPrefs.SetFloat("PlayerY", PositionInScene.y);
        PlayerPrefs.SetFloat("PlayerZ", PositionInScene.z);

        //set checkpoint
        PlayerPrefs.SetFloat("CheckpointX", PositionInScene.x);
        PlayerPrefs.SetFloat("CheckpointY", PositionInScene.y);
        PlayerPrefs.SetFloat("CheckpointZ", PositionInScene.z);
        //save health
        PlayerPrefs.SetFloat("PlayerHealth", GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterData>().CurrentHealth);

        PlayerPrefs.Save();

        FindObjectOfType<GameStateManager>().SaveGameObjects();
        player.position = PositionInScene;
        SceneManager.LoadScene(SceneName);
    }

}
