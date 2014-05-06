using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace DGE{
public class SteeringBehaviours : MonoBehaviour {

    // Flags to enable or disable the variour behaviours
    public bool SeekEnabled;
    public bool FleeEnabled;
    public bool ArriveEnabled;
    public bool FollowPathEnabled;
    public bool PursuitEnabled;
    public bool EvadeEnabled;
    public bool OffsetPursuitEnabled;

    // Parameters required by some of the behaviours
    public GameObject pursueTarget;
    public GameObject evadeTarget;
    public GameObject offsetPursueTarget;
    public Vector3 seekPos;
    public Vector3 fleePos;
    public Vector3 arrivePos;
    public Vector3 offsetPursuitOffset;
    public Path path;
    public float mass;
    
    // Required for the forward euler integration function
    public Vector3 velocity;
    public Vector3 force;
    public float maxSpeed;

    public bool draw;
    public bool looped;
    public Transform loadedWaypoints;

    void Awake()
    {
        path = new Path(); // An empty path
        if (loadedWaypoints !=null)
        {
            foreach (Transform waypoint in loadedWaypoints)
            {
                path.Waypoints.Add(waypoint.position);
            }
        }
        
    }


    void Start()
    {
        force = Vector3.zero;
        velocity = Vector3.zero;
        
        mass = 1.0f;
        maxSpeed = 5.0f;
        TurnOffAll();            
    }

    public void TurnOffAll()
    {
        SeekEnabled = false;
        FleeEnabled = false;
        ArriveEnabled = false;
        FollowPathEnabled = false;
        PursuitEnabled = false;
        EvadeEnabled = false;
        OffsetPursuitEnabled = false;
    }


    #region The Behaviours

    private Vector3 FollowPath()
    {
        path.Draw();
        float epsilon = 5.0f;
        float dist = (transform.position - path.NextWaypoint()).magnitude;
        if (dist < epsilon)
        {
            path.AdvanceToNext();
        }
        if ((!path.Looped) && path.IsLast())
        {
            return Arrive(path.NextWaypoint());
        }
        else
        {
            return Seek(path.NextWaypoint());
        }
    }

    Vector3 OffsetPursuit(Vector3 offset)
    {
        Vector3 target = Vector3.zero;
        target = offsetPursueTarget.transform.TransformPoint(offset);

        float dist = (target - transform.position).magnitude;

        float lookAhead = (dist / maxSpeed);

        target = target + (lookAhead * offsetPursueTarget.GetComponent<SteeringBehaviours>().velocity);

        return Arrive(target);
    }

    Vector3 Pursue()
    {
        Vector3 toTarget = pursueTarget.transform.position - transform.position;
        float dist = toTarget.magnitude;
        float time = dist / maxSpeed;

        Vector3 targetPos = pursueTarget.transform.position + (time * pursueTarget.GetComponent<SteeringBehaviours>().velocity);
    
        return Seek(targetPos);
    }

    Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity;

        desiredVelocity = targetPos - transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        return (desiredVelocity - velocity);
    }

    public Vector3 Arrive(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

        float slowingDistance = 2.0f;
        float distance = toTarget.magnitude;
        if (distance == 0.0f)
        {
            return Vector3.zero;
        }
        const float DecelerationTweaker = 0.0f;
        float ramped = maxSpeed * (distance / (slowingDistance * DecelerationTweaker));

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);
        return desired - velocity;
    }

    Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity;
        desiredVelocity = transform.position - targetPos;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        return (desiredVelocity - velocity);
    }

    Vector3 Evade()
    {
        float dist = (evadeTarget.transform.position - transform.position).magnitude;
        float lookAhead = maxSpeed;

        Vector3 targetPos = evadeTarget.transform.position + (lookAhead * evadeTarget.GetComponent<SteeringBehaviours>().velocity);
        return Flee(targetPos);
    }
    #endregion

	
	// Update is called once per frame
	void Update () {
        // Sum all the forces...
        // Probably not the best way to do this, because some forces need to be prioritised...
        if (SeekEnabled)
        {
            force += Seek(seekPos);
        }
        if (ArriveEnabled)
        {
            force += Arrive(arrivePos);
        }
        if (PursuitEnabled)
        {
            force += Pursue();
        }
        if (FleeEnabled)
        {
            force += Flee(fleePos);
        }
        if (OffsetPursuitEnabled)
        {
            force += OffsetPursuit(offsetPursuitOffset);
        }
        if (FollowPathEnabled)
        {
            force += FollowPath();
        }

        Vector3 acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        if (speed > maxSpeed)
        {
            velocity.Normalize();            
            velocity *= maxSpeed;
        }
        transform.position += velocity * Time.deltaTime;
        // Make the object point in the direction it's moving..
        if (speed > float.Epsilon)
        {
            transform.forward = velocity;
        }
        force = Vector3.zero;	    
	}




    void OnDrawGizmos()
    {

        if (draw)
        {



            for (int i = 1; i < loadedWaypoints.childCount; i++)
            {
                Gizmos.color = new Color(0, 0, 255);
                Gizmos.DrawLine(loadedWaypoints.GetChild(i - 1).position, loadedWaypoints.GetChild(i).position);

            }
            if (looped && (loadedWaypoints.childCount > 0))
            {
                Gizmos.color = new Color(0, 0, 225);
                Gizmos.DrawLine(loadedWaypoints.GetChild(0).position, loadedWaypoints.GetChild(loadedWaypoints.childCount - 1).position);

            }
        }
    }
}
}
