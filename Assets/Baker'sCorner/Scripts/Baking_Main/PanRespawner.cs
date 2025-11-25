using UnityEngine;

public class PanRespawner : MonoBehaviour
{
    public GameObject emptyPanPrefab;
    public Transform pantrySpawnPoint;

    public void RespawnEmptyPan()
    {
        Instantiate(emptyPanPrefab, pantrySpawnPoint.position, pantrySpawnPoint.rotation);
    }
}
