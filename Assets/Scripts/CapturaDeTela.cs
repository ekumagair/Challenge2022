using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturaDeTela : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) && StaticClass.debug == true)
        {
            ScreenCapture.CaptureScreenshot("Guerreiros Mundiais " + Application.version + " " + Random.Range(0, 20000).ToString() + ".png");
        }
    }
}
