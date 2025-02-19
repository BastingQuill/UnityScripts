using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (animator == null)
            Debug.LogError(" No se encontró el Animator en " + gameObject.name);
        if (characterController == null)
            Debug.LogError(" No se encontró el CharacterController en " + gameObject.name);
    }

    private void Update()
    {
        float speed = characterController.velocity.magnitude;
        bool isMoving = speed > 0.1f;

        Debug.Log(" Velocidad: " + speed + " - isWalking: " + isMoving);

        animator.SetBool("isWalking", isMoving);
    }
}
