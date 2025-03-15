using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Extensions.New.Scripts
{
    public class DragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
    {
        public static DragManager Instance;
        private DragItem currentItem;

        private void Awake()
        {
            Instance = this;
        }

        private Vector3 MouseWorldPosition(Vector3 position)
        {
            return Camera.main.ScreenToWorldPoint(position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RaycastHit2D hit = Physics2D.Raycast(MouseWorldPosition(eventData.position), Vector2.down);
            if (hit.transform.TryGetComponent(out DragItem item))
            {
                currentItem = item;
                currentItem.OnSelected();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!currentItem) return;
            transform.SetParent(transform.root);
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            currentItem.OnDrag(pos);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down);
            if (hit)
            {
                if (hit.transform.TryGetComponent(out DragItem item))
                {
                    SwapItem(currentItem, item);
                }
            }
        }

        public void SwapItem(DragItem item1, DragItem item2)
        {
            Transform tmpParent = item1.OriginParent;
            item1.OriginParent = item2.OriginParent;
            item2.OriginParent = tmpParent;
            item2.transform.SetParent(tmpParent);
            ActionEndDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ActionEndDrag();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ActionEndDrag();
        }

        private void ActionEndDrag()
        {
            if (currentItem)
            {
                currentItem.OnDrop();
                currentItem = null;
            }
        }
    }
}