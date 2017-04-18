using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using DotFuzzy;

public enum ProcessState { FAILURE, SUCCESS, PENDING }


//Our interface for unity
public class Entity : MonoBehaviour
{

    public bool fuzzy;
    public bool destinationReached = false;


    public Need hunger;
    public Need stamina;
    public Need thirst;
    public Need hygine;

    private Utility bath;
    private Utility refrigerator;
    private Utility bed;
    private Utility task;

    LinguisticVariable fhunger;
    LinguisticVariable fstamina;
    LinguisticVariable fthirst;
    LinguisticVariable fhygine;

    [SerializeField]
    public List<Node> nodes;
    public int score;


    public AI ai;


    void Start()
    {



        score = 0;

        ai = new AI();
        nodes = FindObjectsOfType<Node>().OrderBy(node => node.order).ToList<Node>();

        fhunger = new LinguisticVariable("Hunger");
        fstamina = new LinguisticVariable("Stamina");
        fthirst = new LinguisticVariable("Thirst");
        fhygine = new LinguisticVariable("Hygine");

        bath = GameObject.Find("Bath").GetComponent<Utility>();
        refrigerator = GameObject.Find("Refrigerator").GetComponent<Utility>();
        bed = GameObject.Find("Bed").GetComponent<Utility>();
        task = GameObject.Find("Task").GetComponent<Utility>();


        hunger = new Need("Hunger", eat, refrigerator, 0f, 100f, 0.1f);
        stamina = new Need("Stamina", rest, bed, 0f, 100f, .2f);
        thirst = new Need("Thirst", drink, refrigerator, 0f, 100f, 0.02f);
        hygine = new Need("Hygine", shower, bath, 0f, 100f, 0.05f);

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


        //register the Needs with our AI for need sorting and for need searching
        ai.registerNeed(hunger);
        ai.registerNeed(stamina);
        ai.registerNeed(thirst);
        ai.registerNeed(hygine);


        //register the default task for the entity with the ActionPriorityList
        ActionPriorityList.registerDefaultAction(Task);



    }

    // Update is called once per frame
    void FixedUpdate()
    {


        fhunger.InputValue = hunger.Value;
        fthirst.InputValue = thirst.Value;
        fstamina.InputValue = stamina.Value;
        fhygine.InputValue = hygine.Value;

        ai.Update(fuzzy);

        if (Input.GetKeyDown(KeyCode.R))
        {

            ai.Sort();

        }

        if (nodes.Count == 0 && destinationReached)
        {

            nodes = FindObjectsOfType<Node>().OrderBy(node => node.order).ToList<Node>();
        }

    }

    public Node FindNearestNode()
    {

        Node closestNode = null;
        float closestDistance = Vector3.Distance(transform.position, nodes[0].transform.position);

        foreach (Node node in nodes)
        {
            var distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance <= closestDistance)
            {

                closestNode = node;

            }

        }

        return closestNode;




    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.tag == "Node")
        {
            if (nodes.Count != 0)
            {
                nodes.Remove(collision.gameObject.GetComponent<Node>());

            }



        }


    }


    //all delgate need functions must return a ProcessState enum
    ProcessState eat()
    {

        if (refrigerator.entityNear != true)
        {
            var nextPoint = Vector3.one;
            if (nodes.Count != 0)
                if (Vector3.Distance(transform.position, FindNearestNode().transform.position) >
                    Vector3.Distance(transform.position, task.transform.position))
                {

                    nextPoint = refrigerator.transform.position;

                }
                else
                {

                    nextPoint = FindNearestNode().transform.position;

                }
            else
            {
                nextPoint = refrigerator.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime);

        }
        else
        {
            hunger.Value += 25;
            if (hunger.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {

                UnityEngine.Debug.Log("Processing: Hunger");
                return ProcessState.PENDING;

            }

        }
        UnityEngine.Debug.Log("Pending: Hunger");
        return ProcessState.PENDING;

    }

    ProcessState rest()
    {

        if (!bed.entityNear)
        {
            var nextPoint = Vector3.one;
            if (nodes.Count != 0)
                if (Vector3.Distance(transform.position, FindNearestNode().transform.position) >
                    Vector3.Distance(transform.position, task.transform.position))
                {

                    nextPoint = bed.transform.position;

                }
                else
                {

                    nextPoint = FindNearestNode().transform.position;

                }
            else
            {
                nextPoint = bed.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime);

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
                UnityEngine.Debug.Log("Processing: Bed");
                return ProcessState.PENDING;

            }

        }
        UnityEngine.Debug.Log("Pending: Bed");
        return ProcessState.PENDING;

    }

    ProcessState drink()
    {


        if (!refrigerator.entityNear)
        {
            var nextPoint = Vector3.one;
            if (nodes.Count != 0)
                if (Vector3.Distance(transform.position, FindNearestNode().transform.position) >
                    Vector3.Distance(transform.position, task.transform.position))
                {

                    nextPoint = refrigerator.transform.position;

                }
                else
                {

                    nextPoint = FindNearestNode().transform.position;

                }
            else
            {
                nextPoint = refrigerator.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime);

        }
        else
        {
            thirst.Value += 25;
            if (thirst.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {
                UnityEngine.Debug.Log("Processing: Thirst");
                return ProcessState.PENDING;

            }

        }
        UnityEngine.Debug.Log("Pending: Thirst");
        return ProcessState.PENDING;


    }

    ProcessState shower()
    {


        if (!bath.entityNear)
        {
            var nextPoint = Vector3.one;
            if (nodes.Count != 0)
                if (Vector3.Distance(transform.position, FindNearestNode().transform.position) >
                    Vector3.Distance(transform.position, task.transform.position))
                {

                    nextPoint = bath.transform.position;

                }
                else
                {

                    nextPoint = FindNearestNode().transform.position;

                }
            else
            {
                nextPoint = bath.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime);

        }
        else
        {

            hygine.Value += 25;
            if (hygine.Value >= 100)
            {
                return ProcessState.SUCCESS;

            }
            else
            {
                UnityEngine.Debug.Log("Processing: Bath");
                return ProcessState.PENDING;

            }

        }
        UnityEngine.Debug.Log("Pending: Bath");
        return ProcessState.PENDING;
    }

    ProcessState Task()
    {

        if (!task.entityNear)
        {
            var nextPoint = Vector3.one;
            if (nodes.Count != 0)
            {

                if (Vector3.Distance(transform.position, FindNearestNode().transform.position) >
                    Vector3.Distance(transform.position, task.transform.position))
                {

                    nextPoint = task.transform.position;

                }
                else
                {

                    nextPoint = FindNearestNode().transform.position;

                }
            }
            else
            {
                nextPoint = task.transform.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime);
            UnityEngine.Debug.Log("Pending: Default Task");
        }
        else
        {

            UnityEngine.Debug.Log("Processing: Default Task");
            score += 1;
            return ProcessState.PENDING;

        }
        return ProcessState.SUCCESS;



    }


    public string GetCurrentMethod()
    {
        StackTrace st = new StackTrace();
        StackFrame sf = st.GetFrame(1);

        return sf.GetMethod().Name;
    }


}
