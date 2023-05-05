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
    public AudioSource dropItemSound;
    public AudioSource pickupItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource grassWalkSound;
    

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
