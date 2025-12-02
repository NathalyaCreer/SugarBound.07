using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    [Header("Recipes")]
    public List<Recipe> allRecipes;

    [Header("Prefabs")]
    public GameObject batterPrefab;
    public GameObject failedMixPrefab;

    [Header("VFX")]
    public GameObject ingredientVFXPrefab;
    public GameObject successMixVFXPrefab;
    public GameObject failedMixVFXPrefab;

    [Header("Settings")]
    public float spawnOffsetY = 1f;
    public float mixCooldownTime = 1f;

    [Header("Current Ingredients")]
    public List<Ingredient> currentIngredients = new List<Ingredient>();

    private bool isMixing = false; // prevents re-entry

    private void OnTriggerEnter(Collider other)
    {
        IngredientInstance ingredientInstance = other.GetComponent<IngredientInstance>();
        if (ingredientInstance == null) return;

        if (!other.CompareTag("Ingredient")) return;

        AddIngredient(ingredientInstance.ingredientData);
        HandleIngredientVFXAndAudio(other.transform.position);

        Destroy(other.gameObject);

        // Try to find a matching recipe only if not already mixing
        if (!isMixing)
        {
            Recipe matchedRecipe = FindExactMatch();
            StartCoroutine(MixRoutine(matchedRecipe));
        }
    }

    private System.Collections.IEnumerator MixRoutine(Recipe matchedRecipe)
    {
        isMixing = true;

        yield return null; // allow physics to settle

        if (matchedRecipe != null)
        {
            SpawnBatter(matchedRecipe);
            currentIngredients.Clear(); // clear only after successful mix
        }
        else if (!CanStillMatchAnyRecipe())
        {
            SpawnFailedMix();
            currentIngredients.Clear(); // clear only after failure
        }

        yield return new WaitForSeconds(mixCooldownTime);
        isMixing = false;
    }

    private void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null) return;
        currentIngredients.Add(ingredient);
        Debug.Log($"Added {ingredient.ingredientName} to the bowl! Current ingredients count: {currentIngredients.Count}");
    }

    private void HandleIngredientVFXAndAudio(Vector3 position)
    {
        if (ingredientVFXPrefab != null)
            Instantiate(ingredientVFXPrefab, position, Quaternion.identity);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("AddIngredient");
    }

    private void SpawnBatter(Recipe matchedRecipe)
    {
        Vector3 spawnPos = transform.position + Vector3.up * spawnOffsetY;
        GameObject batter = Instantiate(batterPrefab, spawnPos, Quaternion.identity);

        if (successMixVFXPrefab != null)
            Instantiate(successMixVFXPrefab, spawnPos, Quaternion.identity);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("CorrectMix");

        Batter batterScript = batter.GetComponent<Batter>();
        if (batterScript != null)
            batterScript.SetRecipeOutcome(matchedRecipe);

        Debug.Log($" Spawned batter for recipe: {matchedRecipe.recipeName}");
    }

    private void SpawnFailedMix()
    {
        Vector3 spawnPos = transform.position + Vector3.up * spawnOffsetY;
        Instantiate(failedMixPrefab, spawnPos, Quaternion.identity);

        if (failedMixVFXPrefab != null)
            Instantiate(failedMixVFXPrefab, spawnPos, Quaternion.identity);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("FailedMix");

        Debug.Log("Failed mix spawned (Gunk).");
    }

    private Recipe FindExactMatch()
    {
        foreach (Recipe recipe in allRecipes)
        {
            if (recipe.requiredIngredients.Count != currentIngredients.Count)
                continue;

            bool allMatch = true;
            foreach (Ingredient ingredient in recipe.requiredIngredients)
            {
                if (!currentIngredients.Contains(ingredient))
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
                return recipe;
        }
        return null;
    }

    private bool CanStillMatchAnyRecipe()
    {
        foreach (Recipe recipe in allRecipes)
        {
            // Skip if already exceeded ingredient count
            if (currentIngredients.Count > recipe.requiredIngredients.Count)
                continue;

            bool possible = true;
            foreach (Ingredient ing in currentIngredients)
            {
                if (!recipe.requiredIngredients.Contains(ing))
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
                return true;
        }

        return false;
    }

    // -----------------------
    // New: Accept poured ingredient (called by PourDetector)
    // -----------------------
    public void AddPouredIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            Debug.LogWarning("MixingBowl: Tried to add null poured ingredient.");
            return;
        }

        // Prevent duplicates (you said poured ingredient should count once)
        if (currentIngredients.Contains(ingredient))
        {
            Debug.Log($"MixingBowl: Poured ingredient {ingredient.ingredientName} already present.");
            return;
        }

        currentIngredients.Add(ingredient);
        Debug.Log($"MixingBowl: Poured ingredient added: {ingredient.ingredientName}");

        // Play same VFX/audio as when adding a normal ingredient
        HandleIngredientVFXAndAudio(transform.position);

        // Immediately try to match recipes (mimics OnTriggerEnter behavior)
        if (!isMixing)
        {
            Recipe matchedRecipe = FindExactMatch();
            StartCoroutine(MixRoutine(matchedRecipe));
        }
    }
}

