using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int stepIndex; // Which step this trigger activates

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (TutorialManager.Instance.GetCurrentStep() == stepIndex - 1)
        {
            TutorialManager.Instance.StepComplete();
        }
    }
}
