using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SpawnConfiguration",
    menuName = "Game/Spawn Configuration"
)]
public class SpawnConfigurationSO : ScriptableObject
{
    public List<SpawnConfig> Waves;

    public enum SpawnInterval
    {
        Continuous, AtOnce
    }

    [Serializable]
    public struct SpawnConfig
    {
        public GameObject Prefab;
        public SpawnInterval Interval;
        public int Amount; // -1 for infinite, TODO: Better solution?
    }
}

