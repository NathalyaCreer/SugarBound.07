using UnityEngine;

public class AllOMaticTray : MonoBehaviour
{
    public AllOMatic allOMatic;
    private GameObject currentItem;

    private void OnTriggerEnter(Collider other)
    {

        IngredientInstance instance = other.GetComponent<IngredientInstance>();
        if (instance == null) return;
        if (currentItem != null) return;

        if (allOMatic == null)
        {
            Debug.LogWarning("AllOMaticTray: No AllOMatic assigned!");
            return;
        }

        currentItem = other.gameObject;
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        allOMatic.InsertItem(currentItem);

        Debug.Log($"Inserted {instance.ingredientData.ingredientName} into tray.");
        
    }
    
    public void RemoveItem()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }
    
}
