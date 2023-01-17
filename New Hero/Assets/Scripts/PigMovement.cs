using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
    private int pigClicks = 0;
    public int minPigClicks;
    Vector2 speed = new Vector2();
    private void Start()
    {
        speed = new Vector2(Random.Range(-0.8f, .8f), Random.Range(-1f, 1f));
        gameObject.GetComponent<SpriteRenderer>().flipX = speed.x > 0 ? true : false;

    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = speed;
    }

    private void OnMouseDown()
    {
        pigClicks++;
        if(pigClicks == minPigClicks)
            GameManager.Instance.ChangeTrophyState("tuman", Trophy.TrophyState.UNLOCKED, true);
        if (pigClicks < minPigClicks)
            GameManager.Instance.ChangeTrophyState("tuman", Trophy.TrophyState.IN_PROGRESS);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Fence")
        {
            speed.y *= -1f;
        }
        else if(collision.gameObject.tag == "House")
        {
            speed.x *= -1f;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = speed.x > 0 ? true : false;

    }
}
