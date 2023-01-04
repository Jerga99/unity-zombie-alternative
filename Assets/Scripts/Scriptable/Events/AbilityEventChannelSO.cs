
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Ability Event Channel")]
public class AbilityEventChannelSO : DescriptionBaseSO
{
    public UnityAction<Ability> OnEventRaised;

    public void RaiseEvent(Ability value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
    }
}

