using UnityEngine;

public class DragDrop2D : MonoBehaviour
{
    [SerializeField] private Transform originParent;

    public Transform OriginParent
    {
        get => originParent;
        set => originParent = value;
    }

    Vector3 offset;
    Collider2D collider2d;
    public string destinationTag = "DropArea";

    public DropArea currentArea;
    private bool hasSwapped = false; 

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
        CheckDropArea();
    }

    void OnMouseUp()
    {
        collider2d.enabled = false;
        DropItem();
        hasSwapped = false;
        collider2d.enabled = true;
    }

    private void CheckDropArea()
    {
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hitInfo;
        if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hitInfo.transform.tag == destinationTag && !hasSwapped)
            {
                if (hitInfo.transform.GetComponent<DropArea>().objDrop)
                {
                    hitInfo.transform.GetComponent<DropArea>().objDrop.transform.position = currentArea.transform.position;
                    currentArea.SwapObjDrop( hitInfo.transform.GetComponent<DropArea>().objDrop);
                }
                currentArea = hitInfo.transform.GetComponent<DropArea>();
                hitInfo.transform.GetComponent<DropArea>().SwapObjDrop(this);
                hasSwapped = true;
            }
        }
        else
        {
            hasSwapped = false;   
        }
    }

    private void DropItem()
    {
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hitInfo;
        if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if (hitInfo.transform.tag == destinationTag)
            {
                transform.position = hitInfo.transform.position + new Vector3(0, 0, -0.01f);
                currentArea = hitInfo.transform.GetComponent<DropArea>();
                hasSwapped = false;
               hitInfo.transform.GetComponent<DropArea>().SwapObjDrop(this);
            }
        }
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}