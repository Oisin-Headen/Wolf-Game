using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapController : MonoBehaviour
{
    public Canvas canvas;

    public void MiniMapClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        // Find position in rect, send that to camera.
        var miniMapRect = gameObject.GetComponent<RectTransform>().rect;


        var mousePos = Input.mousePosition;
        //mousePos.y -= screenRect.y;
        var xMultiplier = (mousePos.x - Screen.width + miniMapRect.width * canvas.scaleFactor) / (miniMapRect.width * canvas.scaleFactor);
        var yMultiplier = (mousePos.y) / (miniMapRect.height * canvas.scaleFactor);


        var camPos = new Vector2(
            xMultiplier * Utilities.MAX_CAMERA_X,
            yMultiplier * Utilities.MAX_CAMERA_Y);

        Camera.main.GetComponent<CameraController>().SetPosition(camPos);
    }
}


