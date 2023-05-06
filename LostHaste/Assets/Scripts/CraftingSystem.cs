using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    // variables
    // screens
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, healthScreenUI;


    public List<string> inventoryItemList = new List<string>();

    // Category Buttons
    Button toolsBTN, survivalBTN, refineBTN, healthBTN;

    // Craft Buttons
    Button craftAxeBTN, craftPlankBTN;
    
    // requirement text
    Text axeReqOne, axeReqTwo, plankReq1;

    public bool isOpen;


    // all blueprints
    public CraftingBlueprint AxeBLP = new CraftingBlueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
    public CraftingBlueprint PlankBLP = new CraftingBlueprint("Plank", 2, 1, "Log", 1, "", 0);


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

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate{openSurvivalCategory();});

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate{openRefineCategory();});

        healthBTN = craftingScreenUI.transform.Find("HealthButton").GetComponent<Button>();
        healthBTN.onClick.AddListener(delegate{openHealthCategory();});


        // AXE REQUIREMENTS
        axeReqOne = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        axeReqTwo = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate{craftAnyItem(AxeBLP);});

        // PLANK REQURIEMENTS
        plankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("Req1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate{craftAnyItem(PlankBLP);});

    }

    void craftAnyItem(CraftingBlueprint blueprintToCraft)
    {

        //sound
        SoundManager.Instance.playSound(SoundManager.Instance.craftingSound);

        StartCoroutine(craftDelaySound(blueprintToCraft));

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

    public IEnumerator craftDelaySound(CraftingBlueprint blueprintToCraft) {
        yield return new WaitForSeconds(3f);

        // produce the amount of items you want according to blueprint
        for(var i = 0; i < blueprintToCraft.numOfItemProduced; i++) {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }

    }


    void openToolsCategory() {
        craftingScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);

        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        healthScreenUI.SetActive(false);
    }

    void openSurvivalCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);

        refineScreenUI.SetActive(false);
        healthScreenUI.SetActive(false);
    }

    void openRefineCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);

        healthScreenUI.SetActive(false);
    }

    void openHealthCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        healthScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // C - crafting system keybind
        // when pressed --> crafting opens
        if(Input.GetKeyDown(KeyCode.C) && !isOpen) {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        } else if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape)) && isOpen) {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            healthScreenUI.SetActive(false);
            if(!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }


    public void RefreshNeededItems() {

        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList) {
            switch(itemName) {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Log":
                    log_count += 1;
                    break;
            }
        }

        // ----- A X E ----- //
        axeReqOne.text = "3 Stone [" + stone_count + "]";
        axeReqTwo.text = "3 Stick [" + stick_count + "]";

        if(stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvaliable(1)) {
            craftAxeBTN.gameObject.SetActive(true);
        } else {
            craftAxeBTN.gameObject.SetActive(false);
        }


        // --- P L A N K   X 2 --- //
        plankReq1.text = "1 Log [" + log_count + "]";

        if(log_count >= 1 && InventorySystem.Instance.CheckSlotsAvaliable(2)) {
            craftPlankBTN.gameObject.SetActive(true);
        } else {
            craftPlankBTN.gameObject.SetActive(false);
        }


    }
}
