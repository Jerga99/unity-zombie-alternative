
using Eincode.ZombieSurvival.Manager;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Ability Upgrade Event Channel")]
public class AbilityUpgradeEventChannelSO : DescriptionBaseSO
{
    public UnityAction<AbilityUpgrade> OnEventRaised;

    public void RaiseEvent(AbilityUpgrade value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
    }
}

