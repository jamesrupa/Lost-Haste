using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }
 
    // variables
    public GameObject quickSlotsPanel;
 
    // holds slots
    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
 
   
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    private void Start()
    {
        PopulateSlotList();
    }

    void Update() {

        // quick slot keyboard inputs
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectQuickSlot(1);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectQuickSlot(2);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectQuickSlot(3);
        } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectQuickSlot(4);
        } else if(Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectQuickSlot(5);
        }


    }

    void SelectQuickSlot(int num) {
        if(checkIfSlotIsFull(num)) {

            if(selectedNumber != num) {
                selectedNumber = num;

                // previous item will unselect
                if(selectedItem != null) {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = getSelectedItem(num);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                // change color
                foreach(Transform child in numbersHolder.transform) {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }


                Text toBeChanged = numbersHolder.transform.Find("number" + num).transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.white;
            } else { // selecting the same slot
                selectedNumber = -1; // null

                // previous item will unselect
                if(selectedItem != null) {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                // change color
                foreach(Transform child in numbersHolder.transform) {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
        }
    }

    GameObject getSelectedItem(int slotNum) {
        return quickSlotsList[slotNum - 1].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int slotNum) {

        if(quickSlotsList[slotNum - 1].transform.childCount > 0) {
            return true;
        } else {
            return false;
        }
    }
 
    private void PopulateSlotList()
    {
        // loops over each of the children in the quickSlots
        foreach (Transform child in quickSlotsPanel.transform)
        {
            // checks for the quickslots tag
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }
 
    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // set transform of our object
        // set as new parent
        itemToEquip.transform.SetParent(availableSlot.transform, false);
 
        InventorySystem.Instance.ReCalculateList();
 
    }
 
 
    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }
 
    public bool CheckIfFull()
    {
 
        int counter = 0;
 
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
        // max number of quickSlots
        if (counter == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}