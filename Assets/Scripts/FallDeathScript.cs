using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathScript : MonoBehaviour
{

    public float threshhold;
 

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag=="Player")
        {
            SceneManager.LoadScene(6);

        }

    }
    
}