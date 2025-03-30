using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    FmodCarSoundManager soundEvent;
    public void ChangeScene(string sceneName){
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

    }
    public void ChangeScene(string sceneName, string SoundEventPath)
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene
        soundEvent = new FmodCarSoundManager(SoundEventPath);
        if (soundEvent != null)
        {
            if (!soundEvent.IsEventPlaying())
            {
                soundEvent.StartEventSound();
            }
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

    }
    public void ChangeScene(int sceneID){
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene
        SceneManager.LoadScene(sceneID,LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

    }
    public void ChangeScene(int sceneID, string SoundEventPath)
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene
        soundEvent = new FmodCarSoundManager(SoundEventPath);
        if (soundEvent != null)
        {
            if (!soundEvent.IsEventPlaying())
            {
                soundEvent.StartEventSound();
            }
        }
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

    }
    public void ReloadCurrentScene(){
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene

        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

        SceneManager.LoadScene(thisScene,LoadSceneMode.Single);
    }
    public void ReloadCurrentScene(string SoundEventPath)
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Get current scene

        soundEvent = new FmodCarSoundManager(SoundEventPath);
        if (soundEvent != null)
        {
            if (!soundEvent.IsEventPlaying())
            {
                soundEvent.StartEventSound();
            }
        }
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentScene); // Unload manually if necessary

        SceneManager.LoadScene(thisScene, LoadSceneMode.Single);
    }



}
