using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
   [SerializeField] private GameObject Description;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Description.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
