using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodiesOfWater : MonoBehaviour
{
    public static BodiesOfWater Instance {
        get;
        set;
    }

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    // variables
    public bool isWater;

    private void Start() {
        isWater = true;
    }
}
