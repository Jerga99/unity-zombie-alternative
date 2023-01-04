using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AbilityHolder : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textHolder;
    public int abilityIndex;

    public UnityEvent<int> clickEvent;

    private void Start()
    {
        textHolder = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ChangeText(string newText)
    {
        textHolder.text = newText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Invoke(abilityIndex);
    }
}

