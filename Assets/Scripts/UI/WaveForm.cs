using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
public class WaveForm : MonoBehaviour
{
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private CanvasGroup container;

    private bool isCountingDown;
    private float countDownTimer;

    private void Awake()
    {
        WaveManager.OnNewWave +=OnNewWave;
    }


    private void OnNewWave(int waveNumber)
    {
        isCountingDown = true;
        countDownTimer = WaveManager.timeBetweenWaves;


        //reset
        countDownText.alpha = 1;
        
        waveNumberText.SetText(waveNumber.ToString());
    }


    private void Update()
    {
        if (!isCountingDown)return;

        countDownTimer -= Time.deltaTime;
        countDownText.gameObject.SetActive(countDownTimer>0.1f);

        if (countDownTimer % 1 > 0.5f)
        {
            countDownText.alpha -= Time.deltaTime * 2;
        }
        else
        {
            countDownText.alpha += Time.deltaTime * 2;
        }


        if (countDownTimer < WaveManager.timeBetweenWaves * 0.5f)
        {
            container.alpha += Time.deltaTime * 0.5f;
        }

        if (countDownTimer < 0.1f)
        {
            isCountingDown = false;
            container.alpha = 0;
        }
        else
        {
            countDownText.SetText(Mathf.Round(countDownTimer).ToString(CultureInfo.InvariantCulture));
        }

        
    }


    private void OnDestroy()
    {
        WaveManager.OnNewWave -= OnNewWave;
    }
}
