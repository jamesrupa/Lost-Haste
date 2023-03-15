using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all interactable objects use this script
public class InteractableObject : MonoBehaviour
{

    
    public bool playerInRange;
    //name of object
    public string ItemName;

    public string GetItemName() {
        return ItemName;
    }

    void Update() {
        // item pickup with E when player is in range & mouse is on object
        if(Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.onTarget) {
            // if inventory isn't full
            if(!InventorySystem.Instance.CheckIfFull()) {
                Debug.Log("Item Added to Inventory");
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject); 
            } else {
                Debug.Log("Inventory Full");
            }
        }
    }

    // enter = player in area of object
    // exit = player left area of object
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            playerInRange = false;
        }
    }
}
