using UnityEngine;

public enum ProcessType
{
    Melt,
    Pop,
    Decorate
}

[System.Serializable]
public class IngredientProcess
{
    [Header("Input Ingredient")]
    public Ingredient inputIngredient;

    [Header("Process Settings")]
    public ProcessType processType;

    [Tooltip("How long the machine runs for this recipe.")]
    public float processTime = 1f;   // ← This is the missing field!

    [Header("Output")]
    public GameObject outputPrefab;

    [Header("Optional Effects")]
    public ParticleSystem processFX;
    public AudioClip processSFX;

    [Header("Decoration Only")]
    public string decorationCode;
}
