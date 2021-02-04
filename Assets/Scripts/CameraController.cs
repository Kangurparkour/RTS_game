using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform cameraTransform;
    public LayerMask groundMask = -1;


    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 1f;

    [Header("Map max/min size")]
    public bool Maplimit;
    public int mapMax;
    public int mapMin;

    [Header("Zoom max/min size")]
    public float sensivityScroll = 2f;
    public float zoomMax;
    public float zoomMin;
    private float zoomPos;
    public float heightDampening = 5f;

    Vector2 p1;
    Vector2 p2;

    [Header("Screen Edge Move")]
    public float screenEdgeBorder;
    public float screescreenEdgeMovementSpeed;


    void Start()
    {
        cameraTransform = transform;
    }


    void Update()
    {
        Controller();
        CameraRotation();
        LimitMap();
    }

    private void Controller()
    {
        // Keyboard move

        Vector3 keyboard_move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        keyboard_move = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * keyboard_move * speed * Time.deltaTime;
        keyboard_move = cameraTransform.InverseTransformDirection(keyboard_move);

        cameraTransform.Translate(keyboard_move, Space.Self);

        //zoom Scroll

        float distanceToGround = CalcualteHight();
        zoomPos -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * sensivityScroll;
        zoomPos = Mathf.Clamp01(zoomPos);
        float targetHight = Mathf.Lerp(zoomMin, zoomMax, zoomPos);

        float diff = 0;
        if (distanceToGround != targetHight)
            diff = targetHight - distanceToGround;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(cameraTransform.position.x, targetHight + diff, cameraTransform.position.z),Time.deltaTime*heightDampening);


        // Edge move

        Vector3 edge_move = new Vector3();

        Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
        Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
        Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
        Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

        edge_move.x = leftRect.Contains(Input.mousePosition) ? -1 : rightRect.Contains(Input.mousePosition) ? 1 : 0;
        edge_move.z = upRect.Contains(Input.mousePosition) ? 1 : downRect.Contains(Input.mousePosition) ? -1 : 0;

        edge_move = Quaternion.Euler(new Vector3(0f,transform.eulerAngles.y,0f)) * edge_move * screescreenEdgeMovementSpeed * Time.deltaTime;
        edge_move = cameraTransform.InverseTransformDirection(edge_move);

        cameraTransform.Translate(edge_move,Space.Self);
    }


    private void CameraRotation()
    {
        //TODO : rotate Q or E

        if (Input.GetMouseButton(2))
        {
            cameraTransform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime, Space.World);
        }


    }

    private void LimitMap()
    {
        if (Maplimit == false)
            return;

        cameraTransform.position = new Vector3(Mathf.Clamp(cameraTransform.position.x, mapMin, mapMax), cameraTransform.position.y, Mathf.Clamp(cameraTransform.position.z, mapMin, mapMax));
    }

    private float CalcualteHight()
    {
        Ray ray = new Ray(cameraTransform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit,groundMask.value))
        {
            Debug.Log(hit.point.magnitude.ToString());
            return (hit.point - cameraTransform.position).magnitude;
        }
        
        
        return 0;
    }

}
