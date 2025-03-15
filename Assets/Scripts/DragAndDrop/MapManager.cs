using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private DragMap[] dragMaps;
    private DragMap currentMapIsDrag;

    public DragMap CurrentMapIsDrag
    {
        get => currentMapIsDrag;
        set => currentMapIsDrag = value;
    }
    private void AutoSwapMap()
    {
        /*DragMap drag = eventData.pointerDrag?.GetComponent<DragMap>();
        if (transform.childCount > 0)
        {
            DragMap currentChild = transform.GetComponentInChildren<DragMap>();
            if (currentChild != null && currentChild != drag) 
            { Transform dragOrigin = drag.OriginParent;
                Transform childOrigin = transform;

                currentChild.OriginParent = dragOrigin;
                currentChild.transform.SetParent(dragOrigin);

                drag.OriginParent = childOrigin;
            }
        }
        else
        {
            drag.OriginParent = transform;
        }*/
    }

    private void FindNearMap()
    {
        foreach (var map in dragMaps)
        {
            
        }
    }
}
