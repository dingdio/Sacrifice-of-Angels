using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {


public GameObject go;
public int number;

private List <GameObject> gos;

void Start (){
	gos = new List<GameObject>();
	for(int i = 0; i < number; i++) {
		addNewObject();
	}
	
}

private bool hasObjectAvailable (){
	 bool  has = false;
	int i = 0;
	while(i < number && !has) {
		has = !gos[i].activeInHierarchy;	
		i++;
	}
	
	return has;
}

public GameObject getObject (){
	if(!hasObjectAvailable()) {
		addNewObject();
		number++;
	}

	GameObject obj = null;
	int i = 0;
	
	while(i < number && obj == null) {
		if(!gos[i].activeInHierarchy) {
			obj = gos[i];
		}
		i++;
	}
	
	return obj;
	
}

public bool equals ( GameObject obj  ){
	return obj == go;
}

private void addNewObject (){
		GameObject obj = Instantiate(go) as GameObject;
		obj.SetActive(false);
		DontDestroyOnLoad(obj);
		gos.Add(obj);
}
}