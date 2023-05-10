using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    private void Awake() {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        button.onClick.AddListener(() =>
        {
            if(SaveManager.Instance.isSlotEmpty(slotNumber) == false) {
                SaveManager.Instance.StartLoadedGame(slotNumber);
                SaveManager.Instance.DeselectButton();
            } else {
                // if empty do nothing
            }
        }
        );
    }

    private void Update() {
        if(SaveManager.Instance.isSlotEmpty(slotNumber)) {
            buttonText.text = "";
        } else {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

}
