using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour
{


    public enum State { }

    // Use this for initialization
    private int health;
    private Need hunger;
    private Need stamina;
    private Need thirst;
    private Need hygine;

    private AI ai;


    private State curState;





    void Start()
    {

        ai = new AI();


        hunger = new Need("Hunger", eat, 0f, 100f);




        ai.registerNeed(hunger);


        Debug.Log(ai.getNeed("Hunger").Name);


    }

    // Update is called once per frame
    void Update()
    {




        ai.Update();






    }



    void eat()
    {


        Debug.Log("Lets eat.");

    }
}
