using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFly : MonoBehaviour
{
    public Vector2 speed = Vector2.zero;
    public float speedMult;
    public Sprite[] rocks;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = rocks[Random.Range(0, rocks.Length-1)];
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = speed*speedMult;    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Rock")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerInteraction>().DamagePlayer(3);
            }
            //play destruction animation
            Destroy(gameObject);
        }
        
    }
}
