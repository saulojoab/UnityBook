using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour {

    public bool maintainWidth = true;
    [Range(-1, 1)]
    public int adaptPosition;

    float defaultWidth;
    float defaultHeight;
    
    Vector3 CameraPos;
    
    void Start()
    {
        // Pegando a resolução mais alta suportada pelo monitor do usuário - Saulo.
        var resolution = Screen.resolutions[Screen.resolutions.Length - 1];
        Debug.Log("Resolução nativa da tela: " + resolution.width + " x " + resolution.height);
        
        // Pega a posição da Câmera.
        CameraPos = Camera.main.transform.position;

        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }
    
    void Update()
    {
        if (maintainWidth)
        {
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;

            //CameraPos.y was added in case camera's y is not in 0.
            Camera.main.transform.position = new Vector3(CameraPos.x, CameraPos.y + adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        }
        else
        {
            //CameraPos.x was added in case camera's x is not in 0.
            Camera.main.transform.position = new Vector3(CameraPos.x + adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        }
    }
}
