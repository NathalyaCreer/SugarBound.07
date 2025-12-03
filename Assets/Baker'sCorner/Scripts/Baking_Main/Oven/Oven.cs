using System.Collections;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [Header("References")]
    public Transform outputPoint;
    public Light powerLight;
    public AudioSource sfxSource;
    public AudioClip ovenStartSFX;
    public AudioClip ovenDingSFX;

    [Header("Bake Settings")]
    public float bakeDuration = 5f; // universal bake time

    private GameObject currentPan;
    private BakingPan currentPanScript;
    private bool isBaking = false;

    // Called by OvenTray
    public void InsertPan(GameObject pan)
    {
        if (isBaking || currentPan != null)
        {
            Debug.LogWarning("Oven already has a pan!");
            return;
        }

        currentPan = pan;
        currentPanScript = pan.GetComponent<BakingPan>();

        if (currentPanScript == null)
        {
            Debug.LogWarning("Inserted object is not a BakingPan!");
            currentPan = null;
            return;
        }

        Debug.Log("Oven: Pan inserted. Recipe: " +
            currentPanScript.GetRecipeForPan()?.recipeName);
    }

    // Called by ONE start button
    public void StartBaking()
    {
        if (currentPan == null)
        {
            Debug.LogWarning("Oven: No pan inserted!");
            return;
        }

        if (isBaking)
        {
            Debug.LogWarning("Oven already baking.");
            return;
        }

        StartCoroutine(BakeRoutine());
    }

    private IEnumerator BakeRoutine()
    {
        isBaking = true;

        // SFX + Light
        if (sfxSource && ovenStartSFX) sfxSource.PlayOneShot(ovenStartSFX);
        if (powerLight) powerLight.enabled = true;

        Recipe recipe = currentPanScript.GetRecipeForPan();
        if (recipe == null)
        {
            Debug.LogWarning("Oven: Pan has no recipe assigned!");
            isBaking = false;
            yield break;
        }

        Debug.Log($"Oven: Baking {recipe.recipeName}...");

        // Wait for the global bake time
        yield return new WaitForSeconds(bakeDuration);

        // Spawn cooked result
        if (recipe.resultPrefab != null)
        {
            Instantiate(
                recipe.resultPrefab,
                outputPoint.position,
                outputPoint.rotation
            );
        }
        else
        {
            Debug.LogWarning("Recipe has no result prefab!");
        }

        // Delete the pan
        Destroy(currentPan);
        currentPan = null;
        currentPanScript = null;

        // Ding
        if (sfxSource && ovenDingSFX) sfxSource.PlayOneShot(ovenDingSFX);
        if (powerLight) powerLight.enabled = false;

        isBaking = false;
    }
}
