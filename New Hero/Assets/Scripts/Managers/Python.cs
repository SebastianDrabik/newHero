using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Python : MonoBehaviour
{

    public Transform upWall;
    public Transform downWall;
    public Transform leftWall;
    public Transform rightWall;

    public GameObject rockPrefab;

    int rockCount = 100;

    public Sprite blue;
    public Sprite yellow;
    public Sprite normal;

    public GameObject yellowPython;
    public GameObject bluePython;

    public short health = 4;

    float timer = 4f;
    [HideInInspector]
    public bool fightStarted = false;
    bool IsAttacking = false;

    enum attackMode
    {
        rock = 0,
        contact = 1,
    }
    
    attackMode currentMode;

    enum attacking
    {
        yellow = 0,
        blue=1,
    }

    void Start()
    {
        currentMode = attackMode.rock;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fightStarted || IsAttacking) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if(currentMode == attackMode.rock)
            {
                RockAttack();
                currentMode = attackMode.contact;
            }else if(currentMode == attackMode.contact)
            {
                Attack();
                currentMode = attackMode.rock;
            }
            timer = 4f;

        }

        if (health == 0f)
        {
            //DIE
            Debug.Log("He ded");
            gameObject.SetActive(false);
        }
    }

    public void RockAttack()
    {
        Vector2 speed = new();
        Transform spawnPoint = gameObject.transform;
        switch (Random.Range(0, 4))
        {
            case 0:
                spawnPoint = downWall;
                speed = Vector2.up;
                break;
            case 1:
                spawnPoint = upWall;
                speed = Vector2.down;
                break;
            case 2:
                spawnPoint = rightWall;
                speed = Vector2.left;
                break;
            case 3:
                spawnPoint = leftWall;
                speed = Vector2.right;
                break;
        }
        for (int i = 0;i<rockCount;i++)
        {
            IEnumerator spawn = CreateRock(speed, spawnPoint);
            StartCoroutine(spawn);
        }
    }

    IEnumerator CreateRock(Vector2 speed,Transform spawnPoint)
    {
        Vector3 pos = spawnPoint.position;
        if (spawnPoint == upWall || spawnPoint == downWall)
        {
            pos.x+=Random.Range(-spawnPoint.localScale.x / 2.0f, spawnPoint.localScale.x / 2.0f);
        }
        else if (spawnPoint == leftWall || spawnPoint == rightWall)
        {
            pos.y += Random.Range(-spawnPoint.localScale.y / 2.0f, spawnPoint.localScale.y / 2.0f);
        }
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        GameObject rock = Instantiate(rockPrefab,pos,Quaternion.identity);
        rock.GetComponent<RockFly>().speed = speed;
    }


    public void Attack()
    {
        IsAttacking = true;
        int attackingNum = Random.Range(0, 2);

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (attackingNum == (int)attacking.yellow)
        {
            yellowPython.SetActive(true);
            yellowPython.transform.SetPositionAndRotation(playerPos, Quaternion.identity);
            yellowPython.GetComponent<MiniPython>().AttackPlayer(health);
            gameObject.GetComponent<SpriteRenderer>().sprite = blue;
        }else if (attackingNum == (int)attacking.blue)
        {
            bluePython.SetActive(true);
            bluePython.transform.SetPositionAndRotation(playerPos, Quaternion.identity);
            bluePython.GetComponent<MiniPython>().AttackPlayer(health);
            gameObject.GetComponent<SpriteRenderer>().sprite = yellow;
        }
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            health--;
        }

        yellowPython.SetActive(false);
        bluePython.SetActive(false);

        gameObject.GetComponent<SpriteRenderer>().sprite = normal;
        IsAttacking = false;
    }
}
