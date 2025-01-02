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

        // Actualizar la direcci�n del arco (seg�n la posici�n del rat�n)
        UpdateBowDirection();

        // Actualizar las animaciones
        UpdateAnimations(moveX, moveY);

        // Disparar la flecha al hacer clic
        if (Input.GetMouseButtonDown(0)) // Bot�n izquierdo
        {
            FireArrow();
        }
    }

    void UpdateAnimations(float moveX, float moveY)
    {
        // Animaci�n de correr
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
        // Obtener la posici�n del rat�n en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Asegurarse de que la rotaci�n no se vea afectada por el eje Z

        // Direcci�n hacia el rat�n
        Vector2 direction = (mousePosition - bowPivot.position).normalized;

        if (invert)
        {
            direction = -direction;
        }

        // Calcular el �ngulo de rotaci�n necesario para mirar hacia el rat�n
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotar el pivote del arco, pero sin invertir el arco
        bowPivot.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireArrow()
    {
        // Activar la animaci�n del arco al disparar
        if (bowAnimator != null)
        {
            bowAnimator.SetTrigger("Shoot");
        }

        // Crear la flecha
        GameObject arrow = Instantiate(arrowPrefab, bowPivot.position, bowPivot.rotation);

        // Configurar la direcci�n de la flecha
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        if (arrowController != null)
        {
            // La flecha se mover� en la direcci�n en la que apunta el arco
            Vector3 direction = bowPivot.right;
            if (invert)
            {
                direction = -direction;
            }
            arrowController.ConfigurarDireccion(direction);
        }
    }
}