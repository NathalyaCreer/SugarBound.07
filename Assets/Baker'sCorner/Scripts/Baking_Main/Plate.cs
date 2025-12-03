using UnityEngine;

public class Plate : MonoBehaviour
{
    public Transform placementPoint;       // where the baked item sits
    public GameObject decoratedVersion;    // what AllOMatic needs

    private GameObject currentFood;

    public void PlaceFood(GameObject bakedFoodPrefab)
    {
        if (currentFood != null)
            Destroy(currentFood);

        currentFood = Instantiate(bakedFoodPrefab, placementPoint.position, placementPoint.rotation);
    }

    public GameObject GetFood()
    {
        return currentFood;
    }
}
