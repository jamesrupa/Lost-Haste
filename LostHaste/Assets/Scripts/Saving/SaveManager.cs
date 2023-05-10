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


    private void Start() {
        binaryPath = Application.persistentDataPath + "/save_game.bin";
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
    }

    #region  || --- GENERAL SECTION --- ||

    #region  || --- SAVING SECTION --- ||
    public void SaveGame() {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();
        SavingTypeSwitch(data);
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

        return new PlayerData(playerStats, playerPosAndRot);

    }
    public void SavingTypeSwitch(AllGameData gameData) {
        if(isSavingToJson) {
            SaveGameDataToJsonFile(gameData);
        } else {
            SaveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region  || --- LOADING SECTION --- ||

    public AllGameData LoadingTypeSwitch() {
        if(isSavingToJson) {
            AllGameData gameData = LoadGameDataFromJsonFile();
            return gameData;
        } else {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame() {

        // player data
        SetPlayerData(LoadingTypeSwitch().playerData);
        // environment data
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

    }

    public void StartLoadedGame() {
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(1f);
        LoadGame();
    }


    #endregion
    #endregion

    #region  || --- TO BINARY SECTION --- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData) {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("DATA SAVED TO " + binaryPath);
    }

    public AllGameData LoadGameDataFromBinaryFile() {

        if(File.Exists(binaryPath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("DATA LOADED FROM " + binaryPath);

            return data;
        } else {
            return null;
        }
    }

    #endregion

    #region  || --- TO JSON SECTION --- ||

    public void SaveGameDataToJsonFile(AllGameData gameData) {
        string json = JsonUtility.ToJson(gameData);
        
        // encryption
        string encrypted = EncryptionDecryption(json);

        using(StreamWriter writer = new StreamWriter(jsonPathProject)) {
            writer.Write(encrypted);
            print("SAVED GAME TO JSON FILE AT: " + jsonPathProject);
        };
    }

    public AllGameData LoadGameDataFromJsonFile() {
        using(StreamReader reader = new StreamReader(jsonPathProject)) {
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
}
