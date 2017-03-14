using System.Collections.Generic;
using DotFuzzy;


public class AI
{
    // Use this for initialization



    FuzzyEngine fuzzyEngine;

    Dictionary<string, Need> AINeedsList;



    public AI()
    {
        fuzzyEngine = new FuzzyEngine();

        AINeedsList =  new Dictionary<string, Need>();

    }

    public void registerNeed(Need need)
    {
        AINeedsList.Add(need.Name, need);

    }

    public Need getNeed(string name) {


        Need tmp = null;

        if (AINeedsList.TryGetValue(name, out tmp)) {

            return tmp;
        }

        return null; 


    }

    public void Update()
    {

        foreach (KeyValuePair<string, Need> need in AINeedsList)
        {

            need.Value.Update(1f);


        }

        if (ActionPriorityList.ActionQueued) {

            ActionPriorityList.Fire();


        }

    }


}
