using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Vector2 speed;
        speed.x = Random.value >= 0.5f ? Random.Range(2f, 4f) : Random.Range(-2f, -4f);
        speed.y = Random.value >= 0.5f ? Random.Range(2f, 4f) : Random.Range(-2f, -4f);

        gameObject.GetComponent<Rigidbody2D>().velocity = speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 initialVelocity= gameObject.GetComponent<Rigidbody2D>().velocity;
        if (collision.gameObject.tag == "Wall")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(initialVelocity.x,initialVelocity.y*-1);
            gameObject.GetComponent<SpriteRenderer>().flipY = !gameObject.GetComponent<SpriteRenderer>().flipY;
        }
        if (collision.gameObject.tag == "Wall_Side")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(initialVelocity.x * -1, initialVelocity.y);
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;

        }
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>().DamagePlayer(2);
            Destroy(gameObject);
        }
    }

}
