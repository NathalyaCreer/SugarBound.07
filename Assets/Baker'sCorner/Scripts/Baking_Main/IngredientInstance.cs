using UnityEngine;

public class IngredientInstance : MonoBehaviour
{
    public Ingredient ingredientData;

    void Start()
    {
        if (ingredientData != null)
            gameObject.name = ingredientData.ingredientName;
    }
}
