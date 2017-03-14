using System.Collections.Generic;
using System.Linq;
using System;




public static class ActionPriorityList
{

    public static List<Action> ActionList = new List<Action>();
    public static bool ActionQueued = false;




    public static void Add(ref Action method)
    {

        ActionList.Add(method);

        ActionQueued = true;

    }


    public static void Fire()
    {

        if (ActionList.Count != 0)
        {
            ActionList.First().Invoke();
        }

        ActionList.Remove(ActionList.First());


        if (ActionList.Count == 0)
        {

            ActionQueued = false;

        }

    }
}
