using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkCube : MonoBehaviour
{
    public static MarkCube Instance;

    Vector2 velocity = new(0f, 0f);
    public GameObject boxAttackPrefab;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject Intro;
    public GameObject codeEditor;
    public GameObject shadow;
    public Button runCode;


    private float abovePlayer = 10f;
    private bool isAttacking = false;
    private float timerMult = 1f;
    private float timer = 2f;
    private int stage = 0;
    int counter = 0;

    [HideInInspector]
    public bool isFighting = false;

    public string[] code =
    {
        "#include<iostream>\n\nusing namespace std;\n\nint main(){\n\tint health = 4;\n\t/*check if health equals 4 and if it does, return 0*/",
        "if(health==4){return 0;}",
        "\treturn 0;\n}"
    };

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        runCode.onClick.AddListener(check);
        if (PlayerPrefs.HasKey("Marco_Defeated"))
        {
            if (PlayerPrefs.GetInt("Marco_Defeated")==1)
            {
                Intro.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void check()
    {
        if (codeEditor.GetComponent<FightManager>().CheckCode())
        {
            Debug.Log("Boss Pokonany");
            PlayerPrefs.SetInt("Marco_Defeated", 1);
            isFighting = false;
            gameObject.GetComponent<Animator>().SetTrigger("Death");
            timer = 1000f;
            StartCoroutine("DeathAni");
        }
        else
        {
            Debug.Log("OJOJ");
        }
        codeEditor.GetComponent<FightManager>().CloseCodeEditor();
        ResetAfterAttack();
    }


    private void Start()
    {
        timer = 3f;
        Invoke("HideIntro", 3);
        if (PlayerPrefs.HasKey("Marco_Defeated"))
            if (PlayerPrefs.GetInt("Marco_Defeated") == 0)
                isFighting = true;
        else
            isFighting = true;
        
    }

    void HideIntro()
    {
        Intro.SetActive(false);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0f)
        {
            if (isAttacking)
            {
                if (abovePlayer <= 2f)
                {
                    JumpAttack();
                }
                shadow.SetActive(true);
                gameObject.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y+abovePlayer),Quaternion.identity);
                shadow.transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y-0.7f), Quaternion.identity);

                abovePlayer -= 8f * Time.deltaTime;
                if (Time.timeScale != 0f)
                {
                    shadow.transform.localScale += new Vector3(0.005f, 0.005f, 0f);
                }
                return;
            }
            if (stage == 2)
            {
                StartCoroutine("Jump");
                isAttacking = true;
            }
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
            if(counter==25 && stage == 1)
            {
                stage = 2;
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

    void JumpAttack()
    {
        Time.timeScale = 0f;
        codeEditor.GetComponent<FightManager>().OpenCodeEditor(code[0],code[2],code[1]);
        isAttacking = false;
        stage = -1;
    }

    private void ResetAfterAttack()
    {
        Debug.Log("reset");
        Time.timeScale = 1f;
        isAttacking = false;
        shadow.SetActive(false);
        counter = 0;
        timer = 2f;
        stage = 0;
        timerMult = 1f;
        abovePlayer = 10f;
        gameObject.transform.SetPositionAndRotation(new Vector3(0f, 11f), Quaternion.identity);
        shadow.transform.localScale = new Vector3(0.4046f, 0.1798318f, shadow.transform.localScale.z);
    }
    IEnumerator Jump()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 10f);

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    IEnumerator DeathAni()
    {
        //I hate sand

        float time = Array.Find(gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips, clip => clip.name == "Marco_Death").length;

        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);

    }
}
