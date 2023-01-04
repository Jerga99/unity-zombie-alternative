using System;
using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    public abstract class Reward : ScriptableObject
    {
        public enum RewardType
        {
            Experience,
            NewAbility
        }

        [Serializable]
        public struct Loot
        {
            public Reward reward;
            public float dropChance;
        }

        public GameObject RewardPrefab;
        public RewardType Type;
        public string Name;

        public virtual void Drop(MonoBehaviour source) { }
    }
}

