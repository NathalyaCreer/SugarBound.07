using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllOMatic : MonoBehaviour
{
    [Header("Machine State")]
    public bool isOpen = false;
    public bool isRunning = false;

    [Header("References")]
    public Transform trayTransform;
    public Transform outputPoint;
    public Light powerLight;
    public ParticleSystem activationVFX;
    public AudioSource humSource;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip successClip;
    public AudioClip errorClip;

    [Header("Processes/Recipes")]
    public List<IngredientProcess> allProcesses = new List<IngredientProcess>();

    private GameObject currentItem;           // The GameObject in the tray
    private IngredientInstance currentInstance; // IngredientInstance component
    private IngredientProcess pendingRecipe;

    private string enteredCode = "";

    void Start()
    {
        if (powerLight) powerLight.enabled = false;
        if (humSource) humSource.Stop();
    }

    // ============================================
    //  TRAY + ITEM HANDLING
    // ============================================

    public void InsertItem(GameObject newItem)
    {
        if (isRunning || currentItem != null)
            return;

        currentItem = newItem;
        currentInstance = currentItem.GetComponent<IngredientInstance>();
        if (currentInstance == null)
        {
            Debug.LogError("AllOMatic: Item missing IngredientInstance!");
            PlayError();
            currentItem = null;
            return;
        }

        // Position item on tray
        currentItem.transform.position = trayTransform.position;
        currentItem.transform.rotation = trayTransform.rotation;
        currentItem.transform.SetParent(trayTransform);

        // Disable Rigidbody so it stays in place
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Debug.Log($"AllOMatic: Inserted {currentInstance.ingredientData.ingredientName} into tray.");

        // Play insert sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play("InsertItem");
    }

    // Called by ButtonAction for code entry
    public void PressNumber(string num)
    {
        enteredCode += num;
        Debug.Log("AllOMatic: Entered code " + enteredCode);
    }

    public void PressMelt() => StartAllOMatic(ProcessType.Melt);
    public void PressPop() => StartAllOMatic(ProcessType.Pop);
    public void PressStart() => StartAllOMatic(ProcessType.Decorate);

    public void OnCodeEntered(string code)
    {
        enteredCode = code;
        Debug.Log("AllOMatic: Code entered = " + enteredCode);
    }

    // ============================================
    //  START PROCESSING
    // ============================================

    public void StartAllOMatic(ProcessType requestedType)
    {
        if (isRunning)
            return;

        if (currentInstance == null)
        {
            Debug.LogWarning("AllOMatic: No item in tray to process.");
            PlayError();
            return;
        }

        Ingredient ingredient = currentInstance.ingredientData;
        IngredientProcess recipe = FindRecipe(ingredient, requestedType);

        if (recipe == null)
        {
            Debug.LogWarning($"AllOMatic: {ingredient.ingredientName} cannot be {requestedType}ed!");
            PlayError();
            return; // Item stays in tray
        }

        // Check decoration code if decorating
        if (requestedType == ProcessType.Decorate && enteredCode != recipe.decorationCode)
        {
            Debug.LogWarning("AllOMatic: Incorrect decoration code!");
            PlayError();
            return; // Item stays in tray
        }

        pendingRecipe = recipe;
        StartMachineEffects();
        StartCoroutine(ProcessRoutine());
    }

    // ============================================
    //  RECIPE LOOKUP
    // ============================================

    private IngredientProcess FindRecipe(Ingredient ingredient, ProcessType type)
    {
        foreach (var recipe in allProcesses)
        {
            if (recipe.inputIngredient == ingredient && recipe.processType == type)
                return recipe;
        }
        return null;
    }

    // ============================================
    //  MACHINE EFFECTS
    // ============================================

    private void StartMachineEffects()
    {
        isRunning = true;

        if (powerLight) powerLight.enabled = true;
        if (activationVFX) activationVFX.Play();
        if (humSource) humSource.Play();
    }

    private void StopMachineEffects()
    {
        isRunning = false;

        if (powerLight) powerLight.enabled = false;
        if (humSource) humSource.Stop();
    }

    // ============================================
    //  PROCESS ROUTINE
    // ============================================

    private IEnumerator ProcessRoutine()
    {
        Debug.Log($"AllOMatic: Processing {pendingRecipe.processType} for {currentInstance.ingredientData.ingredientName}...");

        yield return new WaitForSeconds(pendingRecipe.processTime);

        // Spawn output if prefab exists
        if (pendingRecipe.outputPrefab != null)
        {
            Instantiate(pendingRecipe.outputPrefab, outputPoint.position, outputPoint.rotation);
            PlaySuccess();

            // Optional FX/SFX
            if (pendingRecipe.processFX != null)
                Instantiate(pendingRecipe.processFX, outputPoint.position, Quaternion.identity);
            if (pendingRecipe.processSFX != null)
                sfxSource.PlayOneShot(pendingRecipe.processSFX);

            // Remove original item
            if (currentItem != null)
                Destroy(currentItem);
            currentItem = null;
            currentInstance = null;
        }
        else
        {
            // Failed process → keep item in tray
            Debug.LogWarning("AllOMatic: Process completed but output prefab missing. Item remains in tray.");
            PlayError();
        }

        StopMachineEffects();
    }

    // ============================================
    //  AUDIO UTILITIES
    // ============================================

    public void PlaySuccess()
    {
        if (sfxSource && successClip)
            sfxSource.PlayOneShot(successClip);
    }

    public void PlayError()
    {
        if (sfxSource && errorClip)
            sfxSource.PlayOneShot(errorClip);
    }

}
