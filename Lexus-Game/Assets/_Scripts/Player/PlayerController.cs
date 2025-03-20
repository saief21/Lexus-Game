using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Regard")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;
    
    [Header("Références")]
    [SerializeField] private Transform cameraHolder;
    
    private CharacterController characterController;
    private float verticalRotation = 0f;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float currentMoveSpeed;
    private float gravity = -9.81f;
    private Vector3 velocityY;
    private bool isGrounded;
    private bool isSprinting;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentMoveSpeed = moveSpeed;
        
        // Verrouiller et cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            velocityY.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
        currentMoveSpeed = isSprinting ? sprintSpeed : moveSpeed;
    }

    private void HandleMovement()
    {
        // Calculer la direction du mouvement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(move * currentMoveSpeed * Time.deltaTime);

        // Appliquer la gravité
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocityY.y < 0)
        {
            velocityY.y = -2f;
        }
        velocityY.y += gravity * Time.deltaTime;
        characterController.Move(velocityY * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        // Rotation horizontale (gauche/droite)
        float mouseX = lookInput.x * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotation verticale (haut/bas)
        float mouseY = lookInput.y * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleJump()
    {
        // La logique de saut est maintenant gérée dans OnJump
    }
}
