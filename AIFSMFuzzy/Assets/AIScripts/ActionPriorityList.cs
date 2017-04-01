using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;




public static class ActionPriorityList
{

    public enum State { PENDING, SUCCESSS, FAIL };
    public static List<Need.InternalNeedMethod> ActionList = new List<Need.InternalNeedMethod>();
    public static bool ActionQueued = false;


    //store a reference to our default action so that entities can change their default action or something
    //This is necessary since all the logic for task execution is stored in entity
    private static Func<ProcessState> defaultAction;




    public static void Add(Need.InternalNeedMethod method)
    {

        ActionList.Add(method);

        ActionQueued = true;

    }

    public static void registerDefaultAction(Func<ProcessState> method)
    {

        defaultAction = method;

    }



    public static void Fire()
    {

        if (ActionList.Count != 0)
        {


            var state = ActionList.First()();

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
