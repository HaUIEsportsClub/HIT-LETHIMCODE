using System.Collections;
using System.Collections.Generic;
using pooling;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> levels;
    private Transform currentLevel;

    public void SpawmLevel(int index)
    {
        if(index > 8) return;
        if (currentLevel != null)
        {
            PoolingManager.Despawn(currentLevel.gameObject);
            currentLevel = null;
        }
        currentLevel = PoolingManager.Spawn(levels[index], transform.position, Quaternion.identity, transform);
    }
}
