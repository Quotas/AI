using UnityEngine;

public class Utility : MonoBehaviour
{

    public bool entityNear = false;


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Entity")
        {

            entityNear = true;
            collision.gameObject.GetComponent<Entity>().destinationReached = true;


        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Entity")
        {

            entityNear = false;
            collision.gameObject.GetComponent<Entity>().destinationReached = false;


        }




    }
}
