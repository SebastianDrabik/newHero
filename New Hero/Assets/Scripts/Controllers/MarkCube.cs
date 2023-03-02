using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MarkCube : MonoBehaviour
{
    public static MarkCube Instance;

    Vector2 velocity = new(0f, 0f);
    public GameObject boxAttackPrefab;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject Intro;
    public FightManager codeEditor;
    public GameObject shadow;
    public GameManager manager;
    //public Button runCode;
    public MessageManager messageManager;
    public new Transform camera;
    public PauseMenu canvas;
    public PlayerMovement movement;

    public GameObject audioSpace;

    private float abovePlayer = 10f;
    private bool isAttacking = false;
    private float timerMult = 1f;
    private float timer = 2f;
    private int stage = 0;
    int counter = 0;
    private bool isDead = false;
    private PauseMenu pauseMenu;

    [HideInInspector]
    public bool isFighting = false;

    private List<GameObject> bullets = new();


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        if (SaveSystem.level>=SaveData.Level.MARK_CUBE)
        {
            audioSpace.SetActive(false);
            Intro.SetActive(false);
            gameObject.SetActive(false);
        }
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
    }

    private void Start()
    {
        timer = 3f;
        canvas.SetDisabled(true);
        Invoke(nameof(HideIntro), 3);
        if (SaveSystem.level < SaveData.Level.MARK_CUBE)
        {
            manager.ChangeTrophyState("marco", Trophy.TrophyState.IN_PROGRESS);
            isFighting = true;
            movement.SetMovementDisabled(true);
        }
    }

    void HideIntro()
    {
        movement.SetMovementDisabled(false);
        Intro.SetActive(false);
        canvas.SetDisabled(false);
    }
    private void Update()
    {
        if (isDead) return;
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
            if(counter == 1)
            {
                messageManager.ShowMessage("cube-bullet");
            }
            if(counter==10 && stage == 0)
            {
                stage = 1;
                messageManager.ShowMessage("cube-box");
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

        bullets.Add(bullet);
    }

    private void DestroyAllBullets()
    {
        if (bullets.Count == 0)
            return;
        foreach (GameObject bullet in bullets)
            if(bullet!=null && bullet.gameObject!=null)
                Destroy(bullet);
    }

    public void JumpAttack()
    {
        Time.timeScale = 0f;
        codeEditor.OpenCodeEditor("marco");
        //pauseMenu.SetDisabled(true);
        isAttacking = false;
        stage = -1;
    }

    public void ResetAfterAttack()
    {
        DestroyAllBullets();
        Time.timeScale = 1f;
        isAttacking = false;
        shadow.SetActive(false);
        counter = 0;
        timer = 2f;
        stage = 0;
        timerMult = 1f;
        abovePlayer = 10f;
        gameObject.transform.SetPositionAndRotation(new Vector3(0f, 11.5f), Quaternion.identity);
        shadow.transform.localScale = new Vector3(0.4046f, 0.1798318f, shadow.transform.localScale.z);
    }
    IEnumerator Jump()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 10f);

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    public void HandleCodeExecution(bool result)
    {
        if (result)
        {
            codeEditor.CloseCodeEditor();
            StartCoroutine(nameof(DeathAni));
        }
    }

    //WIN
    IEnumerator DeathAni()
    {
        //I hate sand
        ResetAfterAttack();
        movement.SetMovementDisabled(true);
        pauseMenu.SetDisabled(true);
        gameObject.GetComponent<Animator>().SetTrigger("Death");
        float time = Array.Find(gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips, clip => clip.name == "Marco_Death").length;
        isDead = true;
        Cinemachine.CinemachineVirtualCamera _camera = camera.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        float initialOrtoSize = _camera.m_Lens.OrthographicSize;
        _camera.m_Follow = gameObject.transform;
        _camera.m_Lens.OrthographicSize = 3f;
        yield return new WaitForSeconds(time);
        movement.SetMovementDisabled(false);
        yield return new WaitForSeconds(0.9f);
        isFighting = false;
        _camera.m_Follow = player.transform;
        _camera.m_Lens.OrthographicSize = initialOrtoSize;
        audioSpace.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.ChangeTrophyState("marco", Trophy.TrophyState.UNLOCKED, true);
        pauseMenu.SetDisabled(false);
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
