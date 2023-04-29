using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
 
public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
 
    // variables
    public GameObject trashAlertUI;
 
    private Text textToModify;
 
    public Sprite trash_closed;
    public Sprite trash_opened;
 
    private Image imageComponent;
 
    Button YesBTN, NoBTN;
 
    GameObject draggedItem
    {
        get
        {
            return DragAndDrop.itemBeingDragged;
        }
    }
 
    GameObject itemToBeDeleted;
  
 
 
    public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }
 
 
 
    void Start()
    {
        // grabs all necessary components
        imageComponent = transform.Find("Background").GetComponent<Image>();
 
        textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();
 
        YesBTN = trashAlertUI.transform.Find("Yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });
 
        NoBTN = trashAlertUI.transform.Find("No").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
 
    }
 
 
    public void OnDrop(PointerEventData eventData)
    {
        //itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;
 
            StartCoroutine(notifyBeforeDeletion());
        }
        
    }
 
    IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = "Throw away this " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }
 
    private void CancelDeletion()
    {
        imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }
 
    private void DeleteItem()
    {
        imageComponent.sprite = trash_closed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }
 
    public void OnPointerEnter(PointerEventData eventData)
    {
 
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_opened;
        }
       
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_closed;
        }
    }
 
}