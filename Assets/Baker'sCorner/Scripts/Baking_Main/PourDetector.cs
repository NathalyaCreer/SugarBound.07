using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public Ingredient ingredientData; // what ingredient this pourer gives
    public float unitsPerSecond = 1f;
    public float requiredUnits = 2f;

    private float accumulatedUnits = 0f;
    private bool isPouring = false;
    private MixingBowl currentBowl = null;

    public ParticleSystem pourParticles;

    void Update()
    {
        bool pourCheck = transform.forward.y * Mathf.Rad2Deg < 45;

        if (!isPouring && pourCheck)
            StartPour();
        else if (isPouring && !pourCheck)
            StopPour();

        if (isPouring && currentBowl != null)
        {
            accumulatedUnits += unitsPerSecond * Time.deltaTime;

            if (accumulatedUnits >= requiredUnits)
            {
                currentBowl.AddPouredIngredient(ingredientData);
                accumulatedUnits = 0f; // prevents duplicate adds
            }
        }
    }

    void StartPour()
    {
        isPouring = true;
        if (pourParticles != null) pourParticles.Play();
    }

    void StopPour()
    {
        isPouring = false;
        if (pourParticles != null) pourParticles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MixingBowl bowl))
            currentBowl = bowl;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MixingBowl bowl) && bowl == currentBowl)
            currentBowl = null;
    }
}
