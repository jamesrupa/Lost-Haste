using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {
        get;
        set;
    }

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    // variables
    public bool isSavingToJson;

    // binary save path
    string binaryPath;
    // json project save path
    // used when working on project through unity
    string jsonPathProject;
    // json external save path --> on pc files where game will be played
    // use when build and create executable
    string jsonPathPersistant;

    string fileName = "SaveGame";

    public bool isLoading;

    public Canvas loadingScreen;


    private void Start() {
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region  || --- GENERAL SECTION --- ||

    #region  || --- SAVING SECTION --- ||
    public void SaveGame(int slotNumber) {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();
        data.environmentData = GetEnvironmentData();

        SavingTypeSwitch(data, slotNumber);
    }

    private EnvironmentData GetEnvironmentData()
    {
        List<string> itemsPickedUp = InventorySystem.Instance.itemsPickedUp;

        return new EnvironmentData(itemsPickedUp);
    }

    private PlayerData GetPlayerData() {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentHunger;
        playerStats[2] = PlayerState.Instance.currentHydration;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.player.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.player.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.player.transform.position.z;
        playerPosAndRot[3] = PlayerState.Instance.player.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.player.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.player.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();

        string[] quickSlots = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerPosAndRot, inventory, quickSlots);

    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();

        foreach(GameObject slot in EquipSystem.Instance.quickSlotsList) {
            if(slot.transform.childCount != 0) {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber) {
        if(isSavingToJson) {
            SaveGameDataToJsonFile(gameData, slotNumber);
        } else {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }

    #endregion

    #region  || --- LOADING SECTION --- ||

    public AllGameData LoadingTypeSwitch(int slotNumber) {
        if(isSavingToJson) {
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;
        } else {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber) {

        // player data
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);
        // environment data
        SetEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);

        isLoading = false;

        DisableLoadingScreen();
    }

    private void SetEnvironmentData(EnvironmentData environmentData) {
        foreach(Transform itemType in EnvironmentManager.Instance.allItems.transform) {
            foreach(Transform item in itemType.transform) {
                if(environmentData.pickedUpItems.Contains(item.name)) {
                    Destroy(item.gameObject);
                }
            }
        }
        // repopulates inventory system list
        InventorySystem.Instance.itemsPickedUp = environmentData.pickedUpItems;
    }

    private void SetPlayerData(PlayerData playerData)
    {
        // set player stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentHunger = playerData.playerStats[1];
        PlayerState.Instance.currentHydration = playerData.playerStats[2];

        // set player position
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        // set player rotation
        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.player.transform.rotation = Quaternion.Euler(loadedRotation);

        // inventory
        foreach(string item in playerData.inventoryContent) {
            InventorySystem.Instance.AddToInventory(item);
        }

        // quick slots
        foreach(string item in playerData.quickSlotsContent) {
            GameObject avaliableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
            itemToAdd.transform.SetParent(avaliableSlot.transform, false);
        }

        isLoading = false;
    }

    public void StartLoadedGame(int slotNumber) {
        ActivateLoadingScreen();
        isLoading = true;
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);
    }


    #endregion
    #endregion

    #region  || --- TO BINARY SECTION --- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber) {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("DATA SAVED TO " + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber) {

        if(File.Exists(binaryPath + fileName + slotNumber + ".bin")) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("DATA LOADED FROM " + binaryPath + fileName + slotNumber + ".bin");

            return data;
        } else {
            return null;
        }
    }

    #endregion

    #region  || --- TO JSON SECTION --- ||

    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber) {
        string json = JsonUtility.ToJson(gameData);
        
        // encryption
        string encrypted = EncryptionDecryption(json);

        using(StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json")) {
            writer.Write(encrypted);
            print("SAVED GAME TO JSON FILE AT: " + jsonPathProject + fileName + slotNumber + ".json");
        };
    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber) {
        using(StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json")) {
            string json = reader.ReadToEnd();

            // decrypt
            string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            return data;
        };
    }

    // --- JSON ENCRYPTION --- //

    public string EncryptionDecryption(string jsonString) {

        string keyword = "cisc4900_2023";
        // :) Thanks for the great semester
        // Had a blast working on this project & can't wait to see what the future holds
        // Thanks for all the help & have a great summer
        string result = "";

        for(int i = 0; i < jsonString.Length; i++) {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result; // encryption or decryption string

        // ^ = XOR --> "is the difference of"
    }


    #endregion

    #region  || --- VOLUME SETTINGS SECTION --- ||
    [System.Serializable]
    public class VolumeSettings {
        public float music, effects, master;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master) {
        VolumeSettings volumeSettings = new VolumeSettings() {
            music = _music,
            effects = _effects,
            master = _master,
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();

        print("VOLUME SETTINGS SAVED");
    }

    public VolumeSettings LoadVolumeSettings() {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }

    #endregion

    #region  || --- UTILITY SECTION --- ||

    public bool DoesFileExist(int slotNumber) {
        if(isSavingToJson) {
            // SaveGame1.json
            // SaveGame2.json
            if(System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json")) {
                return true;
            } else {
                return false;
            }

        } else {
            if(System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin")) {
                return true;
            } else {
                return false;
            }
        }
    }

    public bool isSlotEmpty(int slotNumber) {
        if(SaveManager.Instance.DoesFileExist(slotNumber)) {
            return false;
        } else {
            return true;
        }
    }

    public void DeselectButton() {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion

    #region  || --- LOADING SCREEN SECTION --- ||

    public void ActivateLoadingScreen() {
        loadingScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // music while loading
        // animation
        // show
    }

    public void DisableLoadingScreen() {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion
}
