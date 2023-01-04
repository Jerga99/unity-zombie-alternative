using System;
using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
    public StateCondition GetCondition(StateMachine stateMachine)
    {
        var condition = CreateCondition(stateMachine);
        condition._originSO = this;
        condition.Awake(stateMachine);
        return condition;
    }

    public abstract StateCondition CreateCondition(StateMachine stateMachine);
}

public abstract class StateCondition : IStateComponent
{
    internal ConditionSO _originSO;

    public bool IsMet()
    {
        var statement = Statement();
        return statement;
    }

    protected abstract bool Statement();

    public virtual void Awake(StateMachine stateMachine) { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}
