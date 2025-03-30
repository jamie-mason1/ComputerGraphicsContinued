using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{
    public void WinScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void loseScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
