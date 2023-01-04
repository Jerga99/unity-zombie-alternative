using System;
using Eincode.ZombieSurvival.Actions;
using UnityEngine;

public abstract class ActionModifierSO : ScriptableObject
{
    internal abstract void UpdateAction(AbilityAction action);
}

