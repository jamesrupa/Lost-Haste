using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance {
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
    
    // variables
    public Slider masterSlider, musicSlider, effectsSlider;
    public GameObject masterValue, musicValue, effectsValue;
    public Button ApplyButton;

    private void Start() {
        ApplyButton.onClick.AddListener(() => 
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
        });

        StartCoroutine(LoadAndApplySettings());
        // fix UI bug with button
        ApplyButton.enabled = false;
        ApplyButton.enabled = true;
    }

    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        SaveManager.VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

        // set volume settings
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;

        print("VOLUME SETTINGS LOADED");
    }

    private void Update() {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }
}
