using System;
using System.Collections.Generic;
using UnityEngine;
namespace DGE{
    
public class IdleState:State
{
    static Vector3 initialPos = Vector3.zero;

    GameObject enemyGameObject;

    public override string Description()
    {
        return "Idle State";
    }

    public IdleState(GameObject myGameObject, GameObject enemyGameObject)
        : base(myGameObject)
    {
        this.enemyGameObject = enemyGameObject;
    }

    public override void Enter()
    {
        myGameObject.GetComponent<SteeringBehaviours>().path.Looped = true;            
        myGameObject.GetComponent<SteeringBehaviours>().path.draw = true;
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
        myGameObject.GetComponent<SteeringBehaviours>().FollowPathEnabled = true;
    }
    public override void Exit()
    {
        myGameObject.GetComponent<SteeringBehaviours>().path.Waypoints.Clear();
    }

    public override void Update()
    {
        float range = 20.0f;           
        // Can I see the enemy?
        if ((enemyGameObject.transform.position - myGameObject.transform.position).magnitude < range)
        {
            // Is the leader inside my FOV
            myGameObject.GetComponent<StateMachine>().SwicthState(new AttackingState(myGameObject, enemyGameObject));
        }
    }
}
}
