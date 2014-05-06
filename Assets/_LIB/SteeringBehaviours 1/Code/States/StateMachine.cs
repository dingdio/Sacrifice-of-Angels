using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.States
{
	public class StateMachine:MonoBehaviour
	{
        [SerializeField]
        private State currentState;

        public string currentStateName;

        private void Start()
        {
            currentStateName = "State not set";
        }

        public void Update()
        {
            if (currentState != null)
            {
                //SteeringManager.PrintMessage("Current state: " + currentState.Description());
                currentState.Update();
            }
        }

        public void SwicthState(State newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
                currentStateName = newState.Description();
            }

            currentState = newState;
            if (newState != null)
            {
                currentState.Enter();
                currentStateName = newState.Description();
            }
        }
	}
}
