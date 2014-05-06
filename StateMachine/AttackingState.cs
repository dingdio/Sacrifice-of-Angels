using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace DGE{



class AttackingState:State
{
    float timeShot = 0.25f;
    GameObject enemyGameObject;

    public override string Description()
    {
        return "Attacking State";
    }

    public AttackingState(GameObject myGameObject, GameObject enemyGameObject):base(myGameObject)
    {
        this.enemyGameObject = enemyGameObject;
    }

    public override void Enter()
    {
        myGameObject.GetComponent<SteeringBehaviours>().TurnOffAll();
        myGameObject.GetComponent<SteeringBehaviours>().OffsetPursuitEnabled = true;
        myGameObject.GetComponent<SteeringBehaviours>().offsetPursuitOffset = new Vector3(0, 0, 5);
        myGameObject.GetComponent<SteeringBehaviours>().offsetPursueTarget = enemyGameObject;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        /*float range = 10.0f;
        timeShot += Time.deltaTime;
        float fov = Mathf.PI / 4.0f;
        // Can I see the enemy?

        if ((enemyGameObject.transform.position - myGameObject.transform.position).magnitude > range)
        {
            myGameObject.GetComponent<StateMachine>().SwicthState(new IdleState(myGameObject, enemyGameObject));
        }
        else
        {
            float angle;
            Vector3 toEnemy = (enemyGameObject.transform.position - myGameObject.transform.position);
            toEnemy.Normalize();
            angle = (float) Math.Acos(Vector3.Dot(toEnemy, myGameObject.transform.forward));
            if (angle < fov)
            {
                if (timeShot > 0.25f)
                {
                    GameObject lazer = new GameObject();
                    lazer.AddComponent<Lazer>();
                    lazer.transform.position = myGameObject.transform.position;
                    lazer.transform.forward = myGameObject.transform.forward;
                    timeShot = 0.0f;
                }
            }
        }*/
    }
}
}
