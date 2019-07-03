using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Assets assets;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Camera>().backgroundColor = assets.MinimapUnexplored;
    }
}
