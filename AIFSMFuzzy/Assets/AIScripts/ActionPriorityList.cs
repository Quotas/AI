using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;




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
            

                var state = ActionList.First()();

                switch (state)
                {
                    case ProcessState.SUCCESS:
                        ActionList.Remove(ActionList.First());
                        break;
                    case ProcessState.PENDING:
                        //
                        break;
                    case ProcessState.FAILURE:
                        Debug.Log("Failure on event stack at " + ActionList.First());
                        break;

                }
                
            }
        
        else {

            ActionQueued = false;

        }


        

    }

    
}
