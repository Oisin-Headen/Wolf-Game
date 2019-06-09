using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSpaceController : MonoBehaviour
{
    private void OnMouseDown()
    {
        var position = gameObject.transform.parent.GetComponent<SpaceController>().GetPosition();
        Camera.main.GetComponent<CameraController>().SetPosition(position);
        Debug.Log("Minimap");
    }
}

