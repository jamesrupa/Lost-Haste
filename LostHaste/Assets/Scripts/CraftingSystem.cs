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
    public GameObject toolsScreenUI, refineScreenUI, constructionScreenUI, healthScreenUI;


    public List<string> inventoryItemList = new List<string>();

    // Category Buttons
    Button toolsBTN, refineBTN, constructionBTN, healthBTN;

    // Craft Buttons
    Button craftAxeBTN, craftPlankBTN, craftFoundationBTN, craftWallBTN;
    
    // requirement text
    Text axeReqOne, axeReqTwo, plankReq1, foundationReq1, wallReq1;

    public bool isOpen;


    // all blueprints
    public CraftingBlueprint AxeBLP = new CraftingBlueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
    public CraftingBlueprint PlankBLP = new CraftingBlueprint("Plank", 2, 1, "Log", 1, "", 0);
    public CraftingBlueprint FoundationBLP = new CraftingBlueprint("Foundation", 1, 1, "Plank", 4, "", 0);
    public CraftingBlueprint WallBLP = new CraftingBlueprint("Wall", 1, 1, "Plank", 2, "", 0);


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

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate{openRefineCategory();});

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate{openConstructionCategory();});

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

        // FOUNDATION REQUIREMENTS
        foundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("Req1").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate{craftAnyItem(FoundationBLP);});

        // WALL REQURIEMENTS
        wallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("Req1").GetComponent<Text>();

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate{craftAnyItem(WallBLP);});

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

        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        healthScreenUI.SetActive(false);
    }

    void openRefineCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);

        constructionScreenUI.SetActive(false);
        healthScreenUI.SetActive(false);
    }

    void openConstructionCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionScreenUI.SetActive(true);

        healthScreenUI.SetActive(false);
    }

    void openHealthCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        healthScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // C - crafting system keybind
        // when pressed --> crafting opens
        if(Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode) {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        } else if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape)) && isOpen) {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);
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
        int plank_count = 0;

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
                case "Plank":
                    plank_count += 1;
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

        // --- F O U N D A T I O N -- //
        foundationReq1.text = "4 Plank [" + plank_count + "]";

        if(plank_count >= 4 && InventorySystem.Instance.CheckSlotsAvaliable(1)) {
            craftFoundationBTN.gameObject.SetActive(true);
        } else {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        // --- W A L L --- //
        wallReq1.text = "2 Plank [" + plank_count + "]";

        if(plank_count >= 2 && InventorySystem.Instance.CheckSlotsAvaliable(1)) {
            craftWallBTN.gameObject.SetActive(true);
        } else {
            craftWallBTN.gameObject.SetActive(false);
        }

    }
}
