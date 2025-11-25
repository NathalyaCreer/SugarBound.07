using UnityEngine;

public class OvenTray : MonoBehaviour
{
    public Oven oven;                  // Reference to the Oven manager
    private GameObject currentPan;

    private void OnTriggerEnter(Collider other)
    {
        // Only accept objects tagged "Pan"
        if (!other.CompareTag("Pan")) return;
        if (currentPan != null) return;

        if (oven == null)
        {
            Debug.LogWarning("OvenTray: No Oven assigned!");
            return;
        }

        currentPan = other.gameObject;

        // Snap pan to tray
        currentPan.transform.SetParent(transform);
        currentPan.transform.localPosition = Vector3.zero;
        currentPan.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentPan.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Send pan to oven to handle baking
        oven.InsertPan(currentPan);

        // Debug
        BakingPan panScript = currentPan.GetComponent<BakingPan>();
        if (panScript != null && panScript.GetRecipeForPan() != null)
            Debug.Log($"OvenTray: {currentPan.name} snapped in place. Recipe: {panScript.GetRecipeForPan().recipeName}");
        else
            Debug.LogWarning("OvenTray: Inserted pan has no recipe assigned!");
    }

    public void RemovePan()
    {
        if (currentPan != null)
        {
            Destroy(currentPan);
            currentPan = null;
        }
    }
}
