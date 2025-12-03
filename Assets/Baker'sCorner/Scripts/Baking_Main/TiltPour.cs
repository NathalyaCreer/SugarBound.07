using UnityEngine;

public class TiltPour : OnTilt
{
    [Header("Pour Particle Effect")]
    public ParticleSystem pourParticles;

    [Header("Ingredient Data")]
    public Ingredient ingredientData;   // what ingredient this bottle pours

    private bool isPouring = false;

    private void Start()
    {
        // Hook into the tilt events
        OnBegin.AddListener(OnPourStart);
        OnEnd.AddListener(OnPourStop);
    }

    private void OnPourStart(MonoBehaviour obj)
    {
        if (isPouring) return;

        isPouring = true;

        if (pourParticles != null)
            pourParticles.Play();
    }

    private void OnPourStop(MonoBehaviour obj)
    {
        if (!isPouring) return;

        isPouring = false;

        if (pourParticles != null)
            pourParticles.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPouring) return;

        MixingBowl bowl = other.GetComponent<MixingBowl>();
        if (bowl == null) return;

        // Add ingredient automatically while the particles are pouring
        bowl.AddIngredient(ingredientData);
    }
}
