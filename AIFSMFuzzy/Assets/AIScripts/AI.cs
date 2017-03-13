using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DotFuzzy;
using Stateless;

public class AI
{
    // Use this for initialization



    FuzzyEngine fuzzyEngine;

    List<Need> AINeedsList;

    public AI(List<Need> needs)
    {


        fuzzyEngine = new FuzzyEngine();

        AINeedsList = needs;


    }


    public List<Need> getOrderedNeeds()
    {

        return AINeedsList;

    }

}
