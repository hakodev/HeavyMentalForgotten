using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminToolHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.F1) && Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            // Load the next level
            int nextLevelIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextLevelIndex);
        }

        if (Input.GetKey(KeyCode.F1) && Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            // Load the previous level
            int previousLevelIndex = (SceneManager.GetActiveScene().buildIndex - 1 + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(previousLevelIndex);
        }
        
        if (Input.GetKey(KeyCode.F1) && Input.GetKeyDown(KeyCode.A))
        {
            // Load the next level specified by the NextLevelLayerA
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
        }
        
        if (Input.GetKey(KeyCode.F1) && Input.GetKeyDown(KeyCode.B))
        {
            // Load the next level specified by the NextLevelLayerB

            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerB);
        }
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //Load menu
            SceneManager.LoadScene(0);
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
