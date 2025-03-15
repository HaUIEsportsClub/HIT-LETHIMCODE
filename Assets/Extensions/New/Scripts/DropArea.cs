using UnityEngine;

public class DropArea : MonoBehaviour
{
    public DragDrop2D objDrop;

    public void SwapObjDrop(DragDrop2D newObj)
    {
        if (objDrop != null && objDrop.name != newObj.name)
        {
            objDrop = newObj;
        }
        else
        {
            objDrop = newObj;
        }
    }
}