using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all interactable objects use this script
public class InteractableObject : MonoBehaviour
{

    //name of object
    public string ItemName;

    public string GetItemName() {
        return ItemName;
    }
}
