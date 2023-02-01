using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxProjectile : MonoBehaviour
{
    public GameObject box;
    public GameObject target;
    public GameObject explotion;

    bool boxHit = false;
    void Start()
    {
        box.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -9f);
    }

    void Update()
    {
        if (!boxHit)
        {
            if (box.transform.position.y <= target.transform.position.y)
            {
                boxHit = true;
                ExplodeBox();
            }
        }
        

    }

    void ExplodeBox()
    {
        Destroy(box);
        Destroy(target);

        explotion.GetComponent<Animator>().enabled = true;
        if ((GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position).sqrMagnitude < 1.5f*1.5f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>().DamagePlayer(4);
        }
        Destroy(gameObject, 2);
    }


}
