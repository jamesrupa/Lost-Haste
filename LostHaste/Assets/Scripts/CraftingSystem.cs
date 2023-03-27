using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    // variables
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    // Catergory Buttons
    Button toolsBTN;

    // Craft Buttons
    Button craftAxeBTN;
    
    // requirement text
    Text axeReqOne, axeReqTwo;

    public bool isOpen;


    // all blueprints
    public CraftingBlueprint AxeBLP = new CraftingBlueprint("Axe", 2, "Stone", 3, "Stick", 3);


    public static CraftingSystem Instance {get; set;}

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

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{openToolsCategory();});

        // AXE REQUIREMENTS
        axeReqOne = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        axeReqTwo = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate{craftAnyItem(AxeBLP);});

    }

    void craftAnyItem(CraftingBlueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        if(blueprintToCraft.numOfRequirements == 1) {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        } else if (blueprintToCraft.numOfRequirements == 2) {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());

    }

    public IEnumerator calculate() {
        yield return 0; // no delay
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }


    void openToolsCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // C - crafting system keybind
        // when pressed --> crafting opens
        if(Input.GetKeyDown(KeyCode.C) && !isOpen) {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
        } else if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape)) && isOpen) {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            if(!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }


    public void RefreshNeededItems() {

        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList) {
            switch(itemName) {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        // ----- A X E -----
        axeReqOne.text = "3 Stone [" + stone_count + "]";
        axeReqTwo.text = "3 Stick [" + stick_count + "]";

        if(stone_count >= 3 && stick_count >= 3) {
            craftAxeBTN.gameObject.SetActive(true);
        } else {
            craftAxeBTN.gameObject.SetActive(false);
        }





    }
}
