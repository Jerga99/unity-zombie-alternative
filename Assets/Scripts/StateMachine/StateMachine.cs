using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private StateSO[] _statesSO;

    private State[] _states;

    private int _currentPhase = 0;
    private Dictionary<Type, Component> _cachedComponents = new();

    private void Awake()
    {
        InitialState();
    }

    void InitialState()
    {
        _states = new State[_statesSO.Length];
        var createdInstances = new Dictionary<ScriptableObject, object>();

        for (var i = 0; i < _statesSO.Length; i++)
        {
            var state = _statesSO[i].GetState(this, createdInstances);
            _states[i] = state;
        }
    }

    // Use this for initialization
    void Start()
    {
        _states[_currentPhase].OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        State currentState = _states[_currentPhase];

        currentState.OnUpdate();

        if (currentState.CanTransition() && _currentPhase + 1 != _states.Length)
        {
            Transition();
        }
    }

    public new bool TryGetComponent<T>(out T component) where T : Component
    {
        var type = typeof(T);

        if (!_cachedComponents.TryGetValue(type, out var value))
        {
            if (base.TryGetComponent(out component))
            {
                _cachedComponents.Add(type, component);
            }

            return component != null;
        }

        component = (T)value;
        return true;
    }

    public new T GetComponent<T>() where T : Component
    {
        return TryGetComponent(out T component) ?
            component :
            throw new InvalidOperationException($"{typeof(T).Name} not found in {name}.");
    }

    void Transition()
    {
        _states[_currentPhase].OnStateExit();
        _currentPhase++;
        _states[_currentPhase].OnStateEnter();
    }
}

