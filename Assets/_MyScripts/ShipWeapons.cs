using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipWeapons : MonoBehaviour {
//this script controls the weapon fire

    [Serializable]
    public class Phaser {
        public bool  isEnabled;
        public GameObject phaser;
        public List<GameObject> phaserPoint = new List<GameObject>();
        public float lastShot;
        public bool isFiring;
        public float duration = 1.0f;
        public LayerMask hullLayerMask;
        public LayerMask shieldLayerMask;

        public void setPhaser(GameObject phaser)
        {
		    this.phaser = phaser;	
	    }
	
	///<summary>This will check if the phaser can fire</summary>
	///<param name="target">Object to be checked (target)</param>
	public bool canFire ( GameObject target  ){
		 bool  can = false;
		if(isEnabled && phaser) {
			bool  isTime = Time.time >= lastShot + getCooldown();
			can = isTime && canRangeAndAngle(target) && !isFiring;
		}
		
		return can;
	
	}

        public bool canRangeAndAngle ( GameObject target  ){
		
		 bool  can = false;
		WeaponScript ws = phaser.GetComponent<WeaponScript>();
		for(int x = 0; x < phaserPoint.Count && !can; x++) {

			if(ws.isRange(phaserPoint[x], target) && ws.isAngle(phaserPoint[x], target)) {
				can = true;			
			}
		}
		
		return can;
	
	}
	
	
	
	
	///<summary>This will get the closest point where the phaser can fire from. Must check canFire(target) first</summary>
	///<param name="target">Target GameObject</param>
	public GameObject getPoint ( GameObject target  ){
		GameObject point = null;
		WeaponScript ws = phaser.GetComponent<WeaponScript>();
		
		foreach(GameObject p in phaserPoint) {
			
			if(!point) {
				if(ws.isRange(p, target) && ws.isAngle(p, target)) {
					point = p;
				}
			} else {
				if(ws.isRange(p, target) && ws.isAngle(p, target)) {
					float o = (point.transform.position - target.transform.position).sqrMagnitude;
					float n = (p.transform.position -target.transform.position).sqrMagnitude;					
					if(n < o) {
						point = p;
					}
				}
			}
		
		}
		
		return point;
	
	}
	
	///<summary>This will fire the phaser. Must check canFire(target) first. Also, make sure this is started as a coroutine.</summary>
	///<param name="target">Target GameObject</param>
    public void fire(GameObject target)
    {
		lastShot = Time.time;
		if(getType() == WeaponScript.WeaponType.beam) {
            Debug.Log("Phaser Firing.");
			fireBeam(target, getPoint(target));
		}	
	
	}
	
	public IEnumerator fireBeam ( GameObject target ,   GameObject origin  ){
		float rate = 1/duration;
		float i = 0;
		GameObject phaserGO = null;
		isFiring = true;
		while (i < 1) {
			if(target){
				i += rate * Time.deltaTime; 
				Vector3 or = origin.transform.position;
				Vector3 ta = target.transform.position;
				Vector3 dir = (ta - or).normalized;
				RaycastHit hit = new RaycastHit();
				
				if(hasTargetShield(target)) {
					phaserGO = fireShields(target, origin, or, dir, hit, phaserGO);
				} else {
					phaserGO = fireHull(target, origin, or, dir, hit, phaserGO);
				}
			} else {
				i = 2;
			}
			
			
			yield return 0;
		}
		GameObject.Destroy(phaserGO);
		isFiring = false;
	}
	
	private bool hasTargetShield ( GameObject target  ){
		 ShipHealth health = target.GetComponent<ShipHealth>();
		return health.isShieldUp();
			
	}
	//pre hasTargetShield()
	private GameObject fireShields ( GameObject target ,   GameObject origin ,   Vector3 or ,   Vector3 dir ,   RaycastHit hit ,   GameObject phaserGO  ){
		if(Physics.Raycast(or, dir, out hit, getRange(), shieldLayerMask)) {
				
		
				//do phaser logic here
				//get target health script
				GameObject ship = hit.transform.gameObject;
				ship.GetComponent<ShipHealth>().damageShield(getDamage() * Time.deltaTime);
				
				//get hit point
				Vector3 point = hit.point;
				
				//draw phaser
				if(phaserGO == null) {
					phaserGO = Instantiate(phaser) as GameObject;
					setLastShot();
				}
				PhaserScript script = phaserGO.GetComponent<PhaserScript>();
				script.setPhaser(origin, target);
				LineRenderer line = script.line_renderer;
				line.SetPosition(0, or);
				line.SetPosition(1, point);
				
				
			} 
			
			return phaserGO;
	
	}
	
	private GameObject fireHull ( GameObject target ,   GameObject origin ,   Vector3 or ,   Vector3 dir ,   RaycastHit hit ,   GameObject phaserGO  ){
		if(Physics.Raycast(or, dir, out hit, getRange(), hullLayerMask)) {
				
				//do phaser logic here
				//get target health script
				GameObject ship = hit.transform.gameObject;
				ship.GetComponent<ShipHealth>().damageHull(getDamage() * Time.deltaTime);
				
				//get hit point
				Vector3 point = hit.point;
				
				//draw phaser
				if(phaserGO == null) {
					phaserGO = GameObject.Instantiate(phaser) as GameObject;
					setLastShot();
				}
				PhaserScript script = phaserGO.GetComponent<PhaserScript>();
				script.setPhaser(origin, target);
				LineRenderer line = script.line_renderer;
				line.SetPosition(0, or);
				line.SetPosition(1, point);
				
				
			}
			
			return phaserGO;
	}
	
	///<summary>This returns the weapon cooldown</summary>
	float getCooldown (){
			WeaponScript ws = phaser.GetComponent<WeaponScript>();
		 	return ws.getCooldown();
	}
	
	///<summary>This return the weapon type</summary>
	WeaponScript.WeaponType getType (){
		return phaser.GetComponent<WeaponScript>().type;
	}
	
	float getRange (){
		return phaser.GetComponent<WeaponScript>().getRange();	
	}
	
	private Transform getParent ( Transform trans  ){
		Transform par = trans;
	
		while(par.parent) {
			par = par.parent.transform;
		}
		
		return par;
	
	}
	
	float getNextShot (){
		return lastShot + getCooldown();	
	}
	
	float getDamage (){
		return phaser.GetComponent<PhaserScript>().damage;
	
	}
	
	void setLastShot (){
		lastShot = Time.time;
	}
	
}

    [Serializable]
    public class Torpedo {
        public bool  isEnabled;
        public GameObject torpedo;
        public GameObject torpedoPoint;
        public float nextShot;
        public float rate;
	
	void setTorpedo ( GameObject torpedo  ){
		this.torpedo = torpedo;
	
	}

        public bool canFire ( GameObject target  ){
		 bool  can = false;
		if(isEnabled && torpedo) {
			bool  isTime = Time.time >= nextShot;
			can = isRange(target) && isAngle(target) && isTime;
		}
		return can;
	}

        public bool isRange ( GameObject target  ){
		 WeaponScript ws = torpedo.GetComponent<WeaponScript>();
		return ws.isRange(torpedoPoint, target);
	}
	
	bool isAngle ( GameObject target  ){
		 WeaponScript ws = torpedo.GetComponent<WeaponScript>();
		return ws.isAngle(torpedoPoint, target);
	}

        public IEnumerator fire ( GameObject target ,   int num  ){
		
		nextShot = Time.time + getCooldown() * num;
		
		for(int x = 0; x < num; x++) {
			GameObject torp = getPooledWeapon();
			if(torp) {
				torp.transform.position = torpedoPoint.transform.position;
				WeaponScript ws = torp.GetComponent<WeaponScript>();
				ws.setTarget(target);
				ws.setOrigin(torpedoPoint);
				torp.SetActive(true);
			yield return new WaitForSeconds(rate);
			} else {
				Debug.LogWarning(torpedoPoint.transform.parent.parent.parent.name + "Isn't getting any torpedo!");
			}
		}
	
	}
	
	///<summary>This returns the weapon cooldown</summary>
	float getCooldown (){
		 return torpedo.GetComponent<WeaponScript>().getCooldown();
	
	}
	
	///<summary>This return the weapon type</summary>
	WeaponScript.WeaponType getType (){
		return torpedo.GetComponent<WeaponScript>().type;
	}
	
	float getRange (){
		return torpedo.GetComponent<WeaponScript>().getRange();	
	}
	
	float getNextShot (){
		return nextShot;	
	}
	
	float getDamage (){
		return torpedo.GetComponent<PhaserScript>().damage;
	
	}
	
	GameObject getPooledWeapon (){
		GameObject[] pools = GameObject.FindGameObjectsWithTag("Pooler");
		GameObject pool = null;
		int i = 0;
		while(i < pools.Length && pool == null) {
			if(pools[i].GetComponent<ObjectPooler>().equals(torpedo)) {
				pool = pools[i];
			}
			i++;
		}
		
		return pool.GetComponent<ObjectPooler>().getObject();
	
	}

}

enum Volley {
	one,
	three,
	five,
	eight

}

//weapons
public Phaser phaser;
public Torpedo torp1;
public Torpedo torp2;

//needed scripts
ShipTarget target;
ShipOverview properties;

//volley
Volley torpVolley;




void Start (){
    target = gameObject.GetComponent<ShipTarget>();
    properties = gameObject.GetComponent<ShipOverview>();

}

void Update (){
	//if(properties.getRedAlert()) {
		fire();
	//}

}

void fire (){

		botFire();
}


void botFire (){
	if(target.target) 
	{
		phaserFunction();
		torpFunction(torp1);
		torpFunction(torp2);
	}
}
     
void phaserFunction (){
	
	if(phaser.canFire(target.target))
	{
	    Debug.Log("Hello form fire");
		StartCoroutine(phaser.fireBeam(target.target, phaser.getPoint(target.target)));	
	}
	
}

bool hasWeaponInRange ( GameObject target  ){
		
	 return hasPhaserInRange(target) || hasTorpedoInRange(target);
}

bool hasPhaserInRange ( GameObject target  ){
	 bool  has = false;
	if(phaser.isEnabled) {
		has = phaser.canRangeAndAngle(target);
	}
	return has;
}

bool hasTorpedoInRange ( GameObject target  ){
	 bool  isTorp1 = false;
	bool  isTorp2 = false;
	
	if(torp1.isEnabled) {
		isTorp1 = torp1.isRange(target);
	}
	
	if(torp2.isEnabled) {
		isTorp2 = torp2.isRange(target);
	}

	return isTorp1 || isTorp2;

}


void torpFunction ( Torpedo torp  ){
	if(torp.canFire(target.target)) {
		StartCoroutine(torp.fire(target.target, volleyNum()));
	}


}

int volleyNum (){
	int num;
	
	switch(torpVolley) {
		case Volley.three:
			num = 3;
		break;
		case Volley.five: 
			num = 5;
		break;
		case Volley.eight:
			num = 8;
		break;
		default:
			num = 1;

	   break;
	}
	
	

	return num;
}

}