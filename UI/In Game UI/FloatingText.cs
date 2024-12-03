using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Text text;
    private Vector3 StartingPosition;
    private Vector3 moveDirection = Vector3.up;
    private float lifetime = 0.5f;
    private float moveSpeed = 0.5f;
    private float timer;

    public void ActivateDamageText(int damage, Color color)
    {
        timer = lifetime;
        text.color = color;
        text.text = $"-{damage}";
        gameObject.SetActive(true);
        StartingPosition = transform.position;
        
        if (Color.red == color) text.fontSize = 88;
        else text.fontSize = 68;
    }
    
    public void ActivateLevelUp()
    {
        timer = lifetime;
        text.color = Color.yellow;
        text.text = "Level Up";
        gameObject.SetActive(true);
        StartingPosition = transform.position;
        text.fontSize = 44;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            transform.position = StartingPosition;  
        }
    }
}
