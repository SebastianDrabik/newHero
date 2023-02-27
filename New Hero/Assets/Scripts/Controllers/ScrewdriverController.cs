using System.Collections;
using UnityEngine;

public class ScrewdriverController : MonoBehaviour
{
    public GameObject imageGet;
    public GameObject imageBG;
    public MessageManager manager;
    public void GetScrewdriver()
    {
        StartCoroutine(nameof(show));
    }

    IEnumerator show()
    {
        imageGet.SetActive(true);
        imageBG.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        manager.ShowMessage("screwdriver");
        yield return new WaitForSeconds(3.1f);
        imageGet.SetActive(false);
        imageBG.SetActive(false);
        gameObject.SetActive(false);
    }

}
