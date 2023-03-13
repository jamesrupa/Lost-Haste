using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    public GameObject Item {
        get {
            if(transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");

        // if there is no item in that slot
        // set our item into slot
        if(!Item) {
            DragAndDrop.itemBeingDragged.transform.SetParent(transform);
            DragAndDrop.itemBeingDragged.transform.localPosition = new Vector2(0,0);
        }
    }

}
