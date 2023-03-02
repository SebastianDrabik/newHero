using System.Collections;
using UnityEngine;

public class DeathScreenController: MonoBehaviour
{
    public GameObject deathScreen;

    public void ShowDeathScreen()
    {
        Time.timeScale = 0f;
        deathScreen.SetActive(true);
    }

    public void Restart()
    {
        SaveGame.RestartGame();
        SaveSystem.health = PlayerInteraction.maxHealth;
        Time.timeScale = 1f;
        deathScreen.SetActive(false);
    }
}