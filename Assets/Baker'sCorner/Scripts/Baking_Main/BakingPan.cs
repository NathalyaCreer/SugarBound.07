using UnityEngine;

public class BakingPan : MonoBehaviour
{
    [Header("Pan Settings")]
    public int totalScoopsNeeded = 6;           // How many scoops to fill the pan
    public GameObject finalPanPrefab;           // Prefab to spawn when full (with recipe)

    [Header("VFX / SFX")]
    public GameObject scoopVFXPrefab;

    private int scoopsReceived = 0;
    private Recipe assignedRecipe;

    // Called by Spoon when pouring a scoop
    public void ReceiveScoop(Recipe recipe)
    {
        if (assignedRecipe == null)
            assignedRecipe = recipe;

        scoopsReceived++;

        // Play scoop VFX
        if (scoopVFXPrefab != null)
            Instantiate(scoopVFXPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        // Check if pan is full
        if (scoopsReceived >= totalScoopsNeeded)
        {
            SpawnFinalPan();
        }
    }

    private void SpawnFinalPan()
    {
        if (finalPanPrefab == null)
        {
            Debug.LogWarning("BakingPan: No finalPanPrefab assigned!");
            return;
        }

        // Spawn final pan at current position/rotation
        GameObject finalPan = Instantiate(finalPanPrefab, transform.position, transform.rotation);

        // Assign recipe to final pan
        BakingPan finalPanScript = finalPan.GetComponent<BakingPan>();
        if (finalPanScript != null)
        {
            finalPanScript.assignedRecipe = assignedRecipe;
            finalPanScript.scoopsReceived = totalScoopsNeeded; // mark as full
        }

        Debug.Log($"BakingPan: Final pan created with recipe: {assignedRecipe.recipeName}");

        // Destroy this temporary pan
        Destroy(gameObject);
    }

    // Oven reads this
    public Recipe GetRecipeForPan()
    {
        return assignedRecipe;
    }
}
