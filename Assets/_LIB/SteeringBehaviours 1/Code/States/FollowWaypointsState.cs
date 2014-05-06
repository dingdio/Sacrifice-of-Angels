using System;
using System.Collections.Generic;
using UnityEngine;
using BGE;

namespace BGE.States
{
    public class FollowWaypointsState : State
    {
        static Vector3 initialPos = Vector3.zero;

        public override string Description()
        {
            return "Follow Waypoints State";
        }

        public FollowWaypointsState(GameObject entity)
            : base(entity)
        {
        }

        public override void Enter()
        {
            entity.GetComponent<SteeringBehaviours>().path.Looped = false;
            entity.GetComponent<SteeringBehaviours>().path.draw = true;
            entity.GetComponent<SteeringBehaviours>().turnOffAll();
            entity.GetComponent<SteeringBehaviours>().FollowPathEnabled = true;
            entity.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
        }
        public override void Exit()
        {
            //entity.GetComponent<SteeringBehaviours>().path.Waypoints.Clear();
        }

        public override void Update()
        {
            float range = 50.0f;
            // Can I see the leader?
            GameObject leader = SteeringManager.Instance.currentScenario.leader;
            if ((leader.transform.position - entity.transform.position).magnitude < range)
            {
                // Is the leader inside my FOV
                entity.GetComponent<StateMachine>().SwicthState(new AttackingState(entity));
            }
        }
    }
}
