using UnityEngine;
using System.Collections;

public class PhaserScript : MonoBehaviour {
//this script controls the phaser beam


public float range;
public float damage;


public LineRenderer line_renderer;

public float standard_cd = 5.0f;


public GameObject origin;
public GameObject target;


void Update (){
	
	if(!origin || !target) {
		Destroy(gameObject);
	}
	if(origin) changeSoundFocus();

}

public void setPhaser ( GameObject origin ,   GameObject target  ){
	this.origin = origin;
	this.target = target;
}

public void setRenderer(Vector3 origin, Vector3 target)
{
	line_renderer.SetPosition(0, origin);
	line_renderer.SetPosition(0, target);

}

public void changeSoundFocus()
{
	transform.position = origin.transform.position;
}



}