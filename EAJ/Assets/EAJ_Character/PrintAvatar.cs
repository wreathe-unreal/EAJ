using UnityEngine;
using TMPro;

public class DisplayAnimatorControllerName : MonoBehaviour
{
    public TextMeshProUGUI controllerNameText; // Assign this in the inspector

    private Animator animator;

    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
            return;
        }

        // Get the runtime animator controller
        RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
        if (animatorController == null)
        {
            Debug.LogError("Animator does not have a runtime animator controller assigned.");
            return;
        }

        // Get the name of the animator controller
        string controllerName = animatorController.name;

        // Set the TextMeshProUGUI text to the controller name
        controllerNameText.text = controllerName;
    }
}