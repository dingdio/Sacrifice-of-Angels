using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace DGE{
class EvadeState:State
{
    GameObject teasee;
    public override string Description()
    {
        return "Evade State";
    }

    public EvadeState(GameObject entity, GameObject teasee):base(entity)
    {
        this.teasee = teasee;
    }

    public override void Enter()
    {
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
        myGameObject.GetComponent<SteeringBehaviours>().EvadeEnabled = true;
        myGameObject.GetComponent<SteeringBehaviours>().evadeTarget = teasee;
    }

    public override void Update()
    {
        if (Vector3.Distance(myGameObject.transform.position, teasee.transform.position) > 30)
        {
            myGameObject.GetComponent<StateMachine>().SwicthState(new TeaseState(myGameObject, teasee));
        }
    }

    public override void Exit()
    {
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();            
    }
}
}
