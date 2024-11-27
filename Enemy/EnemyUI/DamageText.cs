using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text text;
    private Vector3 StartingPosition;
    private Vector3 moveDirection = Vector3.up;
    private float lifetime = 0.5f;
    private float moveSpeed = 0.5f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Activate(int damage, Color color)
    {
        timer = lifetime;
        text.color = color;
        text.text = $"-{damage}";
        gameObject.SetActive(true);
        StartingPosition = transform.position;
    }

    // Update is called once per frame
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
