using System.Collections.Generic;
using System.Linq;
using System;




public static class ActionPriorityList
{

    public enum State { PENDING, SUCCESSS, FAIL };
    public static List<Need.InternalNeedMethod> ActionList = new List<Need.InternalNeedMethod>();
    public static bool ActionQueued = false;




    public static void Add(Need.InternalNeedMethod method)
    {

        ActionList.Add(method);

        ActionQueued = true;

    }


    public static void Fire()
    {

        if (ActionList.Count != 0)
        {

            if (ActionList.First()() == ProcessState.SUCCESS)
            {
                ActionList.Remove(ActionList.First());


            }
        }


        if (ActionList.Count == 0)
        {

            ActionQueued = false;

        }

    }
}
