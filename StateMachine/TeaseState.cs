using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace DGE{
public class TeaseState:State
{
    GameObject teasee;

    public override string Description()
    {
        return "Tease State";
    }

    public TeaseState(GameObject entity, GameObject teasee):base(entity)
    {
        this.teasee = teasee;
    }

    public override void Enter()
    {
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
        myGameObject.GetComponent<SteeringBehaviours>().PursuitEnabled = true;
        myGameObject.GetComponent<SteeringBehaviours>().pursueTarget = teasee;
    }

    public override void Update()
    {
        float distance = 5.0f;
        if (Vector3.Distance(myGameObject.transform.position, teasee.transform.position) < distance)
        {
            myGameObject.GetComponent<StateMachine>().SwicthState(new EvadeState(myGameObject, teasee));
        }
    }

    public override void Exit()
    {
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();            
    }
}
}

