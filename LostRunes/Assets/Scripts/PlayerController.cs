using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;             // Velocidad del personaje
    public GameObject arrowPrefab;          // Prefab de la flecha
    public Transform bowPivot;              // Empty GameObject como pivote para el arco
    public Animator bowAnimator;            // Animator del arco

    private Vector2 moveInput;
    private Animator anim;
    private bool invert = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento del jugador
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;     // Arriba
        if (Input.GetKey(KeyCode.S)) moveY = -1f;    // Abajo
        if (Input.GetKey(KeyCode.A)) moveX = -1f;    // Izquierda
        if (Input.GetKey(KeyCode.D)) moveX = 1f;     // Derecha

        moveInput = new Vector2(moveX, moveY).normalized;

        // Mover al jugador
        transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        // Actualizar la dirección del arco (según la posición del ratón)
        UpdateBowDirection();

        // Actualizar las animaciones
        UpdateAnimations(moveX, moveY);

        // Disparar la flecha al hacer clic
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo
        {
            FireArrow();
        }
    }

    void UpdateAnimations(float moveX, float moveY)
    {
        // Animación de correr
        bool isRunning = moveInput.magnitude > 0;
        anim.SetBool("isRunning", isRunning);

        // Invertir la escala del personaje (NO del arco)
        if (moveX > 0)
        {
            invert = false;
            transform.localScale = new Vector3(3, 3, 1); // Mover a la derecha
        }
        else if (moveX < 0)
        {
            invert = true;
            transform.localScale = new Vector3(-3, 3, 1); // Mover a la izquierda
        }
    }

    void UpdateBowDirection()
    {
        // Obtener la posición del ratón en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Asegurarse de que la rotación no se vea afectada por el eje Z

        // Dirección hacia el ratón
        Vector2 direction = (mousePosition - bowPivot.position).normalized;

        if (invert)
        {
            direction = -direction;
        }

        // Calcular el ángulo de rotación necesario para mirar hacia el ratón
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotar el pivote del arco, pero sin invertir el arco
        bowPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireArrow()
    {
        // Activar la animación del arco al disparar
        if (bowAnimator != null)
        {
            bowAnimator.SetTrigger("Shoot");
        }

        // Crear la flecha
        GameObject arrow = Instantiate(arrowPrefab, bowPivot.position, bowPivot.rotation);

        // Configurar la dirección de la flecha
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        if (arrowController != null)
        {
            // La flecha se moverá en la dirección en la que apunta el arco
            Vector3 direction = bowPivot.right;
            if (invert)
            {
                direction = -direction;
            }
            arrowController.ConfigurarDireccion(direction);
        }
    }
}