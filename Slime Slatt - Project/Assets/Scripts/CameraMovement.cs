using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0;


    private float xMax; //limit for x value
    private float yMin; // limit for y value 
    


    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    private void Update() {

        GetInput();
    }


    private void GetInput() //WASD movement 
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime); //time.delta = amount of time that has passed since last time update was called( so camera moves the same speed on all devices
  
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);

        }


        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);

    }

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(new Vector3(1, 0)); // 1,0 bottom right of camera box

        xMax = maxTile.x - worldPoint.x; //determines how much you can move
        yMin = maxTile.y - worldPoint.y;
    }

}
