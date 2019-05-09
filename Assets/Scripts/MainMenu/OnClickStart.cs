using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickStart : MonoBehaviour
{


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
