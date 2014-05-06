using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
//this "class" is more here to interface the rest of the game with the several weapon scripts, plus status


    public WeaponType type;
    public float firingAngle;
    public GuiInfo guiInfo;
    public float altRate;

public enum WeaponType
{
	beam,
	torpedo,
	pulse,

}

public class GuiInfo
{
	string name;
	string description;
	Texture2D image;

}



void Start (){
	
}

void Update (){

}

///<summary>Gets the weapon type</summary>
public WeaponType getType()
{
	return type;
}



//this method gets the weapon cooldown
public float getCooldown()
{
	float cd = 0.0f;
	
	switch(type) {
		case WeaponType.beam:
			cd = gameObject.GetComponent<PhaserScript>().standard_cd;
			break;
		case WeaponType.torpedo:
			cd = gameObject.GetComponent<TorpedoScript>().status.cooldown;
			break;
	
	}
	
	return cd;


}

public float getDamage(bool isShield)
{
	float dmg = 0.0f;
	
	switch(type) {
		case WeaponType.beam:
			dmg = gameObject.GetComponent<PhaserScript>().damage;
			break;
		case WeaponType.torpedo:
			dmg = gameObject.GetComponent<TorpedoScript>().getDamage(isShield);
			break;	
	
	}
	
	return dmg;

}

//this method gets the weapon range
public float getRange ()
{
    float range = 0.0f;
	
	switch(type) {
		case WeaponType.beam:
			range = gameObject.GetComponent<PhaserScript>().range;
			break;
		case WeaponType.torpedo:
			range = gameObject.GetComponent<TorpedoScript>().status.range;
			break;

	}
	
	return range;
	
}

//this method checks if the target is in range
public bool isRange(GameObject origin, GameObject target)
{
	 float range = getRange();
	Vector3 or = origin.transform.position;
	Vector3 tar = target.transform.position;
	return ((or - tar).sqrMagnitude <= range * range);


}

//this method check if the target is inside the firing arc
public bool isAngle(GameObject origin, GameObject target)
{

	 return Vector3.Angle(target.transform.position - origin.transform.position, origin.transform.forward) <= firingAngle/2;

}


//this method set the weapon target
//@pre target != null
public void setTarget(GameObject target)
{
	switch(type) {
		
		case WeaponType.torpedo:
			gameObject.GetComponent<TorpedoScript>().setTarget(target);
			break;

	}
}

//this method sets the weapon origin point
//@pre origin != null
public void setOrigin(GameObject origin)
{

	switch(type) {
		
		case WeaponType.torpedo:
			gameObject.GetComponent<TorpedoScript>().setOrigin(origin);
	        break;
	}


}
}