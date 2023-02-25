using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Python : MonoBehaviour
{

    public Transform upWall;
    public Transform downWall;
    public Transform leftWall;
    public Transform rightWall;

    public FightManager editor;
    public GameObject rockPrefab;
    public GameManager manager;
    public GameObject blockadeObject;
    public MessageManager messageManager;

    public DoorController exit;

    int rockCount = 100;

    public Sprite blue;
    public Sprite yellow;
    public Sprite normal;

    public GameObject yellowPython;
    public GameObject bluePython;

    public short health = 4;

    float timer = 10f;
    [HideInInspector]
    public bool fightStarted = false;
    bool IsAttacking = false;
    private bool blockade = false;

    public Slider healthbar;

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
        exit.locked = true;
    }

    void Update()
    {
        if (!fightStarted || IsAttacking) return;
        if (manager.GetTrophyState("goofylanguage") != Trophy.TrophyState.IN_PROGRESS)
            manager.ChangeTrophyState("goofylanguage", Trophy.TrophyState.IN_PROGRESS);
        if (!blockade)
            blockadeObject.SetActive(true);
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
            timer = 10f;

        }

        if (health == 0f)
        {
            BossDeath();
        }
    }

    private void BossDeath()
    {
        //DIE
        blockadeObject.SetActive(false);
        manager.ChangeTrophyState("goofylanguage", Trophy.TrophyState.UNLOCKED, true);
        Debug.Log("He ded");
        fightStarted = false;
        SaveSystem.level = SaveData.Level.PYTHON;
        exit.locked = false;
        healthbar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void RockAttack()
    {
        messageManager.ShowMessage("python-try-to-survive");
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
            // win
            editor.CloseCodeEditor();
            health--;
            Time.timeScale = 1f;
        }
        yellowPython.SetActive(false);
        bluePython.SetActive(false);

        healthbar.value = health;

        gameObject.GetComponent<SpriteRenderer>().sprite = normal;
        IsAttacking = false;
    }
}
