using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{

    public List<string> pickedUpItems;
    public EnvironmentData(List<string> _pickedUpItems) {
        pickedUpItems = _pickedUpItems;
    }
}