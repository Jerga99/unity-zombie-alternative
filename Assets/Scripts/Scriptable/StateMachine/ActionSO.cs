using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateActionSO : ScriptableObject
{
    public StateAction GetAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
    {
        if (createdInstances.TryGetValue(this, out var instance))
        {
            return (StateAction)instance;
        }

        var action = CreateAction(stateMachine);
        createdInstances.Add(this, action);

        action._originSO = this;
        action.Awake(stateMachine);
        return action;
    }

    public abstract StateAction CreateAction(StateMachine stateMachine);
}

public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
{
    public override StateAction CreateAction(StateMachine stateMachine) => new T();
}


public abstract class StateAction : IStateComponent
{
    public StateActionSO OriginSO => _originSO;
    internal StateActionSO _originSO;

    public abstract void Awake(StateMachine stateMachine);

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnUpdate() { }
}

