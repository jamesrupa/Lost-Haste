using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance {
        get;
        set;
    }

    // variables
    public GameObject inventoryScreenUI;
    public bool isOpen;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isFull;

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
        isFull = false;

        PopulateSlotList();
    }

    // adds slots to slotList
    // children of inventory screen
    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform) {
            if(child.CompareTag("Slot")) {
                slotList.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // I - inventory keybind
        // when pressed --> inventory opens
        if(Input.GetKeyDown(KeyCode.I) && !isOpen) {
            Debug.Log("I pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        } else if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && isOpen) {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName) {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList) {
            // if it doesnt have children == avaliable/empty

            if(slot.transform.childCount == 0) {
                return slot;
            }

        }
        // never gets to this point

        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach(GameObject slot in slotList) {

            if(slot.transform.childCount > 0) {
                counter += 1;
            }
        }
        // inventory max slots 
        if(counter == 18) {
                return true;
        } else {
            return false;
        }

    }
}
