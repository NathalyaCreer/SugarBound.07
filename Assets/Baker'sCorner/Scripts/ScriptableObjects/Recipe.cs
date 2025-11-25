using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;

    [Header("Ingredients")]
    public List<Ingredient> requiredIngredients;

    [Header("Prefabs & Bake Info")]
    public GameObject finalPanPrefab;   // The specific pan prefab for this recipe
    public GameObject resultPrefab;     // Final baked item
    public GameObject burntPrefab;      // Burnt item
    public float bakeTime = 30f;        // Recommended bake time in seconds
    public Sprite recipeImage;
}



