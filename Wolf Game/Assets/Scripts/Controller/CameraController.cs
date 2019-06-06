using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    private bool dragging = false;
    private Vector3 mouseOrigin;

    void Start()
    {
        var x = Utilities.HEX_SIZE * Mathf.Sqrt(3)/2 * (Utilities.MAP_WIDTH);
        var y = Utilities.HEX_SIZE * 3/2f * (Utilities.MAP_HEIGHT/2);
        mainCamera.transform.position = new Vector3(x, y, -10);
        mainCamera.orthographicSize = (Utilities.MAX_CAMERA_SIZE + Utilities.MIN_CAMERA_SIZE) / 2;
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


        // Pan Camera
        if(Input.GetMouseButtonDown(0))
        {
            dragging = true;
            mouseOrigin = Input.mousePosition;
            mouseOrigin.z = 0;
            mouseOrigin = mainCamera.ScreenToWorldPoint(mouseOrigin);
        }
 
        if(dragging)
        {
            var currentMouse = Input.mousePosition;
            currentMouse.z = 0;
            currentMouse = mainCamera.ScreenToWorldPoint(currentMouse);

            var newX = transform.position.x + (mouseOrigin.x - currentMouse.x);
            var newY = transform.position.y + (mouseOrigin.y - currentMouse.y);

            var newPos = new Vector3(
                Mathf.Max(Mathf.Min(newX, Utilities.MAX_CAMERA_X), Utilities.MIN_CAMERA_X),
                Mathf.Max(Mathf.Min(newY, Utilities.MAX_CAMERA_Y), Utilities.MIN_CAMERA_Y), 
                -10
            );

            transform.position = newPos;
        }
 
        dragging &= !Input.GetMouseButtonUp(0);
    }
}
