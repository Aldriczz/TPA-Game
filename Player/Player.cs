using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [HideInInspector] public List<Tile> resultMap;
    private InputControl inputControl;
    private Animator animator;
    
    public bool isMoving = false; 
    public bool isClickedWhileMoving = false;

    public Player()
    {
        inputControl = new InputControl();
        AssignInput();
    }
    void AssignInput()
    {
        inputControl.Player.Move.performed += ctx => OnClickMove();
    }
    
    public void EnableInput()
    {
        inputControl.Enable();
    }

    public void DisableInput()
    {
        inputControl.Disable();
    }

    public void OnClickMove()
    {
        if (isMoving)
        {
            isClickedWhileMoving = true;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            resultMap = HoverManager.Instance.path;
            if (resultMap != null && resultMap.Count > 0 && resultMap.Count <= 15)
            {
                isMoving = true;
            }
            else
            {
                Debug.Log("No valid path found.");
            }
        }
    }
    
}
