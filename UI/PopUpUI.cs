using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpUI : MonoBehaviour
{
    public static PopUpUI Instance { get; private set; }
    
    [SerializeField] 
    private GameObject FloorClearedUI;
    [SerializeField]
    private GameObject GameOverUI;
    
    [SerializeField]
    private PlayerStatsSO PlayerStats;

    public void ShowFloorClearedPopUp()
    {
        if (PlayerStats.CurrentLevel < 101)
        {
            if (PlayerStats.CurrentLevel == PlayerStats.UnlockLevel && PlayerStats.UnlockLevel < 101)
            {
                PlayerStats.UnlockLevel++;
            }
            PlayerStats.CurrentLevel++;
        }
        FloorClearedUI.SetActive(true);
    }

    public void ShowGameOverPopUp()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void FloorClearedContinueButton()
    {
        
        SceneManager.LoadScene("Game");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this; else Destroy(Instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
