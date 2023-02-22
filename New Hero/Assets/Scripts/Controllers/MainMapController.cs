using System;
using UnityEngine;


public class MainMapController : MonoBehaviour
{
    public GameObject firewall;
    public SaveData.Level minFirewallLevel;


    void Start()
    {
        if (SaveSystem.level <= minFirewallLevel)
            firewall.SetActive(false);
    }
}