using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectManager : MonoBehaviour
{
    public static DragObjectManager Instance;
    private DragItemObject currentItem;

    public bool smartDrag = true;
    public bool isDraggable = true;
    public bool canDrag = true;
    private Vector2 initialPostionMouse;
    public Vector2 initialPositionObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            OnDrag(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnEndDrag(Input.mousePosition);
        }
    }
    
    public void OnBeginDrag(Vector3 position)
    {
        if(!canDrag) return;
        canDrag = false;
        if (isDraggable && Input.GetMouseButtonDown(0))
        {
            if (smartDrag)
            {
                initialPostionMouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 newPosition = position;
                newPosition.z = 0f;
                RaycastHit2D hit = Physics2D.Raycast(MouseWorldPosition(newPosition), Vector3.down);
                if (hit && hit.transform.TryGetComponent(out DragItemObject item))
                {
                    currentItem = item;
                    currentItem.OnSelected();
                    initialPositionObject = currentItem.transform.localPosition;
                }
            }

            isDraggable = true;
        }
    }

    public void OnDrag(Vector3 position)
    {
        if(!isDraggable) return;
        if (!currentItem) return;
        transform.SetParent(transform.root);
        /*
        Vector3 pos = Camera.main.ScreenToWorldPoint(position);
        */
        if (isDraggable)
        {
            if (!smartDrag)
            {
                currentItem.transform.localPosition = (Vector2)MouseWorldPosition(Input.mousePosition);
            }
            else
            {
                currentItem.transform.localPosition =
                    initialPositionObject + (Vector2)MouseWorldPosition(Input.mousePosition) - initialPostionMouse;
            }
        }
        /*RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
        if (hit)
        {
            if (hit.transform.TryGetComponent(out DragItemObject item))
            {
                /*SwapItem(currentItem, item);#1#
            }
        }*/
    }

    public void SwapItem(DragItemObject item1, DragItemObject item2)
    {
        Transform tmpParent = item1.OriginParent;
        item1.OriginParent = item2.OriginParent;
        item2.OriginParent = tmpParent;
        item2.transform.SetParent(tmpParent);
        item2.transform.localPosition = Vector2.zero;
    }

    public void OnEndDrag(Vector3 position)
    {
        ActionEndDrag(position);
    }

    public void OnPointerUp(Vector3 position)
    {
        ActionEndDrag(position);
    }

    private void ActionEndDrag(Vector3 position)
    {
        isDraggable = true;
        smartDrag = true;
        initialPositionObject = Vector2.zero;
        canDrag = true;
        if (currentItem)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
            if (hit)
            {
                if (hit.transform.TryGetComponent(out DragItemObject item))
                {
                    SwapItem(currentItem, item);
                }
            }   
            currentItem.OnDrop();
            currentItem = null;
        }
    }
    private Vector3 MouseWorldPosition(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }
}