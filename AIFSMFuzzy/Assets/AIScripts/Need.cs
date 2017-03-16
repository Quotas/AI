using UnityEngine;
using System;
using Stateless;
using DotFuzzy;




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
    public float FuzzyPriorty { get; set; }
    public float Value { get; set; }

    private float _minValue;
    private float _maxValue;
    private float _scale;

    public FuzzyEngine fuzzyengine;

    private Utility utility;

    public delegate ProcessState InternalNeedMethod();

    public InternalNeedMethod method;


    public Need(string name, Func<ProcessState> task, Utility u, float min, float max, float scale = 1)
    {

        state = new StateMachine<State, Trigger>(State.Fufilled);

        state.Configure(State.Fufilled)
            .Permit(Trigger.OnMinValueReached, State.Unfufilled);

        state.Configure(State.Unfufilled)
            .OnEntry(t => ActionPriorityList.Add(method))
            .Permit(Trigger.OnMaxValueReached, State.Fufilled);

        method = new InternalNeedMethod(task);
        _minValue = min;
        _maxValue = max;
        _scale = scale;

        utility = u;

        Value = max;
        Name = name;
        Priority = 0;

        fuzzyengine = new FuzzyEngine();


        LinguisticVariable priority = new LinguisticVariable("Priority");
        priority.MembershipFunctionCollection.Add(new MembershipFunction("High", 0, 25, 25, 50));
        priority.MembershipFunctionCollection.Add(new MembershipFunction("Low", 50, 75, 75, 100));


        fuzzyengine.LinguisticVariableCollection.Add(priority);
        fuzzyengine.Consequent = "Priority";

    }

    public void newRule(FuzzyRule rule)
    {

        fuzzyengine.FuzzyRuleCollection.Add(rule);



    }


    public void Update(float interval)
    {


        FuzzyPriorty = (float)fuzzyengine.Defuzzify();




        if (state.IsInState(State.Fufilled))
        {

            if (Value > _minValue)
            {

                Value -= interval * _scale;

            }
            else if (Value <= _minValue)
            {
                Value = _minValue;
                state.Fire(Trigger.OnMinValueReached);
            }

            Priority = (Mathf.FloorToInt(Value * .1f) * -1) + 10;

        }
        else if (state.IsInState(State.Unfufilled))
        {

            if (Value >= _maxValue)
            {
                Value = _maxValue;
                state.Fire(Trigger.OnMaxValueReached);

            }

            Priority = (Mathf.FloorToInt(Value * .1f) * -1) + 10;

        }



    }


}

