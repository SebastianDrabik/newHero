using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrophiesManager : MonoBehaviour
{ 
    public GameObject RowPrefab;
    public GameObject Content;

    public void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            foreach (Trophy t in GameManager.Instance.trophies)
            {
                GameObject row = Instantiate(RowPrefab, Content.transform);
                row.GetComponent<TrophyObjectController>().SetData(t);
            }
        }
    }

    public void Hide()
    {
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}
