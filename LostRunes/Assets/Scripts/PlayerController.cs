using UnityEngine;

using UnityEngine;

using UnityEngine;

using UnityEngine;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;             // Velocidad del personaje
    public GameObject arrowPrefab;          // Prefab de la flecha
    public Transform bowPivot;              // Pivote del arco
    public Animator bowAnimator;            // Animator del arco
    public ArrowPool arrowPool;             // Pool de flechas

    private Vector2 moveInput;              // Entrada de movimiento
    private Animator anim;                  // Animator del jugador
    private bool invert = false;            // Control para invertir el arco
    private Rigidbody2D rb;                 // Referencia al Rigidbody2D
    private ICommand fireCommand;           // Comando para disparar

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D
    }

    void Update()
    {
        // Manejar movimiento
        HandleMovement();

        // Actualizar dirección del arco (ratón)
        UpdateBowDirection();

        // Disparar flecha al hacer clic
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo
        {
            FireArrow();
        }
    }

    void HandleMovement()
    {
        // Entrada de movimiento
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;     // Arriba
        if (Input.GetKey(KeyCode.S)) moveY = -1f;    // Abajo
        if (Input.GetKey(KeyCode.A)) moveX = -1f;    // Izquierda
        if (Input.GetKey(KeyCode.D)) moveX = 1f;     // Derecha

        moveInput = new Vector2(moveX, moveY).normalized;

        // Ejecutar el comando de movimiento
        ICommand moveCommand = new MoveCommand(this, moveInput);
        moveCommand.Execute();

        // Actualizar animaciones
        UpdateAnimations(moveInput);
    }

    public void Move(Vector2 direction)
    {
        // Usar Rigidbody2D para moverse y manejar colisiones
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void UpdateAnimations(Vector2 movement)
    {
        // Establecer si el personaje está corriendo
        bool isRunning = movement.magnitude > 0;
        anim.SetBool("isRunning", isRunning);

        // Ajustar la dirección de la escala para girar el personaje
        if (movement.x > 0)
        {
            invert = false; // Mirando hacia la derecha
            transform.localScale = new Vector3(3, 3, 1);
        }
        else if (movement.x < 0)
        {
            invert = true; // Mirando hacia la izquierda
            transform.localScale = new Vector3(-3, 3, 1);
        }
    }

    void UpdateBowDirection()
    {
        // Obtener posición del ratón
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ignorar el eje Z

        // Dirección hacia el ratón
        Vector2 direction = (mousePosition - bowPivot.position).normalized;

        if (invert)
        {
            direction = -direction;
        }

        // Calcular el ángulo de rotación necesario
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar rotación al pivote del arco
        bowPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireArrow()
    {
        // Obtener la dirección hacia el ratón
        Vector3 direction = bowPivot.right;
        if (invert)
        {
            direction = -direction;
        }

        // Crear comando de disparo
        fireCommand = new FireCommand(bowPivot, arrowPool, direction, bowAnimator);
        fireCommand.Execute();
    }
}