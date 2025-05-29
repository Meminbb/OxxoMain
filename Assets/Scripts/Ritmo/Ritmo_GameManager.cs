using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ritmo_GameManager : MonoBehaviour
{
    public static Ritmo_GameManager instance;

    [Header("Audio y ritmo")]
    public AudioSource theMusic;
    public Ritmo_BeatScroller theBS;
    public bool startPlaying;

    [Header("Puntaje")]
    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    [Header("Multiplicador")]
    public int currentMultiplier = 1;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    [Header("Intentos")]
    public int intentosRestantes = 5;
    public Text intentosText;
    public bool gameOver = false;

    [Header("UI de Puntaje")]
    public Text scoreText;
    public Text multiText;

    [Header("Panel Game Over")]
    public GameObject gameOverPanel;
    public Text textoPuntuacion;
    public Button botonReintentar;
    public Button botonMenu;

    [Header("Escoba")]
    public Ritmo_Escoba escoba;

    [Header("Indicador visual con sprite")]
    public Image medidorImage;
    public Sprite spriteVerde;
    public Sprite spriteAmarillo;
    public Sprite spriteRojo;

    void Start()
    {
        instance = this;
        currentMultiplier = 1;
        gameOver = false;

        scoreText.text = "Score: 0";
        multiText.text = "Multiplier: x1";

        if (intentosText != null)
            intentosText.text = "Intentos: " + intentosRestantes;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (multiplierThresholds == null || multiplierThresholds.Length == 0)
            multiplierThresholds = new int[] { 4, 8, 12 };

        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReintentarNivel);
        if (botonMenu != null)
            botonMenu.onClick.AddListener(IrAlMenu);

        ActualizarMedidor();
        StartGame();
    }

    void Update() { }

    private void StartGame()
    {
        if (!startPlaying)
        {
            startPlaying = true;
            theBS.hasStarted = true;
            theMusic.Play();
        }
    }

    public void NoteHit()
    {
        int index = currentMultiplier - 1;

        if (multiplierThresholds != null && index >= 0 && index < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierTracker >= multiplierThresholds[index])
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        UpdateTexts();
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        escoba.Limpiar();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        escoba.Limpiar();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        escoba.Limpiar();
    }

    public void NoteMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "Multiplier: x1";
        intentosRestantes--;

        if (intentosText != null)
            intentosText.text = "Intentos: " + intentosRestantes;

        ActualizarMedidor();

        if (intentosRestantes <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        theMusic.Stop();
        theBS.hasStarted = false;
        startPlaying = false;
        gameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (textoPuntuacion != null)
                textoPuntuacion.text = "Puntos obtenidos: " + currentScore;
        }
    }

    private void UpdateTexts()
    {
        scoreText.text = "Score: " + currentScore;
        multiText.text = "Multiplier: x" + currentMultiplier;
    }

    public void PresionarBoton(string columna)
{
    Ritmo_NoteObject[] notas = FindObjectsOfType<Ritmo_NoteObject>();
    Ritmo_NoteObject mejorNota = null;
    float mejorDistancia = float.MaxValue;

    foreach (Ritmo_NoteObject nota in notas)
    {
        if (nota.keyToPress.ToLower() == columna.ToLower() && nota.canBePressed && !nota.wasPressed)
        {
            float distancia = Mathf.Abs(nota.transform.position.y);
            if (distancia < mejorDistancia)
            {
                mejorDistancia = distancia;
                mejorNota = nota;
            }
        }
    }

    if (mejorNota != null)
    {
        mejorNota.OnHit();
    }
}


    public void ReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void ActualizarMedidor()
    {
        if (medidorImage == null) return;

        if (intentosRestantes >= 4)
            medidorImage.sprite = spriteVerde;
        else if (intentosRestantes >= 2)
            medidorImage.sprite = spriteAmarillo;
        else
            medidorImage.sprite = spriteRojo;
    }
}
