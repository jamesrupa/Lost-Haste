using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats;
    // [0] - Health, [1] - Hunger, [2] - Hydration

    public float[] playerPositionAndRotation;
    // position x, y , z & rotation x, y, z


    // public string[] inventoryContent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot) {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
    }
}
