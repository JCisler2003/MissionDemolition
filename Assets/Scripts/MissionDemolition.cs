using UnityEngine;
using UnityEngine.UI;

public enum GameMode{
    idle,
    playing,
    levelEnd,
    gameOver
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject gameOverScreen;  // Assign Game Over UI Panel in Unity
    public Button playAgainButton;     // Assign Play Again Button in Unity

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public int failedAttempts; // Track failed attempts per level
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    private const int maxAttempts = 5; // Maximum attempts before Game Over

    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        failedAttempts = 0;

        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(RestartGame);
        }

        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        shotsTaken = 0;     // Reset shot count
        failedAttempts = 0; // Reset failed attempts for new level
        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
        gameOverScreen.SetActive(false); // Hide Game Over screen
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        if (mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level >= levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
        if (S.shotsTaken > maxAttempts) // Check if the player reached the attempt limit
        {
            S.GameOver();
        }
    }

    void GameOver()
    {
        mode = GameMode.gameOver;
        gameOverScreen.SetActive(true); // Show Game Over screen

        // Disable projectile interaction and stop audio
        DisableProjectileInteraction();
    }

    void DisableProjectileInteraction()
    {
        // Find all projectiles in the scene
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            // Stop the projectile's audio
            AudioSource audioSource = projectile.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            // Disable the projectile's Rigidbody (freeze it)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Freeze the projectile
            }
        }
    }

    void RestartGame()
    {
        gameOverScreen.SetActive(false);
        level = 0;
        shotsTaken = 0;
        failedAttempts = 0;
        StartLevel();
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}