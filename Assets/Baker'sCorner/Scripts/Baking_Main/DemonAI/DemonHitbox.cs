using UnityEngine;

public class DemonHitbox : MonoBehaviour
{
    private CandyDemonAI demonAI;

    private void Awake()
    {
        demonAI = GetComponentInParent<CandyDemonAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If projectile or throwable hits
        if (other.CompareTag("Projectile"))
        {
            demonAI.OnHit();
        }
    }
}

