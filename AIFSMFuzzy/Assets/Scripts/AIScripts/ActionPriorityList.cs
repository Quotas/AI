using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;




public static class ActionPriorityList
{

    public enum State { PENDING, SUCCESSS, FAIL };
    public static List<Func<ProcessState>> ActionList = new List<Func<ProcessState>>();
    public static bool ActionQueued = false;


    //store a reference to our default action so that entities can change their default action or something
    //This is necessary since all the logic for task execution is stored in entity
    private static Func<ProcessState> defaultAction;


    public static void Clear()
    {

        ActionList.Clear();

    }

    public static void Add(Func<ProcessState> method)
    {

        ActionList.Add(method);

        ActionQueued = true;

    }

    public static void registerDefaultAction(Func<ProcessState> method)
    {

        defaultAction = method;

    }



    public static void Fire(Need need = null)
    {
        ProcessState state;

        if (ActionList.Count != 0)
        {

            if (need != null)
            {

                state = need.InternalNeedMethod();

            }
            else
            {

                state = ActionList.First()();
            }

            switch (state)
            {
                case ProcessState.SUCCESS:
                    ActionList.Remove(ActionList.First());
                    break;
                case ProcessState.PENDING:

                    break;
                case ProcessState.FAILURE:
                    UnityEngine.Debug.Log("Failure on event stack at " + ActionList.First());
                    break;

            }

        }

        //if our need action list is empty do our default action

        else
        {

            defaultAction();
        }




    }




}
