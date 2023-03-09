using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameObject interactionInfoUI;
    // uses legacy Text UI not TextMeshPro
    Text interactionText;
    // Start is called before the first frame update
    void Start()
    {
        interactionText = interactionInfoUI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // raycast points to center of screen
        // where mouse pointer is locked
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // if something is hit with raycast (true)
        // distance set to 2.5f
        if(Physics.Raycast(ray, out hit, 2.5f)) {
            var selectionTransform = hit.transform;
            // change its property

            // if object has interactable script (true)
            // change text
            if(selectionTransform.GetComponent<InteractableObject>()) {

                interactionText.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interactionInfoUI.SetActive(true);
            } else {
                interactionInfoUI.SetActive(false);
            }
            // if no longer looking at object turn off InteractionInfoUI
        } else {
            interactionInfoUI.SetActive(false);
        }
    }
}
