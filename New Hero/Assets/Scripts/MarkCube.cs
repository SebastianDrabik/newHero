using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkCube : MonoBehaviour
{
    Vector2 velocity = new(0f, 0f);
    public GameObject boxAttack;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            velocity.y *= -1f;
        }
        if (collision.gameObject.tag == "Wall_Side")
        {
            velocity.x *= -1f;
        }
    }

    public void BoxAttack()
    {
        Instantiate(boxAttack, player.transform.position, Quaternion.identity);
    }
}
