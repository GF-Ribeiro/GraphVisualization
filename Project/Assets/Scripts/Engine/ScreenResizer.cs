using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetNativeRes();
    }

    public static void SetNativeRes()
    {
       Screen.SetResolution(1280, 720, false);
    }
}
