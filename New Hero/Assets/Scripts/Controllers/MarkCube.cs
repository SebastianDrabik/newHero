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
    public new Transform camera;
    public PauseMenu canvas;


    private float abovePlayer = 10f;
    private bool isAttacking = false;
    private float timerMult = 1f;
    private float timer = 2f;
    private int stage = 0;
    int counter = 0;
    private bool isDead = false;

    [HideInInspector]
    public bool isFighting = false;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        runCode.onClick.AddListener(Check);
        if (SaveSystem.level>=SaveData.Level.MARK_CUBE)
        {
            Intro.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void Check()
    {
        canvas.SetDisabled(false);
        if (codeEditor.GetComponent<FightManager>().CheckCode())
        {
            Debug.Log("Boss Pokonany");
            gameObject.GetComponent<Animator>().SetTrigger("Death");
            timer = 1000f;
            StartCoroutine(nameof(DeathAni));
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
        canvas.SetDisabled(true);
        Invoke(nameof(HideIntro), 3);
        if (SaveSystem.level < SaveData.Level.MARK_CUBE)
            isFighting = true;
    }

    void HideIntro()
    {
        Intro.SetActive(false);
        canvas.SetDisabled(false);
    }
    private void Update()
    {
        if (isDead) return;
        timer -= Time.deltaTime;
        if(timer<=0f)
        {
            // TODO
            GameManager.Instance.ChangeTrophyState("marco", Trophy.TrophyState.IN_PROGRESS);
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
                StartCoroutine(nameof(Jump));
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
        StartCoroutine(nameof(DestroyBullet), bullet);
    }

    public void JumpAttack()
    {
        Time.timeScale = 0f;
        codeEditor.GetComponent<FightManager>().OpenCodeEditor("marco");
        canvas.GetComponent<PauseMenu>().SetDisabled(true);
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
        isDead = true;
        Cinemachine.CinemachineVirtualCamera _camera = camera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        float initialOrtoSize = _camera.m_Lens.OrthographicSize;
        _camera.m_Follow = gameObject.transform;
        _camera.m_Lens.OrthographicSize = 3f;
        yield return new WaitForSeconds(time + 0.9f);
        isFighting = false;
        _camera.m_Follow = player.transform;
        _camera.m_Lens.OrthographicSize = initialOrtoSize;
        gameObject.SetActive(false);
        GameManager.Instance.ChangeTrophyState("marco", Trophy.TrophyState.UNLOCKED, true);
        SaveSystem.level = SaveData.Level.MARK_CUBE;
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(6);
        if(bullet == null)
        {
            yield break;
        }
        bullet.GetComponent<Animator>().SetTrigger("Break");
        Destroy(bullet, 1);
    }
}
