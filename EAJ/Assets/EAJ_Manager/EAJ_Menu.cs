using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EAJ
{
    public class EAJ_Menu : MonoBehaviour
    {
        public TextMeshProUGUI PlayerOneReadyText;
        public TextMeshProUGUI PlayerTwoReadyText;
        public TextMeshProUGUI CountdownText;
        public TextMeshProUGUI LoadingText;
        
        private InputManager PlayerOneInput;
        private WeaponSystem PlayerTwoInput;

        private bool bPlayerOneReady = false;
        private bool bPlayerTwoReady = false;
        private float Countdown = 3f;
        private bool bCountdownStarted = false;

        void Start()
        {
            PlayerOneInput = FindObjectOfType<InputManager>();
            PlayerOneReadyText.enabled = false;
            PlayerTwoReadyText.enabled = false;
            CountdownText.enabled = false;
            LoadingText.enabled = false;
        }

        void Update()
        {
            if (PlayerTwoInput == null)
            {
                PlayerTwoInput = FindObjectOfType<WeaponSystem>();
            }
            
            if (!bPlayerOneReady && PlayerOneInput.MenuReadyInput)
            {
                bPlayerOneReady = true;
                PlayerOneReadyText.enabled = true;
            }

            if (PlayerTwoInput == null)
            {
                return;
            }
            
            if (!bPlayerTwoReady && PlayerTwoInput.MenuReadyPressed)
            {
                bPlayerTwoReady = true;
                PlayerTwoReadyText.enabled = true;
            }

            if (bPlayerOneReady && bPlayerTwoReady && !bCountdownStarted)
            {
                StartCoroutine(StartCountdown());
            }
        }

        private IEnumerator StartCountdown()
        {
            bCountdownStarted = true;
            CountdownText.enabled = true;

            while (Countdown > 0)
            {
                CountdownText.text = Countdown.ToString("F0");
                yield return new WaitForSecondsRealtime(1f);
                Countdown -= 1f;
            }

            CountdownText.enabled = false;
            LoadingText.enabled = true;
            SceneManager.LoadScene("EAJ_Arena");
        }

    }
}
