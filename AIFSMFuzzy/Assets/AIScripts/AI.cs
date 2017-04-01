using System.Collections.Generic;
using System.Linq;
using DotFuzzy;
using UnityEngine;


public class AI
{

    public FuzzyEngine fuzzyEngine;

    Dictionary<string, Need> AINeedsList;

    //Constructor
    public AI()
    {
        fuzzyEngine = new FuzzyEngine();

        AINeedsList = new Dictionary<string, Need>();

    }

    //Register a Need in our NeedsList
    public void registerNeed(Need need)
    {
        AINeedsList.Add(need.Name, need);

    }

    //Get a Need object by name from the dictionary
    public Need getNeed(string name)
    {


        Need tmp = null;

        if (AINeedsList.TryGetValue(name, out tmp))
        {

            return tmp;
        }

        return null;


    }

    //This function will give use an IEnumberable rather than sorting our dictionary
    public void Sort()
    {

        //Print the needs to the debug console in order of priority in ascending
        var sortedDict = from entry in AINeedsList orderby entry.Value.Priority ascending select entry;
        foreach (KeyValuePair<string, Need> need in sortedDict)
        {

            Debug.Log(need.Value.Name + " :" + need.Value.Priority);

        }

        var fuzzysortedDict = from entry in AINeedsList orderby entry.Value.FuzzyPriorty ascending select entry;
        foreach (KeyValuePair<string, Need> need in sortedDict)
        {

            Debug.Log(need.Value.Name + " :" + need.Value.FuzzyPriorty);

        }
    }

    //Basic update function will be fixed called rate at 60 per second but will decay the needs and fire registered 
    //Need methods in the ActionPriorityList
    public void Update()
    {

        foreach (KeyValuePair<string, Need> need in AINeedsList)
        {

            need.Value.Update(1f);


        }

        ActionPriorityList.Fire();




    }


}
