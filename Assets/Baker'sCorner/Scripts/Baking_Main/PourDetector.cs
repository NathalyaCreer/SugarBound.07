using UnityEngine;

/// <summary>
/// Simple pour detector that uses a particle system and accumulates "units" while pouring
/// into a MixingBowl. When enough units are poured, it tells the bowl to add the Ingredient.
/// </summary>
public class PourDetector : MonoBehaviour
{
    [Header("Pour Settings")]
    public int pourAngleThreshold = 45;        // same logic as your previous script
    public float unitsPerSecond = 1f;          // how many "units" are added per second of pouring
    public float unitsRequiredToAdd = 1.5f;    // how many units until we add the ingredient once

    [Header("References")]
    public ParticleSystem pourParticles;       // particle to play while pouring
    public Ingredient pourIngredient;          // which Ingredient this pourer represents

    // runtime
    private bool isPouring = false;
    private float accumulatedUnits = 0f;
    private MixingBowl currentBowl = null;

    void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourAngleThreshold;

        if (!isPouring && pourCheck) StartPour();
        if (isPouring && !pourCheck) StopPour();

        if (isPouring && currentBowl != null && pourIngredient != null)
        {
            accumulatedUnits += unitsPerSecond * Time.deltaTime;

            if (accumulatedUnits >= unitsRequiredToAdd)
            {
                // Add ingredient to bowl once per threshold reached, then subtract threshold
                currentBowl.AddPouredIngredient(pourIngredient);
                accumulatedUnits -= unitsRequiredToAdd;
            }
        }
    }

    private void StartPour()
    {
        isPouring = true;
        if (pourParticles != null)
        {
            pourParticles.transform.parent = null; // keep particle world-positioned
            pourParticles.transform.position = transform.position; 
            pourParticles.Play();
        }
        // Debug
        Debug.Log($"{name}: Start pouring {pourIngredient?.ingredientName}");
    }

    private void StopPour()
    {
        isPouring = false;
        accumulatedUnits = 0f; // reset partial pour
        if (pourParticles != null)
            pourParticles.Stop();

        Debug.Log($"{name}: Stop pouring");
    }

    private float CalculatePourAngle()
    {
        // returns forward.y in degrees (like prior)
        return transform.forward.y * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MixingBowl>(out var bowl))
        {
            currentBowl = bowl;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<MixingBowl>(out var bowl) && bowl == currentBowl)
        {
            currentBowl = null;
            // Optionally stop pouring when leaving bowl
            if (isPouring) StopPour();
        }
    }
}

