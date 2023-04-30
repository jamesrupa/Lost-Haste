using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance {
        get;
        set;
    }

    // variables
    public GameObject inventoryScreenUI;
    public GameObject ItemInfoUI;

    public bool isOpen;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public GameObject ItemPickUpAlert;
    public Text pickupName;
    public Image pickupImage;

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

        Cursor.visible = false;
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
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        } else if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && isOpen) {
            inventoryScreenUI.SetActive(false);
            if(!CraftingSystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
                
            }
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName) {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    void TriggerPopUp(string itemName, Sprite itemSprite) {
        ItemPickUpAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(CountDown());
        
    }

    IEnumerator CountDown() {
        yield return new WaitForSeconds(1);
        ItemPickUpAlert.SetActive(false);
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

    public void RemoveItem(string nameToRemove, int amountToRemove) {

        int counter = amountToRemove;

        // start at last inventory slot and go through inventory backwards
        for(var i = slotList.Count - 1; i >= 0; i--) {

            if(slotList[i].transform.childCount > 0 ) {
                if(slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0) {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    public void ReCalculateList() {
        itemList.Clear();

        foreach(GameObject slot in slotList) {

            if(slot.transform.childCount > 0) {

                string name = slot.transform.GetChild(0).name; 
                //how it appears --> Item (Clone)
                string str1 = "(Clone)";
                string result = name.Replace(str1, "");

                itemList.Add(result);
            }
        }
    }
}
