using UnityEngine;
using System.Collections;

public class MusicPlayerCode : MonoBehaviour {

    public AudioClip startKlingon;
	
	void Start()
	{
		//audio.loop = true;
		//audio.clip = MusicToPlay;
		//audio.Play();
	}

    public void StartKlingon()
    {
        audio.loop = true;
        audio.clip = startKlingon;
        audio.Play();
    }
}
