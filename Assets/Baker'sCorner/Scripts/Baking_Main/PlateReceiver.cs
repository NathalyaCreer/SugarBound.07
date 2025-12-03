using UnityEngine;

public class PlateReceiver : MonoBehaviour
{
    public Plate plate;

    private void OnTriggerEnter(Collider other)
    {
        CookedFood cooked = other.GetComponent<CookedFood>();

        if (cooked != null)
        {
            plate.PlaceFood(cooked.gameObject);
        }
    }
}
