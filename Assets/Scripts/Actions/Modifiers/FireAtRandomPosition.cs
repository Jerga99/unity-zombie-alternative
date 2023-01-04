using Eincode.ZombieSurvival.Actions;
using UnityEngine;

[CreateAssetMenu(
    fileName = "FireAtRandomPosition",
    menuName = "Abilities/Modifiers/FireAtRandomPosition"
)]
public class FireAtRandomPosition : ActionModifierSO
{
    internal override void UpdateAction(AbilityAction action)
    {
        if (action.direction == Vector3.zero)
        {
            action.direction = Random.insideUnitCircle.normalized;
            action.transform.rotation = RotateAction(action);
        }

        var distance = Time.deltaTime * 2.0f;

        action.transform.position += action.direction * distance;
        action.distanceTraveled += distance;

        if (action.distanceTraveled > action.range)
        {
            action.distanceTraveled = 0;
            action.direction = Vector3.zero;
            action.Release();
        }
    }

    private Quaternion RotateAction(AbilityAction action)
    {
        float angle = Mathf.Atan2(-action.direction.y, -action.direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

