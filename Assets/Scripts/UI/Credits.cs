using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class Credits : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            StartCoroutine(EnableRoutine());
        }

        private IEnumerator EnableRoutine()
        {
            yield return new WaitForSeconds(1f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}