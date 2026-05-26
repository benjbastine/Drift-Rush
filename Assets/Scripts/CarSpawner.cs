using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Player 1")]
    public GameObject playerCarPrefab;
    public Transform playerSpawnPoint;

    [Header("Player 2")]
    public GameObject player2CarPrefab;
    public Transform player2SpawnPoint;

    [Header("AI Cars")]
    public GameObject[] aiCarPrefabs;
    public Transform[] aiSpawnPoints;

    [Header("Game Mode")]
    public bool isTwoPlayerMode = false;

    private LapManager lapManager;

    void Awake()
    {
        lapManager = GetComponent<LapManager>();

        if (PlayerPrefs.HasKey("PlayerMode"))
            isTwoPlayerMode = PlayerPrefs.GetInt("PlayerMode") == 2;

        // Spawn Player 1
        GameObject player1 = Instantiate(
            playerCarPrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation
        );
        player1.name = "Player 1";
        lapManager.RegisterCar(player1);

        if (isTwoPlayerMode)
        {
            // Spawn Player 2
            GameObject player2 = Instantiate(
                player2CarPrefab,
                player2SpawnPoint.position,
                player2SpawnPoint.rotation
            );
            player2.name = "Player 2";
            lapManager.RegisterCar(player2);

            // Spawn AI Cars
            for (int i = 0; i < aiCarPrefabs.Length; i++)
            {
                GameObject ai = Instantiate(
                    aiCarPrefabs[i],
                    aiSpawnPoints[i].position,
                    aiSpawnPoints[i].rotation
                );
                ai.name = $"BotRacer{i + 1}";
                lapManager.RegisterCar(ai);
            }
        }
        else
        {
            // Single player — spawn all AI cars
            for (int i = 0; i < aiCarPrefabs.Length; i++)
            {
                GameObject ai = Instantiate(
                    aiCarPrefabs[i],
                    aiSpawnPoints[i].position,
                    aiSpawnPoints[i].rotation
                );
                ai.name = $"BotRacer{i + 1}";
                lapManager.RegisterCar(ai);
            }
        }

        // Freeze all cars immediately after spawning
        FreezeAllCars();
    }

    void FreezeAllCars()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in cars)
        {
            Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
            CarController cc = car.GetComponent<CarController>();
            Player2Controller p2 = car.GetComponent<Player2Controller>();
            AICarController ai = car.GetComponent<AICarController>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (cc != null) cc.canMove = false;
            if (p2 != null) p2.canMove = false;
            if (ai != null) ai.enabled = false;
        }
    }
}