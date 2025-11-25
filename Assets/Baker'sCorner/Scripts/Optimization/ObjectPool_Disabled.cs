using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj, float delay = 2f)
    {
        StartCoroutine(ReturnAfterDelay(obj, delay));
    }

    private System.Collections.IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
