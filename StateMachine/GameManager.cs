using UnityEngine;
using System.Collections;
namespace DGE{
public class GameManager : MonoBehaviour
{

    public GameObject testShip;
    public GameObject testShip2;
	void Start () 
    {
        
        testShip.GetComponent<StateMachine>().SwicthState(new IdleState(testShip, testShip.GetComponent<ShipTarget>().target));
        testShip2.GetComponent<StateMachine>().SwicthState(new IdleState(testShip2, testShip2.GetComponent<ShipTarget>().target));
        
	}
	

	void Update () 
    {
	
	}
}
}
