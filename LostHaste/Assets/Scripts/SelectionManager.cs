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


    public Image whiteDotImage;
    public Image handIcon;
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

                if(interactable.CompareTag("pickable")) {
                    whiteDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                } else {
                    handIcon.gameObject.SetActive(false);
                    whiteDotImage.gameObject.SetActive(true);
                }

            } else {
                onTarget = false;
                interactionInfoUI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                whiteDotImage.gameObject.SetActive(true);
            }
            // if no longer looking at object turn off InteractionInfoUI
        } else {
            onTarget = false;
            interactionInfoUI.SetActive(false);
            handIcon.gameObject.SetActive(false);
            whiteDotImage.gameObject.SetActive(true);
        }
    }

    public void DisableSelection() {
        handIcon.enabled = false;
        whiteDotImage.enabled = false;
        interactionInfoUI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection() {
        handIcon.enabled = true;
        whiteDotImage.enabled = true;
        interactionInfoUI.SetActive(true);
    }
}
