using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSaveManager : MonoBehaviour
{
    public static MainMenuSaveManager Instance {
        get;
        set;
    }

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

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


}
