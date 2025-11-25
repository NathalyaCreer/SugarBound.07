using System.Collections;
using UnityEngine;

public class CandyDemonAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;                     // Player to look at
    public GameManager gameManager;              // GameManager reference

    [Header("Teleport Settings")]
    public Transform[] teleportPoints;           // All points demon can teleport to
    public float minWait = 2f;                   // Min time before next teleport
    public float maxWait = 5f;                   // Max time before next teleport
    public float appearChance = 0.3f;            // Chance to appear visibly

    [Header("Jump Scare")]
    public GameObject jumpScarePrefab;           // Prefab for jump scare
    public float failThreshold = 3;              // Number of fails before jump scare

    [Header("Audio")]
    public AudioClip subtleSound;                // Ambient subtle sound
    public AudioClip appearSound;                // Sound when demon appears
    public AudioSource audioSource;              // Audio source for sounds

    private bool jumpScareTriggered = false;
    private Renderer demonRenderer;

    private void Start()
    {
        demonRenderer = GetComponentInChildren<Renderer>();
        if (demonRenderer != null)
            demonRenderer.enabled = false; // Start invisible

        if (gameManager == null)
            gameManager = GameManager.Instance;

        StartCoroutine(TeleportRoutine());
    }

    // Called by GameManager when player fails a recipe
    public void OnPlayerFail(int totalFails)
    {
        Debug.Log($"CandyDemonAI: Player fail count = {totalFails}");

        if (totalFails >= failThreshold)
        {
            TriggerJumpScare();
        }
    }

    private IEnumerator TeleportRoutine()
    {
        while (!jumpScareTriggered)
        {
            if (teleportPoints.Length == 0)
                yield break;

            // Pick random point
            Transform target = teleportPoints[Random.Range(0, teleportPoints.Length)];
            transform.position = target.position;

            // Determine if demon appears visibly
            bool willAppear = Random.value < appearChance;

            if (demonRenderer != null)
                demonRenderer.enabled = willAppear;

            // Play subtle or appear sound
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = willAppear ? appearSound : subtleSound;
                audioSource.Play();
            }

            if (willAppear && player != null)
                transform.LookAt(player);

            Debug.Log($"CandyDemonAI: Teleported to {target.name} | Visible: {willAppear}");

            float waitTime = Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void TriggerJumpScare()
    {
        if (jumpScareTriggered) return;

        jumpScareTriggered = true;
        Debug.Log("CandyDemonAI: Jump Scare Triggered!");

        if (jumpScarePrefab != null && player != null)
        {
            Instantiate(jumpScarePrefab, player.position + player.forward * 0.5f, Quaternion.identity);
        }

        // Optional: Freeze player or play additional sound here
    }
}
