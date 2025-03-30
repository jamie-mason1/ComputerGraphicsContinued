using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitbottom : MonoBehaviour
{
    //win sound lose sound
    public void WinScene()
    {
        SceneManager.LoadScene("WinScreen");
        //play win sound
    }

    public void loseScene()
    {
        SceneManager.LoadScene("LoseScreen");
        //play lose sound
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            loseScene();
        }

        if (other.gameObject.tag == "Push")
        {
            WinScene();
        }
    }

}
