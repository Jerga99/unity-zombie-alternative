using UnityEngine;
using static Eincode.ZombieSurvival.Scriptable.Reward;

public class Lootable : MonoBehaviour
{
    public Loot Loot;

    public void DropItem()
    {
        var roll = Random.Range(0, 100);

        if (roll <= Loot.dropChance)
        {
            Loot.reward.Drop(this);
        }
    }
}

