using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragMap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform originParent;
    [SerializeField] private Image img;

    public Transform OriginParent
    {
        get => originParent;
        set
        {
            originParent = value;
        }
    }

    private void OnValidate()
    {
        if (originParent == null) originParent = transform.parent;
        if (img == null) img = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (img != null) img.raycastTarget = false;
        originParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root); 
        transform.position = eventData.position;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.raycastTarget = true;
        transform.SetParent(originParent);
    }
}