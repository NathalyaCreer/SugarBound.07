using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    [Header("References")]
    public Transform outputPoint;               // Where baked/burnt items spawn
    public Light powerLight;                    // optional power indicator
    public AudioSource sfxSource;               // optional audio source
    public AudioClip ovenStartSFX;
    public AudioClip ovenDingSFX;

    [Header("Baking Settings")]
    public float overbakeMultiplier = 1.2f;    // overbake threshold

    private GameObject currentPan;
    private BakingPan currentPanScript;
    private bool isBaking = false;
    private float bakeTimeSet = 0f;

    // Called by OvenTray when a pan is snapped
    public void InsertPan(GameObject pan)
    {
        if (isBaking || currentPan != null)
        {
            Debug.LogWarning("Oven: Already has a pan in place!");
            return;
        }

        currentPan = pan;
        currentPanScript = pan.GetComponent<BakingPan>();

        if (currentPanScript == null)
        {
            Debug.LogWarning("OvenTray: Inserted object is not a BakingPan!");
            currentPan = null;
            return;
        }

        Debug.Log($"Oven: {pan.name} inserted. Recipe: {currentPanScript.GetRecipeForPan()?.recipeName}");
    }

    // Called by buttons
    public void SetBakeTime(float seconds)
    {
        bakeTimeSet = seconds;
        Debug.Log($"Oven: Bake time set to {bakeTimeSet} seconds");
    }

    public void StartBaking()
    {
        if (currentPan == null)
        {
            Debug.LogWarning("Oven: No pan to bake!");
            return;
        }

        if (isBaking)
        {
            Debug.LogWarning("Oven: Already baking!");
            return;
        }

        if (bakeTimeSet <= 0f)
        {
            Debug.LogWarning("Oven: Bake time not set!");
            return;
        }

        StartCoroutine(BakeRoutine());
    }

    private IEnumerator BakeRoutine()
    {
        isBaking = true;

        // Start FX/SFX
        if (ovenStartSFX != null && sfxSource != null)
            sfxSource.PlayOneShot(ovenStartSFX);
        if (powerLight != null) powerLight.enabled = true;

        Debug.Log($"Oven: Baking {currentPan.name} for {bakeTimeSet} seconds...");

        yield return new WaitForSeconds(bakeTimeSet);

        // Determine result
        Recipe recipe = currentPanScript.GetRecipeForPan();
        if (recipe == null)
        {
            Debug.LogWarning("Oven: Pan has no recipe!");
            isBaking = false;
            yield break;
        }

        bool isBurnt = bakeTimeSet > recipe.bakeTime * overbakeMultiplier;
        GameObject prefabToSpawn = isBurnt ? recipe.burntPrefab : recipe.resultPrefab;

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, outputPoint.position, outputPoint.rotation);
            Debug.Log($"Oven: Spawned {(isBurnt ? "Burnt" : "Baked")} {recipe.recipeName}!");
        }
        else
        {
            Debug.LogWarning("Oven: Result prefab is missing!");
        }

        // Delete old pan
        Destroy(currentPan);
        currentPan = null;
        currentPanScript = null;
        bakeTimeSet = 0f;

        // Ding SFX
        if (ovenDingSFX != null && sfxSource != null)
            sfxSource.PlayOneShot(ovenDingSFX);

        if (powerLight != null) powerLight.enabled = false;
        isBaking = false;
    }
}
