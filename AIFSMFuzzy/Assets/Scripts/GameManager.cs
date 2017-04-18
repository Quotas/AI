using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    Entity entity;
    Text hunger;
    Text stamina;
    Text thirst;
    Text hygiene;
    Text score;
    Text mode;

    void Start()
    {
        entity = FindObjectOfType<Entity>();
        hunger = FindObjectOfType<Canvas>().transform.FindChild("Hunger").GetComponent<Text>();
        stamina = FindObjectOfType<Canvas>().transform.FindChild("Stamina").GetComponent<Text>();
        thirst = FindObjectOfType<Canvas>().transform.FindChild("Thirst").GetComponent<Text>();
        hygiene = FindObjectOfType<Canvas>().transform.FindChild("Hygiene").GetComponent<Text>();
        score = FindObjectOfType<Canvas>().transform.FindChild("Score").GetComponent<Text>();
        mode = FindObjectOfType<Canvas>().transform.FindChild("Mode").GetComponent<Text>();

        DontDestroyOnLoad(this);


    }

    // Update is called once per frame
    void Update()
    {

        if (entity == null || hunger == null || stamina == null || thirst == null || hygiene == null)
        {
            entity = FindObjectOfType<Entity>();
            hunger = FindObjectOfType<Canvas>().transform.FindChild("Hunger").GetComponent<Text>();
            stamina = FindObjectOfType<Canvas>().transform.FindChild("Stamina").GetComponent<Text>();
            thirst = FindObjectOfType<Canvas>().transform.FindChild("Thirst").GetComponent<Text>();
            hygiene = FindObjectOfType<Canvas>().transform.FindChild("Hygiene").GetComponent<Text>();
            score = FindObjectOfType<Canvas>().transform.FindChild("Score").GetComponent<Text>();
            mode = FindObjectOfType<Canvas>().transform.FindChild("Mode").GetComponent<Text>();
        }


        hunger.text = "Hunger: " + entity.hunger.Value;
        thirst.text = "Thirst: " + entity.thirst.Value;
        stamina.text = "Stamina: " + entity.stamina.Value;
        hygiene.text = "Hygiene: " + entity.hygine.Value;
        score.text = "Score: " + entity.score;

        if (entity.fuzzy)
        {

            mode.text = "Mode: Fuzzy";

        }
        else
        {
            mode.text = "Mode: Finite State Machine";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActionPriorityList.Clear();
            SceneManager.LoadScene("Main");



        }

        if (Input.GetKeyDown(KeyCode.F))
        {

            entity.fuzzy = !entity.fuzzy;

        }

    }
}
