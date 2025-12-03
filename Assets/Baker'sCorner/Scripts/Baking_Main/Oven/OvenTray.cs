using UnityEngine;

public class OvenTray : MonoBehaviour
{
    public Oven oven;
    private GameObject currentPan;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Pan")) return;
        if (currentPan != null) return;

        if (oven == null)
        {
            Debug.LogWarning("OvenTray: No Oven assigned!");
            return;
        }

        currentPan = other.gameObject;

        // Snap position
        currentPan.transform.SetParent(transform);
        currentPan.transform.localPosition = Vector3.zero;
        currentPan.transform.localRotation = Quaternion.identity;

        // Freeze physics
        Rigidbody rb = currentPan.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Insert into oven AND immediately start baking
        oven.InsertPan(currentPan);
        oven.StartBaking();  // <-- automatically begin

        Debug.Log("OvenTray: Pan inserted and auto-baking started.");
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
