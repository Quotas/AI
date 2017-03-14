using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProcessState { FAILURE, SUCCESS, PENDING }


//Our interface for unity
public class Entity : MonoBehaviour
{


    private int health;
    private Need hunger;
    private Need stamina;
    private Need thirst;
    private Need hygine;

    private AI ai;


    void Start()
    {

        ai = new AI();


        hunger = new Need("Hunger", eat, 0f, 100f);
        stamina = new Need("Stamina", rest, 0f, 100f, 2f);
        thirst = new Need("Thirst", drink, 0f, 100f, 0.2f);
        hygine = new Need("Hygine", shower, 0f, 100f, 0.5f);

        ai.registerNeed(hunger);
        ai.registerNeed(stamina);
        ai.registerNeed(thirst);
        ai.registerNeed(hygine);



    }

    // Update is called once per frame
    void Update()
    {

        ai.Update();

    }


    //all delgate need functions must return a ProcessState enum
    ProcessState eat()
    {

        Debug.Log("Lets eat.");

        return ProcessState.SUCCESS;
    }

    ProcessState rest()
    {

        Debug.Log("I need to sleep.");

        return ProcessState.SUCCESS;
    }

    ProcessState drink()
    {

        Debug.Log("I need to drink something.");

        return ProcessState.SUCCESS;


    }

    ProcessState shower()
    {

        Debug.Log("I need to take a shower.");

        return ProcessState.SUCCESS;
    }


}
