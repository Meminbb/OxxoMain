using UnityEngine;

public class Patrullar : MonoBehaviour
{
    public Transform[] puntosMovimiento;

    [SerializeField] private float velocidadMovimento = 2f;
    [SerializeField] private float distanciaMinima = 0.1f;
    [SerializeField] private float tiempoEspera = 2f;
    [SerializeField] private float tiempoPausaLarga = 5f;
    [SerializeField, Range(0f, 1f)] private float probabilidadDePausa = 0.3f;

    private int siguientePaso = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool esperando = false;
    private float temporizador = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (puntosMovimiento.Length > 0)
            Girar(puntosMovimiento[siguientePaso]);
        else
            Debug.LogWarning("El NPC no tiene puntos asignados.");
    }

    void Update()
    {
        if (puntosMovimiento == null || puntosMovimiento.Length == 0)
            return;

        if (esperando)
        {
            temporizador -= Time.deltaTime;
            if (temporizador <= 0f)
            {
                esperando = false;
                Girar(puntosMovimiento[siguientePaso]);
            }

            if (animator != null)
                animator.SetBool("isWalking", false);

            return;
        }

        if (animator != null)
            animator.SetBool("isWalking", true);

        transform.position = Vector2.MoveTowards(transform.position, puntosMovimiento[siguientePaso].position, velocidadMovimento * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntosMovimiento[siguientePaso].position) < distanciaMinima)
        {
            
            if (siguientePaso == puntosMovimiento.Length - 1)
            {
                Ritmo_NPCSpawner spawner = FindObjectOfType<Ritmo_NPCSpawner>();
                if (spawner != null)
                {
                    spawner.SpawnNuevoNPC();
                }

                Destroy(gameObject);
                return;
            }

            siguientePaso = (siguientePaso + 1) % puntosMovimiento.Length;

            esperando = true;
            temporizador = (Random.value < probabilidadDePausa) ? tiempoPausaLarga : tiempoEspera;
        }
    }

    private void Girar(Transform destino)
    {
        if (transform.position.x < destino.position.x)
            spriteRenderer.flipX = false; 
        else
            spriteRenderer.flipX = true;  
    }
}
