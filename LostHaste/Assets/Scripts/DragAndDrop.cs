using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    // variables
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f; // adds transparency
        // ray cast will ignore item
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root); // sets item to be child or root (Inventory rather than slot)
        itemBeingDragged = gameObject;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta;
        // item will move at the same time and speed with our mouse
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemBeingDragged = null;

        if(transform.parent == startParent || transform.parent == transform.root) {
            // only become a child of a slot
            // if not returns to original slot
            transform.position = startPosition;
            transform.SetParent(startParent);
        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f; // original transparency
        canvasGroup.blocksRaycasts = true;
    }

}
