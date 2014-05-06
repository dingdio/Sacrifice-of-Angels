using System;
using BGE;
using UnityEngine;
using System.Collections;


public enum Faction
    {
        Federation,
        Dominion
    }


public class ShipTarget : MonoBehaviour
{



    public GameObject target;

    [SerializeField]
    private ArrayList enemies = new ArrayList();

    [SerializeField]
    private GameObject[] allShips;

    [SerializeField]
    private SteeringBehaviours behaviours;

    private Faction myFaction;


    

	void Start ()
	{
	    behaviours = GetComponent<BGE.SteeringBehaviours>();
        myFaction = gameObject.GetComponent<ShipOverview>().faction;
	    FindTarget();
        behaviours.target = target;
        behaviours.leader = target;
        //PursuitBehaviour();

	}
	

	void Update ()
	{

	    if (target == null)
	    {
	        FindTarget();
	    }
	}


        



    public void FindTarget()
    {
        enemies.Clear();
        allShips = GameObject.FindGameObjectsWithTag("Ship");

        foreach (GameObject ship in allShips)
        {
            ShipOverview overview = ship.GetComponent<ShipOverview>();
            if (overview.faction != myFaction)
            {
                enemies.Add(ship);
            }
        }
        target = GetClosestEnemy(enemies);
        behaviours.leader = target;
    }

    GameObject GetClosestEnemy(ArrayList enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
