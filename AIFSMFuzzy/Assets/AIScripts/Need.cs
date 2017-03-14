using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Stateless;


public class Need
{


    enum State
    {
        Unfufilled,
        Fufilled,

    }

    enum Trigger
    {
        OnMinValueReached,
        OnMaxValueReached,


    }

    StateMachine<State, Trigger> state;

    public string Name { get; set; }
    public int Priority { get; set; }
    public float Value { get; set; }

    private float _minValue;
    private float _maxValue;
    private float _scale;

    private Action _methodTask;

    //StateMachine<State, Trigger>.TriggerWithParameters<int> setThresholdTrigger;

    public Need(string name, Action task, float min, float max, float scale = 1)
    {

        state = new StateMachine<State, Trigger>(State.Fufilled);

        state.Configure(State.Fufilled)
            .Permit(Trigger.OnMinValueReached, State.Unfufilled);

        state.Configure(State.Unfufilled)
            .OnEntry(t => ActionPriorityList.Add(ref _methodTask))
            .Permit(Trigger.OnMaxValueReached, State.Fufilled);

        _methodTask = task;
        _minValue = min;
        _maxValue = max;
        _scale = scale;

        Value = max;
        Name = name;
        Priority = 0;

    }

    public void Update(float interval)
    {
        if (state.IsInState(State.Fufilled))
        {

            if (Value > _minValue)
            {

                Value -= interval * _scale;

            }
            else if (Value == _minValue)
            {

                state.Fire(Trigger.OnMinValueReached);
            }


        }
        else if (state.IsInState(State.Unfufilled))
        {

            if (Value == _maxValue)
            {

                state.Fire(Trigger.OnMaxValueReached);

            }

        }



    }


}

