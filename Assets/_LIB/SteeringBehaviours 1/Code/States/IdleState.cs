using System;
using System.Collections.Generic;
using UnityEngine;
using BGE;

namespace BGE.States
{
    public class IdleState:State
    {
        static Vector3 initialPos = Vector3.zero;

        public override string Description()
        {
            return "Idle State";
        }

        public IdleState(GameObject entity):base(entity)
        {
        }

        public override void Enter()
        {
            if (initialPos == Vector3.zero)
            {
                initialPos = entity.transform.position;
            }
            //entity.GetComponent<SteeringBehaviours>().path.Looped = true;            
            //entity.GetComponent<SteeringBehaviours>().path.draw = true;
            entity.GetComponent<SteeringBehaviours>().turnOffAll();
            entity.GetComponent<SteeringBehaviours>().FollowPathEnabled = true;
            entity.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
            entity.GetComponent<SteeringBehaviours>().maxSpeed = 300;
        }
        public override void Exit()
        {
            //entity.GetComponent<SteeringBehaviours>().path.Waypoints.Clear();
        }

        public override void Update()
        {
            if (entity.GetComponent<ShipTarget>().target== null)
            return;
            GameObject leader = entity.GetComponent<ShipTarget>().target;    
            
            float range = 70.0f;           
            // Can I see the leader?

            if ((leader.transform.position - entity.transform.position).magnitude < range)
            {
                // Is the leader inside my FOV
                entity.GetComponent<StateMachine>().SwicthState(new AttackingState(entity));
            }
            else
            {
                entity.GetComponent<ShipTarget>().FindTarget();
            }
        }
    }
}
