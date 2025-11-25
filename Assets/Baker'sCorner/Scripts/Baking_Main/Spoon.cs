using UnityEngine;

public class Spoon : MonoBehaviour
{
    [Header("Spoon Settings")]
    public Transform scoopPoint;                  // assign tip of spoon
    public GameObject scoopVisualPrefab;          // small visual scoop (no rigidbody/collider ideally)

    private bool hasBatter = false;
    private Recipe heldRecipe;
    private GameObject scoopVisual;
    private Batter currentBowlBatter;             // reference to bowl batter for removing scoops

    private void OnTriggerEnter(Collider other)
    {
        // --- SCOOPING ---
        Batter batter = other.GetComponent<Batter>();
        if (batter != null && !hasBatter)
        {
            hasBatter = true;
            heldRecipe = batter.GetRecipeOutcome();
            currentBowlBatter = batter;

            // --- Spawn visual scoop ---
            if (scoopVisualPrefab != null && scoopPoint != null)
            {
                scoopVisual = Instantiate(scoopVisualPrefab, scoopPoint);
                scoopVisual.transform.localPosition = Vector3.zero;
                scoopVisual.transform.localRotation = Quaternion.identity;
                scoopVisual.transform.localScale = Vector3.one * 0.04f; // small and safe scale

                // Remove any rigidbody/collider from visual
                Rigidbody rb = scoopVisual.GetComponent<Rigidbody>();
                if (rb != null) Destroy(rb);
                Collider col = scoopVisual.GetComponent<Collider>();
                if (col != null) col.enabled = false;
            }

            batter.RemoveScoop(); // remove one scoop
            Debug.Log($"Spoon scooped batter: {heldRecipe.recipeName}");
            AudioManager.Instance.Play("Scoop");
            return;
        }

        // --- POURING INTO PAN ---
        BakingPan pan = other.GetComponent<BakingPan>();
        if (pan != null && hasBatter)
        {
            PourIntoPan(pan);
        }
    }

    private void PourIntoPan(BakingPan pan)
    {
        if (pan == null || heldRecipe == null) return;

        pan.ReceiveScoop(heldRecipe); // Pass in the recipe
        hasBatter = false;
        heldRecipe = null;

        if (scoopVisual != null)
            Destroy(scoopVisual);

        Debug.Log("🥣 Poured scoop into pan!");
        AudioManager.Instance.Play("Pour");
    }

    // Optional helpers
    public bool HasBatter() => hasBatter;
    public Recipe GetHeldRecipe() => heldRecipe;
}
