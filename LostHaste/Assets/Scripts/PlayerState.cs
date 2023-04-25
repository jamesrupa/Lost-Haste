using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance {
        get; 
        set;
    }

    // --- Player Health ---
    public float currentHealth;
    public float maxHealth;

    // --- Player Hunger ---
    public float currentHunger;
    public float maxHunger;

    float distanceTraveled = 0;
    Vector3 lastPosition;

    public GameObject player;

    // --- Player Hydration ---
    public float currentHydration;
    public float maxHydration;


    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start() {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHydration = maxHydration;
        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration() {
        while(true) {

            currentHydration -= 1;
            yield return new WaitForSeconds(10);
            // every 5 seconds 1 hydration while decrease
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveled += Vector3.Distance(player.transform.position, lastPosition);
        lastPosition = player.transform.position;

        if(distanceTraveled >= 5) {
            distanceTraveled = 0;
            currentHunger -= 1;
        }



        //test for status bars
        if(Input.GetKeyDown(KeyCode.N)) {
            currentHealth -= 10;
        }
    }
}
