using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    private bool strangeSceneDeleted = false; 
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        if (!strangeSceneDeleted)
        {
            Scene[] scenes = SceneManager.GetAllScenes();
            foreach (Scene scene in scenes)
            {
                if (scene.name == "DontDestroyOnLoad")
                {
                    SceneManager.UnloadSceneAsync(scene);
                    strangeSceneDeleted = true;
                }
            }
        }
    }
}
