using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;

    Transform cameraTransform;
    public LayerMask groundMask = -1;
    public GameObject destiantionPoint;
    public Texture2D cursorTexture;
    List<Unit> selectedUnits = new List<Unit>();

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

    RectTransform selectionBox;
    Rect selectionRect;
    Rect boxRect;

    Vector2 mousePos;
    Vector2 ScreenPos;
    [SerializeField] LayerMask commandLayerMask = -1;

    void Start()
    {
        cameraController = this;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        selectionBox = GetComponentInChildren<Image>(true).transform as RectTransform;
        cameraTransform = transform;
        selectionBox.gameObject.SetActive(false);
    }


    void Update()
    {
        Controller();
        CameraRotation();
        LimitMap();
        SelecionBoxUpdate();
    }

    private void Controller()
    {
        mousePos = Input.mousePosition;
        ScreenPos = Camera.main.ScreenToViewportPoint(mousePos);
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

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(cameraTransform.position.x, targetHight + diff, cameraTransform.position.z), Time.deltaTime * heightDampening);


        // Edge move

        Vector3 edge_move = new Vector3();

        Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
        Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
        Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
        Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

        edge_move.x = leftRect.Contains(mousePos) ? -1 : rightRect.Contains(mousePos) ? 1 : 0;
        edge_move.z = upRect.Contains(mousePos) ? 1 : downRect.Contains(mousePos) ? -1 : 0;

        edge_move = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * edge_move * screescreenEdgeMovementSpeed * Time.deltaTime;
        edge_move = cameraTransform.InverseTransformDirection(edge_move);

        cameraTransform.Translate(edge_move, Space.Self);
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

        if (Physics.Raycast(ray, out hit, groundMask.value))
        {
            Debug.Log(hit.point.magnitude.ToString());
            return (hit.point - cameraTransform.position).magnitude;
        }


        return 0;
    }

    private void SelecionBoxUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionBox.gameObject.SetActive(true);
            selectionRect.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            selectionRect.size = mousePos - selectionRect.position;
            boxRect = AbsRect(selectionRect);
            selectionBox.anchoredPosition = boxRect.position;
            selectionBox.sizeDelta = boxRect.size;
            if (boxRect.size.x != 0 || boxRect.size.y != 0)
                UpdateSelcetedUnits();

        }

        if (Input.GetMouseButtonDown(1))
        {
            GiveCommand();
        }
    }

    private Rect AbsRect(Rect rect)
    {
        if (rect.width < 0)
        {
            rect.x += rect.width;
            rect.width *= -1;
        }
        if (rect.height < 0)
        {
            rect.y += rect.height;
            rect.height *= -1;
        }

        return rect;
    }

    private void UpdateSelcetedUnits()
    {
        selectedUnits.Clear();
        foreach (Unit unit in Unit.SelectableUnits)
        {
            if (!unit || !unit.IsAlive)
                continue;

            var pos = unit.transform.position;
            var posInScreen = Camera.main.WorldToScreenPoint(pos);
            bool inRect = isInRect(boxRect, posInScreen);
            (unit as ISelectable).SetSelected(inRect || IsMouseOnUnits(unit));

            if (inRect || IsMouseOnUnits(unit))
            {
                selectedUnits.Add(unit);
            }


        }
    }

    private bool isInRect(Rect rect, Vector2 point)
    {
        return point.x >= rect.position.x && point.x <= (rect.position.x + rect.size.x)
            && point.y >= rect.position.y && point.y <= (rect.position.y + rect.size.y);
    }





    private void GiveCommand()
    {
        Ray ray = Camera.main.ViewportPointToRay(ScreenPos);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100000, commandLayerMask))
        {
            object commandData = null;
            if (hit.collider is TerrainCollider)
            {
                commandData = hit.point;
            }
            else
            {
                commandData = hit.collider.gameObject.GetComponent<Unit>();
            }

            GiveCommand(commandData);
        }
    }

    private void GiveCommand(object command)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.SendMessage("GetCommand", command, SendMessageOptions.DontRequireReceiver);
        }
    }
    private bool IsMouseOnUnits(Unit unit)
    {
        Ray ray = Camera.main.ViewportPointToRay(ScreenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, commandLayerMask))
        {
            Unit hitUnit = hit.collider.gameObject.GetComponent<Unit>();

            if (hitUnit != null)
                if (hitUnit == unit)
                    return true;
        }
        return false;
    }
}
