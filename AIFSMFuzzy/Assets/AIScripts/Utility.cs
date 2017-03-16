using UnityEngine;

public class Utility : MonoBehaviour
{

    public bool entityNear = false;


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Entity")
        {
            Debug.Log("Collision!");
            entityNear = true;


        }


    }
}
