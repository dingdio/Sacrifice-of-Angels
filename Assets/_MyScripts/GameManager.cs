using System.Collections.Generic;
using BGE.States;
using UnityEngine;
using System.Collections;
namespace BGE{
public class GameManager : MonoBehaviour
    {
        public List<GameObject> enemyFleet; 
	    void Start () 
        {



	        if (enemyFleet!=null)
	        {
	            foreach (GameObject fleet in enemyFleet)
	            {
	                for (int i = 0; i < fleet.transform.childCount; i++)
	                {
                        fleet.transform.GetChild(i).gameObject.GetComponent<StateMachine>().SwicthState(new IdleState(fleet.transform.GetChild(i).gameObject));
	                }
	                   
                    
	            }
                
	        }
	    }
	

	    void Update () 
        {
	
	    }
    }
}
