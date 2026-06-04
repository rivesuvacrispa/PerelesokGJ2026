using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Transform camTransform;
        [SerializeField] private float rotationSpeed;

        private float currentRot = 0;
        
        public void StartGame()
        {
            Application.targetFrameRate = 60;
            SceneManager.LoadScene(1);
        }

        private void Update()
        {
            currentRot += Time.deltaTime * rotationSpeed;
            camTransform.rotation = Quaternion.Euler(0, currentRot, 0);
        }
    }
}