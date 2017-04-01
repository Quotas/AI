using UnityEngine;

public class Utility : MonoBehaviour
{

    public bool entityNear = false;


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Entity")
        {

            entityNear = true;


        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Entity")
        {

            entityNear = false;


        }




    }
}
