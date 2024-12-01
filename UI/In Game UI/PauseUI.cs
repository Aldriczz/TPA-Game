using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; }
    
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject MainMenuUI;
    [SerializeField]
    private GameObject UpgradeMenuUI;
    
    // Start is called before the first frame update
    private void Start()
    {
        if(Instance == null && Instance != this) Instance = this; else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUIToggle();
        }
    }

    public void PauseUIToggle()
    {
        if (pauseUI.activeSelf)
        {
            Time.timeScale = 1f;
            pauseUI.SetActive(false);
            Player.Instance.EnableInput();
        }
        else
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            Player.Instance.DisableInput();
        }
    }

    public void BackToUpgrade()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        MainMenuUI.SetActive(false);
        UpgradeMenuUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        MainMenuUI.SetActive(true);
        UpgradeMenuUI.SetActive(false);
    }
}
