using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance {
        get;
        set;
    }

    public GameObject inventoryScreenUI;
    public bool isOpen;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // I - inventory keybind
        // when closed --> inventory opens
        if(Input.GetKeyDown(KeyCode.I) && !isOpen) {
            Debug.Log("I pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        } else if (Input.GetKeyDown(KeyCode.I) && isOpen) {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }
}
