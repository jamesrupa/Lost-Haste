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
    // holds name for the slots
    public List<string> itemList = new List<string>();
 
   
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
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // add item to the list
        itemList.Add(cleanName);
 
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