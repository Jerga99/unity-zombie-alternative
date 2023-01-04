using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Int Event Channel")]
public class IntIntEventChannelSO : DescriptionBaseSO
{
    public UnityAction<int, int> OnEventRaised;

    public void RaiseEvent(int value1, int value2)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value1, value2);
        }
    }
}

