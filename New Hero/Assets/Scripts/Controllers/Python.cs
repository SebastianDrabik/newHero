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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
