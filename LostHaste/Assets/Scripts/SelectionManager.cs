using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {get; set;}

    public GameObject interactionInfoUI;
    // uses legacy Text UI not TextMeshPro
    Text interactionText;
    public bool onTarget;
    public GameObject selectedObject;
    // Start is called before the first frame update
    void Start()
    {
        onTarget = false;
        interactionText = interactionInfoUI.GetComponent<Text>();
    }

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
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
        if(Physics.Raycast(ray, out hit)) {
            var selectionTransform = hit.transform;
            // change its property
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            // if object has interactable script (true)
            // change text
            if(interactable && interactable.playerInRange) {

                onTarget = true;
                selectedObject = interactable.gameObject;

                interactionText.text = interactable.GetItemName();
                interactionInfoUI.SetActive(true);
            } else {
                onTarget = false;
                interactionInfoUI.SetActive(false);
            }
            // if no longer looking at object turn off InteractionInfoUI
        } else {
            onTarget = false;
            interactionInfoUI.SetActive(false);
        }
    }
}
