using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkCube : MonoBehaviour
{
    Vector2 velocity = new(0f, 0f);
    public GameObject boxAttackPrefab;
    public GameObject player;
    public GameObject bulletPrefab;


    private float timerMult = 1f;
    private float timer = 2f;
    private int stage = 0;
    int counter = 0;

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

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0f)
        {
            if (stage == 0)
            {
                BulletAttack();
            }
            if (stage == 1)
            {
                BulletAttack();
                BoxAttack();
                timerMult = Mathf.Clamp(timerMult - 0.07f, 0.5f, 1f);
            }
            timer = 2f;
            timer *= timerMult;
            counter++;
            if(counter==10 && stage == 0)
            {
                stage = 1;
            }
        }
    }

    public void BoxAttack()
    {
        Instantiate(boxAttackPrefab, player.transform.position, Quaternion.identity);
    }

    void BulletAttack()
    {
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(bullet, 6f);
    }
}
