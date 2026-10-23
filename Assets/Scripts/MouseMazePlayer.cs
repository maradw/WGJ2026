
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class MouseMazePlayer : MonoBehaviour
{
    [Header("Posicion inicial")]
    [SerializeField] private Transform startPoint;

    private Rigidbody2D rb;
    private Camera mainCamera;

    private Vector2 startPosition;
    private Vector2 destino;
    private Vector2 posicionCursorAlReiniciar;

    private bool bloqueado;


    private bool started;
    private Collider2D mCollider;

    /// <summary>
    /// /
    /// </summary>
    /// 

    [SerializeField] GameObject[] maze;
    [SerializeField] GameObject[] target;
    private int currentMaze = 0;


    [SerializeField] private MiniGameManager miniGameManager;


    int count = 0;
    private void ActivateMaze()
    {
        for (int i = 0; i < maze.Length; i++)
        {
            maze[i].SetActive(i == currentMaze);
            target[i].SetActive(i == currentMaze);
        }
    }
    private void OnEnable()
    {
        ActivateMaze();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;

        startPosition = startPoint != null
            ? startPoint.position
            : rb.position;

        destino = startPosition;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.position = startPosition;
    }

    private void Start()
    {
        Cursor.visible = true; //may need to hide
        Cursor.lockState = CursorLockMode.Confined;

        SincronizarCursor();
    }


    private void Update()
    {
        if (!started)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();

                Vector2 worldPosition = mainCamera.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y,Mathf.Abs(mainCamera.transform.position.z) ) );

                if (mCollider.OverlapPoint(worldPosition))
                {
                    started = true;
                }
            }

            return;
        }

        //////


        if (Mouse.current == null || mainCamera == null)
            return;

        if (bloqueado)
        {
            Vector2 cursor = Mouse.current.position.ReadValue();

            if ((cursor - posicionCursorAlReiniciar).sqrMagnitude > 4f)
            {
                bloqueado = false;
            }
            else
            {
                return;
            }
        }

        Vector2 posicionMouse = Mouse.current.position.ReadValue();

        float distanciaCamara =
            Mathf.Abs(mainCamera.transform.position.z - rb.position.y * 0f);

        Vector3 posicionPantalla = new Vector3(
            posicionMouse.x,
            posicionMouse.y,
            distanciaCamara
        );

        Vector3 posicionMundo =
            mainCamera.ScreenToWorldPoint(posicionPantalla);

        destino = new Vector2(posicionMundo.x, posicionMundo.y);
    }

    private void FixedUpdate()
    {
        if (!bloqueado)
        {
            rb.MovePosition(destino);
        }
    }

    private void SincronizarCursor()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector3 posicionPantalla =
            mainCamera.WorldToScreenPoint(startPosition);

        Mouse.current.WarpCursorPosition(
            new Vector2(posicionPantalla.x, posicionPantalla.y)
        );

        destino = startPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReiniciarSiEsPared(collision.gameObject);
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ReiniciarSiEsPared(other.gameObject);
        if (other.gameObject.CompareTag("Target"))
        {
            currentMaze++;
            print("target");
            if (currentMaze < maze.Length)
            {
                ActivateMaze();

                Debug.Log("Siguiente laberinto: " + (currentMaze + 1));
            }
            else
            {
                Debug.Log("¡Completaste los tres laberintos!");

                maze[currentMaze-1].SetActive (false);
                this.enabled = false;
                this.gameObject.SetActive(false);

                //UI finished or restart reinicia si pierdes las 3 vidas
                miniGameManager.FinishMinigame();
            }

            return;
        }
    }

    private void ReiniciarSiEsPared(GameObject objeto)
    {
        if (!objeto.CompareTag("Pared") || bloqueado)
            return;

        Debug.Log("Choque con pared: " + objeto.name);

        bloqueado = true;
        rb.position = startPosition;
        destino = startPosition;

        SincronizarCursor();

        // Guardar la posición después de recolocar el cursor.
        if (Mouse.current != null)
        {
            posicionCursorAlReiniciar =
                Mouse.current.position.ReadValue();
        }

        Debug.Log("Jugador y cursor reiniciados.");
        count++;
        print ("times hit the wall: " + count);
        //loselife
        if (GameProgressManager.Instance != null)
        {
            if(count ==3)
            {
                GameProgressManager.Instance.LoseLife();
                count = 0;
            }
            //GameProgressManager.Instance.LoseLife();
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
