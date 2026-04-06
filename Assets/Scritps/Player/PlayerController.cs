using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración")]
    float horizontal, vertical;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 7f;
    [SerializeField] private Rigidbody playerRigidbody;
    private Vector3 moveDirection;
    public bool isGrounded;

    private Animator animator;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    void Update()
    {
        HandleInput();

        if (moveDirection != Vector3.zero)
        {
            RotateTowardsMovement();
        }

        UpdateAnimations();

    }

    private void UpdateAnimations()
    {
        bool isWalking = moveDirection.magnitude > 0 && !Input.GetKey(KeyCode.LeftShift);
        bool isSprinting = moveDirection.magnitude > 0 && Input.GetKey(KeyCode.LeftShift);
        bool isJumping = !isGrounded;
        bool isIdle = moveDirection.magnitude == 0 && isGrounded;
        
        animator.SetBool("Walking", isWalking);
        animator.SetBool("Sprinting", isSprinting);
        animator.SetBool("Jumping", isJumping);
        animator.SetBool("Idle", isIdle);
    }

    void FixedUpdate()
    {
        ApplyPhysicsMovement();
    }
    private void HandleInput()
    {
        // Entrada básica de movimiento
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Salto si oprimimos Boton Jump (input manager) y está tocando el suelo.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            ApplyJump();
        }
    }
    private void ApplyPhysicsMovement()
    {
        // Movimiento con fuerzas físicas por medio de método MovePosition
        playerRigidbody.MovePosition(transform.localPosition + moveDirection * moveSpeed * Time.fixedDeltaTime);

    }
    private void ApplyJump()
    {
        //Usamos método AddForce en Rigidbodycpara aplicar una fuerza vertical con modo de Im
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //Esto nos comunica cuando colisiona con un objeto que tenga Tag "Floor"
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
    //Esto nos comunica cuando deja de colisionar con un objeto que tenga Tag "Floor"
    private void OnCollisionExit(Collision other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
    private void RotateTowardsMovement()
    {
        // Rotación suave hacia la dirección de movimiento
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}

