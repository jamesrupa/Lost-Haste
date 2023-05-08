using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public static SoundManager Instance {
        get; 
        set;
    }

    // --- SOUNDS --- //
    public AudioSource itemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource grassWalkSound;
    
    // --- MUSIC --- //
    public AudioSource gameMusic;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public void playSound(AudioSource sound) {
        if(!sound.isPlaying) {
            sound.Play();
        }
    }

    

}
