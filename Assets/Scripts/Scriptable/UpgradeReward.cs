using Eincode.ZombieSurvival.Actions;
using UnityEngine;

namespace Eincode.ZombieSurvival.Scriptable
{
    [CreateAssetMenu(
        fileName = "Data",
        menuName = "ScriptableObjects/UpgradeReward",
        order = 5)
    ]
    public class UpgradeReward : Reward
    {
        public override void Drop(MonoBehaviour source)
        {
            var actionGo = Instantiate(
                RewardPrefab,
                source.transform.position,
                Quaternion.identity
            );

            var dropItem = actionGo.GetComponent<DropItem>();
            dropItem.isStatic = true;
        }
    }
}