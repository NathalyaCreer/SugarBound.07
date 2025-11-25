using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    [Header("Input Action Properties")]
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;

    [Header("Hand Animator")]
    public Animator handAnimator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();
        float grip = gripValue.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", trigger);
        handAnimator.SetFloat("Grip", grip);

        //point logic//
        float point = 0f;
        if(grip > 0.3f && trigger < 0.1f)
        {
            point = 1f;
        }
        
        handAnimator.SetFloat("Point", point);
    }
}
