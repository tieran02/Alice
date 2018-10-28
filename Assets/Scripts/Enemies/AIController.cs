using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {
    //Set predefined patrol points for the AI to follow
    public Transform[] PatrolPoints;
    //If no patrol points how far should the AI roam
    public float RoamRadius = 10;
    //The disance between the player and AI before it gets dectected
    public float DetectDistance;
    //A list of items the Ai will drop on death
    public List<Item> DropedItems;
    //Should the AI automaticlly detect the player on start?
    public bool DetectFromStart = false;
    //Health bar object to hide/show 
    public GameObject HealthBar;

    //Has the player been detected?
    public bool Detected { get; private set; }

    //component variables
    private NavMeshAgent agent;
    private CharacterData characterData;
    //Player gameobject
    private GameObject player;
    //current patrol index to set as destination
    private int currentTargetPatrol = 1;

    private void Awake()
    {
        //Find and assign the player game object
        player = GameObject.FindGameObjectWithTag("Player");
        //Set component variables
        agent = GetComponent<NavMeshAgent>();
        characterData = GetComponent<CharacterData>();

        //If  AI has items to drop add the drop items method to the onDeathEvent of the character data
        if(DropedItems != null && characterData != null)
            characterData.OnDeathEvent += DropItems;
        //Hide the healthbar
        HealthBar.SetActive(false);
    }

    void DropItems()
    {
        //for each item in the droped items list instantiate the droped item at the postion of the AI
        foreach (var item in DropedItems)
        {
            Instantiate(item.DropedItem, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //check if player is within detect distance or if the the AI has detect from start
        if (GetDistanceToPlayer() <= DetectDistance || DetectFromStart)
        {
            //player is within detect range but may not be visable, so we need to raycast to see if the Ai can actually see the player
            Vector3 playerDir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, playerDir);
            //raycast hit data
            RaycastHit hit;

            //if raycast hits an object or AI has detect from start 
            if (Physics.Raycast(ray, out hit, DetectDistance) || DetectFromStart)
            {
                //check if the ray hit the player or is AI has detect from start then set the destination to the player
                if (hit.transform.tag == "Player" || DetectFromStart)
                {
                    //set destination and set detected to true
                    agent.destination = player.transform.position;
                    Detected = true;
                }
            }
        }
        else // if player is not within distance and does not have detect from start then the player has not been detected
            Detected = false; 

        //if player not detected then patrol area
        if (!Detected)
        {
            Patrol();
        }

        if(GetDistanceToPlayer() <= DetectDistance)
        {
            // if within range show health bar
            HealthBar.SetActive(true);
        }
        else
        {
            // if out of range hide health bar
            HealthBar.SetActive(false);
        }
    }

    
    public float GetDistanceToPlayer()
    {
        if(player != null)
            return Vector3.Distance(transform.position, player.transform.position);
        return float.MaxValue;
    }

    private void Patrol()
    {
        //patrol between predefined points
        if(PatrolPoints != null && PatrolPoints.Length > 1)
        {
            //set destination to the current target patrol
            Vector3 target = PatrolPoints[currentTargetPatrol].position;
            agent.destination = target;
            //if AI is within 2 units of the target goto next point
            if (Vector3.Distance(transform.position, target) <= 2)
            {
                NextPoint();
            }
        }
        else // else roam randomly within radius
        {
            if(RoamRadius > 0)
            agent.destination = RandomPointWithinRadius();
        }
    }

    Vector3 RandomPointWithinRadius()
    {
        //check if roam radius is greater than 0
        if (RoamRadius > 0)
        {
            //get random direction
            Vector3 dir = Random.insideUnitSphere * RoamRadius;
            //add current position to direction
            dir += transform.position;
            //Get point on navmesh to return
            NavMeshHit hit;
            Vector3 pos = Vector3.zero;
            if (NavMesh.SamplePosition(dir, out hit, RoamRadius, 1))
            {
                pos = hit.position;
            }
            return pos;
        }
        return Vector3.zero; // return zero if no roam radius
    }

    void NextPoint()
    {
        // if current target is less then the set patrol points then increment current target by one
        if (currentTargetPatrol + 1 < PatrolPoints.Length)
        {
            currentTargetPatrol++;
        }
        else // else reset current Target Patrol to the start
        {
            currentTargetPatrol = 0;
        }  
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void Start()
    {
        agent.isStopped = false;
    }

    //draw roam radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, RoamRadius);
    }
}
