using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour {

    public static AudioManage Instance;
    private AudioSource player;

	// Use this for initialization
	void Start () {
        Instance = this;
        player = GetComponent<AudioSource>(); 
	}
	
    //play Sound
    public void playSound(string name)
    {
        AudioClip Clip = Resources.Load<AudioClip>(name);
        player.PlayOneShot(Clip);
    }
    //stop Sound
    public void stopSound()
    {
        player.Stop();
    }

    // Update is called once per frame
	void Update () {
		
	}
}
