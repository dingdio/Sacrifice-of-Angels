using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ShipHealth : MonoBehaviour {

    [Serializable]
    public class ship_Health {
    public float maxHealth;
    public float health;
    public float maxShields;
    public float shields;

    public bool hasShield (){
		 return shields > 0;	
	}
	
	//pre hasShield()
    public void shieldDamage ( float damage  ){
		shields -= damage;	
	}

    public void hullDamage ( float damage  ){
		health -= damage;
	}

}
    [Serializable]
    public class ShieldsShow {
    public float lastHit;
    public float showDur = 1.0f;

    public void setHit (){
		lastHit = Time.time;
	}
	
}
    [Serializable]
    public class ShieldRegeneration {
    public bool  isRegen = false; //checks if the ships shields can regenerate
    public float lastHit; //checks last hit by a weapon
    public float timeInt; //contains time interval from last hit before the shield start regenerating
    public float regenRate; //contains the regeneration rate

}

public ship_Health shipHealth;
public ShieldsShow shieldShow;
public ShieldRegeneration shieldRegen;
public ShipOverview properties;
public GameObject explosion;
public GameObject shield;


void Start (){

	//get other scripts
	properties = gameObject.GetComponent<ShipOverview>();
	
	//get health stats
	shipHealth.maxHealth = properties.hullHealth;
	shipHealth.health = shipHealth.maxHealth;
	
	//get shield stats
	shipHealth.maxShields = properties.shieldHealth;
	shipHealth.shields = shipHealth.maxShields;
	

}

void Update (){

	updateHealth();
	Die();
	ShieldShow();
	shield_regen();

}

//this function controls a ship shield regeneration
public void shield_regen (){
	//checks if the time interval has passed and the shield is able of regenerating
	if (shieldRegen.lastHit + shieldRegen.timeInt <= Time.time && shieldRegen.isRegen == true && shipHealth.shields < shipHealth.maxShields)
	{
	
		shipHealth.shields += shieldRegen.regenRate * Time.deltaTime; //adds shield
	
	}

}

public void updateHealth (){

	shipHealth.maxHealth = properties.hullHealth;

}

public void Die (){

	if (shipHealth.health <= 0)
	{
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);	
	}
	

}


public Vector3 calculatePosition ( Vector3 position ,   float drift  ){

	Vector3 v = Random.insideUnitSphere;
	
	for(int i = 0;  i < 3; i++) {
		v[i] = v[i] * drift;
	}
	
	return v + position;

}




void ShieldShow (){
	if(shield) {
		if (Time.time <= shieldShow.lastHit + shieldShow.showDur && shield.renderer.enabled == true && shieldShow.lastHit != 0)
		{
			float totTime = shieldShow.showDur + shieldShow.lastHit;
			float remTime = totTime - Time.time;
			
			
			
			float alpha = (1 * remTime)/shieldShow.showDur;

		    Color color = shield.renderer.material.color;
            shield.renderer.material.color = new Color(color.r, color.g, color.b, alpha);
			
		
		}
		else
		{
            Color color = shield.renderer.material.color;
            shield.renderer.material.color = new Color(color.r, color.g, color.b, 0);
		}
	}

}

void OnDestroy (){
	
	gameObject.SetActive(false);
 
}

    public bool isShieldUp (){

	 return shipHealth.hasShield();
    }

//pre: isShieldUp()
    public void damageShield ( float damage  ){
	    shipHealth.shieldDamage(damage);
	    shieldShow.setHit();

    }

//pre: !isShieldUp()
    public void damageHull ( float damage  ){
	    shipHealth.hullDamage(damage);

    }

void showShields (){
	shieldShow.setHit();

}

//note: there're more conditions to be checked
void setDamage ( float damage  ){


	if(shipHealth.shields - damage > 0) {
		shipHealth.shields -= damage;
		shieldShow.setHit();
	} else if (shipHealth.shields > 0) {
		shipHealth.shields = 0;
		shieldShow.setHit();
	} else {
		shipHealth.health -= damage;
	}
}
}