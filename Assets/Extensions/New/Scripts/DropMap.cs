using UnityEngine;
using UnityEngine.EventSystems;

public class DropMap : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DragMap drag = eventData.pointerDrag?.GetComponent<DragMap>();
        if (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(drag.OriginParent);
        }
        drag.OriginParent = transform;
        Debug.Log("OK");
    }
}