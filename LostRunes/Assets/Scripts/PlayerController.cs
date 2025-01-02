using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del personaje
    public bool canMove = true; // Control de movimiento

    private Animator anim;
    private Vector2 moveInput;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;

        // Movimiento en X e Y usando W, A, S, D
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;     // Arriba
        if (Input.GetKey(KeyCode.S)) moveY = -1f;    // Abajo
        if (Input.GetKey(KeyCode.A)) moveX = -1f;    // Izquierda
        if (Input.GetKey(KeyCode.D)) moveX = 1f;     // Derecha

        // Normaliza el movimiento para evitar velocidad más rápida en diagonal
        moveInput = new Vector2(moveX, moveY).normalized;

        // Mueve al personaje
        transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        // Actualiza animaciones
        UpdateAnimations(moveX, moveY);

        // Ataques
        if (Input.GetButtonDown("Fire1"))
        {
            Attack(moveX, moveY);
        }
    }


    void UpdateAnimations(float moveX, float moveY)
    {
        // Animación de correr
        bool isRunning = moveInput.magnitude > 0;
        anim.SetBool("isRunning", isRunning);

        // Invertir sprite si se mueve a la izquierda
        if (moveX > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    void Attack(float moveX, float moveY)
    {
        canMove = false; // Bloquea el movimiento mientras ataca
        anim.SetBool("isAttacking", true);

        // Determina la dirección del ataque
        if (moveX > 0)
            anim.SetFloat("attackDirection", 0); // Derecha
        else if (moveX < 0)
            anim.SetFloat("attackDirection", 1); // Izquierda
        else if (moveY > 0)
            anim.SetFloat("attackDirection", 2); // Adelante
        else if (moveY < 0)
            anim.SetFloat("attackDirection", 3); // Atrás
        else
            anim.SetFloat("attackDirection", 2); // Por defecto adelante

        // Regresa al movimiento después de 0.1 segundos
        Invoke("ResetAttack", 0.1f);
    }

    void ResetAttack()
    {
        canMove = true;
        anim.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta colisiones con objetos o enemigos
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Colisión con enemigo");
        }
    }
}
