using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
     AudioSource AmogusSound;

     void Start()
     {
          AmogusSound = GetComponent<AudioSource>();
     }

     public void PlayGame()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     }

     public void GoToMain()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
     }

     public void GoToMainFromHTP()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
     }

     public void GoToMainFromCreds()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
     }

     public void GoToMainFromDeath()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
     }

     public void GoToDeath()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
     }

     public void GoToCreds()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
     }

     public void GoToHTP()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
     }

     public void PlaySound()
     {
          AmogusSound.Play();
     }


}
