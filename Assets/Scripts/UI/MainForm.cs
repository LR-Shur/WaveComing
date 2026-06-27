using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MainForm : MonoBehaviour
{
   [SerializeField] private Image healthBar;
   [SerializeField] private TMP_Text healthText;


   private void OnEnable()
   {
        GameEvents.OnPlayerHealthChanger += OnPlayerHealthChanger;
   }

    private void OnPlayerHealthChanger(float currentHealth, float startHealth)
    {
        healthBar.fillAmount = currentHealth / startHealth;
        healthText.SetText($"{currentHealth}/{startHealth}");
    }

   private void OnDisable()
   {
        GameEvents.OnPlayerHealthChanger -= OnPlayerHealthChanger;
   }
}
