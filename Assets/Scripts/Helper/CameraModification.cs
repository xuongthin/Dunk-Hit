using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModification : MonoBehaviour
{
    private void Start()
    {
        float w = Screen.width;
        float h = Screen.height;

        float size = (h / w) * 10.8f / 2;
        Camera.main.orthographicSize = size;
    }
}
