using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RecipeBookUI : MonoBehaviour
{
    [Header("Recipe List")]
    public List<Recipe> recipes = new List<Recipe>();
    private int currentIndex = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI ingredientsText;
    public Image recipeImage;

    void Start()
    {
        if (recipes.Count == 0)
        {
            Debug.LogWarning("No recipes assigned to RecipeBookUI!");
            return;
        }

        UpdateUI();
    }

    public void NextRecipe()
    {
        if (recipes.Count == 0) return;
        currentIndex = (currentIndex + 1) % recipes.Count; // wrap around
        UpdateUI();
    }

    public void PreviousRecipe()
    {
        if (recipes.Count == 0) return;
        currentIndex = (currentIndex - 1 + recipes.Count) % recipes.Count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Recipe recipe = recipes[currentIndex];
        recipeNameText.text = recipe.recipeName;

        ingredientsText.text = "";
        foreach (var ingredient in recipe.requiredIngredients)
        {
            if (ingredient != null)
                ingredientsText.text += "- " + ingredient.ingredientName + "\n";
        }

        //recipe image
        if (recipe.recipeImage != null)
        {
            recipeImage.sprite = recipe.recipeImage;
            recipeImage.enabled = true;
        }
        else
        {
            recipeImage.enabled = false; //hides if not there
        }
    }
}

