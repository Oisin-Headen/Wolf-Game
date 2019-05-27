using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    // private bool dragging = false;
    // private Vector3 mouseOrigin;

    void Start()
    {
        var x = Utilities.HEX_SIZE * Mathf.Sqrt(3)/2 * (Utilities.MAP_WIDTH);
        var y = Utilities.HEX_SIZE * 3/2f * (Utilities.MAP_HEIGHT/2);
        mainCamera.transform.position = new Vector3(x, y, -10);
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            if(gameObject.transform.position.x < Utilities.MAX_CAMERA_X)
            {
                transform.Translate(new Vector3(Utilities.CAMERA_SPEED * Time.deltaTime,0,0));
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(gameObject.transform.position.x > Utilities.MIN_CAMERA_X)
            {
                transform.Translate(new Vector3(-Utilities.CAMERA_SPEED * Time.deltaTime,0,0));
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(gameObject.transform.position.y > Utilities.MIN_CAMERA_Y)
            {
                transform.Translate(new Vector3(0,-Utilities.CAMERA_SPEED * Time.deltaTime,0));
            }
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            if(gameObject.transform.position.y < Utilities.MAX_CAMERA_Y)
            {
                transform.Translate(new Vector3(0,Utilities.CAMERA_SPEED * Time.deltaTime,0));
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mainCamera.orthographicSize = Mathf.Min(Utilities.MAX_CAMERA_SIZE, 
                mainCamera.orthographicSize + Time.deltaTime * Utilities.CAMERA_ZOOM_SPEED);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mainCamera.orthographicSize = Mathf.Max(Utilities.MIN_CAMERA_SIZE, 
                mainCamera.orthographicSize - Time.deltaTime * Utilities.CAMERA_ZOOM_SPEED);
        }


        // // Pan Camera
        // if(Input.GetMouseButtonDown(0))
        //  {
        //     dragging = true;
        //     mouseOrigin = Input.mousePosition;
        //      //Get the ScreenVector the mouse clicked
        //     //  panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition); 
        //  }
 
        //  if(dragging)
        //  {
        //      //Get the difference between where the mouse clicked and where it moved
        //     Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - mouseOrigin; 
        //      //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
        //     Vector3 move = new Vector3(pos.x * Time.deltaTime * Utilities.CAMERA_SPEED,
        //         pos.y * Time.deltaTime * Utilities.CAMERA_SPEED, -10);
        //     mainCamera.transform.Translate(move, Space.Self);
        //  }
 
        //  if(Input.GetMouseButtonUp(0))
        //  {
        //      dragging = false;
        //  }
    }
}
