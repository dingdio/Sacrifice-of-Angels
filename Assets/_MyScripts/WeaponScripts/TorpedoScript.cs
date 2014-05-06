using System;
using UnityEngine;
using System.Collections;

public class TorpedoScript : MonoBehaviour {

    [Serializable]
    public class stats
    {
	public float speed; //torpedo speed
    public float hullDmg; //torpedo damage vs hull
    public float shieldDmg; //torpedo damage vs shield
    public float range; //torpedo range
    public float cost; //torpedo "normal" cost, to be used in GUI only
    public float cooldown; // torpedo cooldown
	
}
    [Serializable]
    public class effects
    {
        public GameObject explosion; //the explosion Game Object
        public bool hasExploded = false; //checks if the torpedo has already detonated

}

    public stats status;
    public effects effect;
    public GameObject target; //target ship
    public GameObject origin; //origin ship
    public float launched; //launch time
    public bool isSpread = false; //checks if the torpedo is already a spread



void FixedUpdate (){
	
	calc_range();
	HomeIn();
	CheckTTargetAndOrigin();
	

}

public void CheckTTargetAndOrigin()
{
	if(!target || !origin)
	{
		Destroy(gameObject);
	}

}


public void calc_range()
{
	int time_passed = (int)(Time.time - launched);
    int distance_made = (int)(time_passed * status.speed * Time.deltaTime);
	
	if (distance_made >= status.range)
	{
		Destroy(gameObject);
	}

}

public void HomeIn()
{
	rigidbody.velocity = status.speed * transform.forward * Time.deltaTime;
	

}


public void OnEnable()
{
	AudioSource audio = gameObject.GetComponent<AudioSource>();
	audio.Play();
	
	
	if(target)
	{	
		transform.LookAt(target.transform);
	}


	rigidbody.velocity = status.speed * transform.forward * Time.deltaTime;
	
	launched = Time.time;
	effect.hasExploded = false;
	
	
	
}


public void OnTriggerEnter(Collider hit)
{

	//check if it's a shield trigger
	if(hit.tag == "Shields" && effect.hasExploded == false)
	{
		GameObject hitGO = getParent(hit.transform).gameObject;
		if(hitGO != origin)
		{
			if (hitGO.tag == "Ship")
			{
				shipTrigger(hitGO);
			}
		}
	
	
	}



}

public void shipTrigger(GameObject hitGO)
{



    ShipOverview hitHS = hitGO.GetComponent<ShipOverview>();
	float shields= hitHS.shieldHealth;
	if(shields > 0)
	{
		effect.hasExploded = true;
		if(shields >= status.shieldDmg)
		{
            hitHS.shieldHealth -= status.shieldDmg;
		}
		else
		{
            hitHS.shieldHealth = 0;
		}
			
		Instantiate(effect.explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}
		

}



public void OnCollisionEnter ( Collision hit  ){
	GameObject go = hit.gameObject;
	Debug.Log(origin.name + "/" + go.name);
	if(hit.gameObject != origin && !effect.hasExploded)
	{		
		if(go.tag == "Ship")
		{
			shipCollision(go);
		}
		else if(go.tag == "Torpedoes")
		{
			Destroy(gameObject);
		}
			
	}

}


private void shipCollision ( GameObject hit  ){
    effect.hasExploded = true;
    ShipHealth hitHS = hit.GetComponent<ShipHealth>();
    hitHS.shipHealth.health -= getDamage(false);
    hitHS.shieldRegen.lastHit = Time.time;
    Instantiate(effect.explosion, transform.position, transform.rotation);
    Destroy(gameObject);
}

public void Destroy(GameObject obj)
{
	obj.SetActive(false);
}

//this method set the target
//pre: target != null
public void setTarget ( GameObject target  ){
	this.target = target;
}
//this method sets the origin
//pre origin != null
public void setOrigin(GameObject origin)
{
	this.origin = getParent(origin.transform).gameObject;
	
}

public float getDamage(bool isShield)
{
	float dmg;
	if(isShield) {
		dmg = status.shieldDmg;
	} else {
		dmg = status.hullDmg;
	}
	return dmg;
}

private Transform getParent ( Transform trans  ){
		Transform par = trans;
		
		while(par.parent) {
			
			par = par.parent.transform;
		}
		
		return par;
	
	}
}