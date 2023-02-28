using System;
using UnityEngine;


public class MainMapController : MonoBehaviour
{
    public GameObject firewall;
    public GameObject PathMiddle;
    public SaveData.Level minFirewallLevel;
    public DoorController caveEntrance;
    [Space]
    private GameManager manager;

    public NPCManager SGP;
    public NPCManager SGP2;

    public BoxCollider2D PC_Trigger;
    public GameObject firewallBoss;
    public GameObject screwdriver;
    //[Header("Dialogue key before fight with python")]
    //public string SGPKey1;
    //[Header("Dialogue key after fight with python")]
    //public string SGPKey2;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (SaveSystem.level < minFirewallLevel)
        {
            firewall.SetActive(false);
            PathMiddle.SetActive(true);
        }
        if(SaveSystem.level == SaveData.Level.CPP_ADVANCED)
        {
            manager.ShowObjective("find-sgp");
        }

        if(SaveSystem.level >= SaveData.Level.PYTHON)
        {
            SGP.gameObject.SetActive(false);
            manager.ShowObjective("firewall");
        }
        if(SaveSystem.level >= SaveData.Level.CPP_MASTER)
        {
            SGP2.gameObject.SetActive(false);
            manager.HideObjective();
        }
        if (SaveSystem.level >= SaveData.Level.END_GAME)
        {
            PC_Trigger.enabled = false;
            firewallBoss.SetActive(false);
            screwdriver.SetActive(false);
            caveEntrance.locked = false;
        }
    }
}