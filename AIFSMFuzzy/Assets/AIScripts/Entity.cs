using UnityEngine;
using DotFuzzy;


public enum ProcessState { FAILURE, SUCCESS, PENDING }


//Our interface for unity
public class Entity : MonoBehaviour
{


    private Need hunger;
    private Need stamina;
    private Need thirst;
    private Need hygine;

    private Utility bath;
    private Utility refrigerator;
    private Utility bed;
    private Utility task;

    LinguisticVariable fhunger = new LinguisticVariable("Hunger");
    LinguisticVariable fstamina = new LinguisticVariable("Stamina");
    LinguisticVariable fthirst = new LinguisticVariable("Thirst");
    LinguisticVariable fhygine = new LinguisticVariable("Hygine");


    private AI ai;


    void Start()
    {

        ai = new AI();

        bath = GameObject.Find("Bath").GetComponent<Utility>();
        refrigerator = GameObject.Find("Refrigerator").GetComponent<Utility>();
        bed = GameObject.Find("Bed").GetComponent<Utility>();
        task = GameObject.Find("Task").GetComponent<Utility>();


        hunger = new Need("Hunger", eat, refrigerator, 0f, 100f);
        stamina = new Need("Stamina", rest, bed, 0f, 100f, 2f);
        thirst = new Need("Thirst", drink, refrigerator, 0f, 100f, 0.2f);
        hygine = new Need("Hygine", shower, bath, 0f, 100f, 0.5f);

        //FuzzyLogicEngine setup goes here
        fhunger.MembershipFunctionCollection.Add(new MembershipFunction("Hungry", 0, 0, 20, 40));
        fhunger.MembershipFunctionCollection.Add(new MembershipFunction("Neutral", 30, 50, 50, 70));
        fhunger.MembershipFunctionCollection.Add(new MembershipFunction("Full", 50, 80, 100, 100));

        fstamina.MembershipFunctionCollection.Add(new MembershipFunction("Tired", 0, 0, 20, 40));
        fstamina.MembershipFunctionCollection.Add(new MembershipFunction("Neutral", 30, 50, 50, 70));
        fstamina.MembershipFunctionCollection.Add(new MembershipFunction("Rested", 50, 80, 100, 100));

        fthirst.MembershipFunctionCollection.Add(new MembershipFunction("Thirsty", 0, 0, 20, 40));
        fthirst.MembershipFunctionCollection.Add(new MembershipFunction("Neutral", 30, 50, 50, 70));
        fthirst.MembershipFunctionCollection.Add(new MembershipFunction("Full", 50, 80, 100, 100));

        fhygine.MembershipFunctionCollection.Add(new MembershipFunction("Dirty", 0, 0, 20, 40));
        fhygine.MembershipFunctionCollection.Add(new MembershipFunction("Neutral", 30, 50, 50, 70));
        fhygine.MembershipFunctionCollection.Add(new MembershipFunction("Clean", 50, 80, 100, 100));


        hunger.fuzzyengine.LinguisticVariableCollection.Add(fhunger);
        stamina.fuzzyengine.LinguisticVariableCollection.Add(fstamina);
        thirst.fuzzyengine.LinguisticVariableCollection.Add(fthirst);
        hygine.fuzzyengine.LinguisticVariableCollection.Add(fhygine);

        hunger.newRule(new FuzzyRule("IF (Hunger IS Hungry) OR (Hunger IS Neutral) THEN Priority IS High"));
        hunger.newRule(new FuzzyRule("IF (Hunger IS Full) THEN Priorty IS Low"));

        stamina.newRule(new FuzzyRule("IF (Stamina IS Tired) OR (Stamina IS Neutral) THEN Priority IS High"));
        stamina.newRule(new FuzzyRule("IF (Stamina IS Rested) THEN Priorty IS Low"));

        thirst.newRule(new FuzzyRule("IF (Thirst IS Thirsty) OR (Thirst IS Neutral) THEN Priority IS High"));
        thirst.newRule(new FuzzyRule("IF (Thirst IS Full) THEN Priorty IS Low"));

        hygine.newRule(new FuzzyRule("IF (Hygine IS Dirty) OR (Hygine IS Neutral) THEN Priority IS High"));
        hygine.newRule(new FuzzyRule("IF (Hygine IS Clean) THEN Priorty IS Low"));



        ai.registerNeed(hunger);
        ai.registerNeed(stamina);
        ai.registerNeed(thirst);
        ai.registerNeed(hygine);

        ActionPriorityList.registerDefaultAction(Task);



    }

    // Update is called once per frame
    void FixedUpdate()
    {


        fhunger.InputValue = hunger.Value;
        fthirst.InputValue = thirst.Value;
        fstamina.InputValue = stamina.Value;
        fhygine.InputValue = hygine.Value;

        ai.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {

            ai.Sort();

        }

    }

    //all delgate need functions must return a ProcessState enum
    ProcessState eat()
    {

        if (refrigerator.entityNear != true)
        {

            transform.position = Vector3.MoveTowards(transform.position, refrigerator.transform.position, Time.deltaTime);

        }
        else
        {
            hunger.Value += 5;
            if (hunger.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {

                return ProcessState.PENDING;

            }

        }

        return ProcessState.PENDING;

    }

    ProcessState rest()
    {

        if (!bed.entityNear)
        {

            transform.position = Vector3.MoveTowards(transform.position, bed.transform.position, Time.deltaTime);

        }
        else
        {
            stamina.Value += 5;
            if (stamina.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {

                return ProcessState.PENDING;

            }

        }

        return ProcessState.PENDING;

    }

    ProcessState drink()
    {


        if (!refrigerator.entityNear)
        {

            transform.position = Vector3.MoveTowards(transform.position, refrigerator.transform.position, Time.deltaTime);

        }
        else
        {
            thirst.Value += 5;
            if (thirst.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {

                return ProcessState.PENDING;

            }

        }

        return ProcessState.PENDING;


    }

    ProcessState shower()
    {


        if (!bath.entityNear)
        {

            transform.position = Vector3.MoveTowards(transform.position, bath.transform.position, Time.deltaTime);

        }
        else
        {

            hygine.Value += 5;
            if (hygine.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {

                return ProcessState.PENDING;

            }

        }

        return ProcessState.PENDING;
    }

    ProcessState Task()
    {

        if (!task.entityNear)
        {

            transform.position = Vector3.MoveTowards(transform.position, task.transform.position, Time.deltaTime);

        }
        else
        {

            return ProcessState.PENDING;

        }

        return ProcessState.PENDING;



    }


}
