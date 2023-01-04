using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StateSO",
    menuName = "StateMachine/New State"
)]
public class StateSO : ScriptableObject
{
    [SerializeField]
    private StateActionSO[] _actions;

    [SerializeField]
    private ConditionSO[] _conditions;

    public virtual State GetState(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
    {
        if (createdInstances.TryGetValue(this, out var instance))
        {
            return (State)instance;
        }

        var state = new State();
        createdInstances.Add(this, state);

        state._originSO = this;
        state._conditions = _conditions == null ? new StateCondition[0] : GetConditions(stateMachine);
        state._actions = _actions == null ? new StateAction[0] : GetActions(stateMachine, createdInstances);

        return state;
    }

    private StateAction[] GetActions(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
    {
        var actions = new StateAction[_actions.Length];
        for (var i = 0; i < actions.Length; i++)
        {
            var action = _actions[i].GetAction(stateMachine, createdInstances);
            actions[i] = action;
        }

        return actions;
    }

    private StateCondition[] GetConditions(StateMachine stateMachine)
    {
        var conditions = new StateCondition[_conditions.Length];
        for (var i = 0; i < conditions.Length; i++)
        {
            var condition = _conditions[i].GetCondition(stateMachine);
            conditions[i] = condition;
        }

        return conditions;
    }
}

public class State
{
    internal StateSO _originSO;
    internal StateAction[] _actions;
    internal StateCondition[] _conditions;

    // Curently all condition must be met in order to transition state
    // later maybe OR conditions as well

    public State() { }

    public bool CanTransition()
    {
        bool isMet = false;
        for (var i = 0; i < _conditions.Length; i++)
        {
            var condition = _conditions[i];
            isMet = condition.IsMet();

            if (!isMet)
            {
                break;
            }
        }

        return isMet;
    }

    public void OnStateEnter()
    {
        for (var i = 0; i < _actions.Length; i++)
        {
            _actions[i].OnStateEnter();
        }
    }

    public void OnStateExit()
    {
        for (var i = 0; i < _actions.Length; i++)
        {
            _actions[i].OnStateExit();
        }
    }

    public void OnUpdate()
    {
        for (var i = 0; i < _actions.Length; i++)
        {
            _actions[i].OnUpdate();
        }


    }
}

