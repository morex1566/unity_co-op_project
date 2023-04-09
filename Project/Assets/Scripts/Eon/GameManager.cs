using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    GameObject health;

    int healthCount = 10;

    private void Start()
    {
        instance = this;
    }

    public void HealthCheck()
    {
        if (healthCount == 1)
        {
            SceneManager.LoadScene("Editor");
        }
        health.transform.GetChild(healthCount - 1).gameObject.SetActive(false);
        healthCount--;
    }
 
}
