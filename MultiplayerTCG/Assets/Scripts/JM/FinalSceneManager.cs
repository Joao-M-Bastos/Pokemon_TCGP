using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneManager : MonoBehaviour
{
    public void OnVoltarClick()
    {
        SceneManager.LoadScene(1);
    }
}
