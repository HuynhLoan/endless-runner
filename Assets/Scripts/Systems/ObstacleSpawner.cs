using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle")]
    public GameObject cactusPrefab;

    [Header("Grass Platform")]
    public GameObject grassPrefab;
    public Vector2 grassHeightRange = new Vector2(1.5f, 3.5f);
    [Range(0f, 1f)]
    public float grassChance = 0.4f;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float spawnOffsetX = 2f;

    [Header("Ground")]
    public Transform ground;

    [Header("Coin On Grass")]
    public GameObject coinPrefab;
    public int coinPerGrass = 4;
    public float coinSpacing = 0.5f;
    public float coinHeight = 0.3f;

    [Header("Airplane")]
    public GameObject airplanePrefab;
    public Vector2 airplaneHeightRange = new Vector2(2.5f, 4.5f);
    [Range(0f, 1f)]
    public float airplaneChance = 0.2f;

    private float timer;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        float r = Random.value;

        if (r < airplaneChance)
        {
            SpawnAirplane();
        }
        else if (r < airplaneChance + grassChance)
        {
            SpawnGrass();
        }
        else
        {
            SpawnCactus();
        }
    }
    void SpawnAirplane()
    {
        if (!airplanePrefab || !ground) return;

        GameObject obj = Instantiate(airplanePrefab);

        float groundTopY =
            ground.GetComponent<Collider2D>().bounds.max.y;

        float spawnY = groundTopY +
            Random.Range(airplaneHeightRange.x, airplaneHeightRange.y);

        float spawnX = GetSpawnX();

        obj.transform.position = new Vector3(spawnX, spawnY, 0f);

        AddMove(obj);
    }

    void SpawnCactus()
    {
        if (!cactusPrefab || !ground) return;

        GameObject obj = Instantiate(cactusPrefab);

        Collider2D groundCol = ground.GetComponent<Collider2D>();
        Collider2D col = obj.GetComponent<Collider2D>();

        float spawnY = groundCol.bounds.max.y + col.bounds.extents.y;
        float spawnX = GetSpawnX();

        obj.transform.position = new Vector3(spawnX, spawnY, 0f);

        AddMove(obj);
    }

    void SpawnGrass()
    {
        if (!grassPrefab || !ground) return;

        GameObject obj = Instantiate(grassPrefab);

        float groundTopY =
            ground.GetComponent<Collider2D>().bounds.max.y;

        float spawnY = groundTopY +
            Random.Range(grassHeightRange.x, grassHeightRange.y);

        float spawnX = GetSpawnX();

        obj.transform.position = new Vector3(spawnX, spawnY, 0f);

        AddMove(obj);

        SpawnCoinsOnGrass(obj);
    }

void SpawnCoinsOnGrass(GameObject grass)
{
    if (!coinPrefab) return;

    SpriteRenderer grassSR = grass.GetComponent<SpriteRenderer>();
    if (!grassSR) return;

    float grassTopWorldY = grassSR.bounds.max.y;

    float startX =
        -((coinPerGrass - 1) * coinSpacing) / 2f;

    for (int i = 0; i < coinPerGrass; i++)
    {
        Vector3 worldPos = new Vector3(
            grass.transform.position.x + startX + i * coinSpacing,
            grassTopWorldY + coinHeight,
            0f
        );

        GameObject coin = Instantiate(
            coinPrefab,
            worldPos,
            Quaternion.identity
        );

        coin.transform.SetParent(grass.transform, true);
    }
}



    float GetSpawnX()
    {
        return Camera.main.ViewportToWorldPoint(
            new Vector3(1f, 0f, 0f)
        ).x + spawnOffsetX;
    }

    void AddMove(GameObject obj)
    {
        if (!obj.TryGetComponent(out ObstacleMovement _))
            obj.AddComponent<ObstacleMovement>();
    }

    void OnDisable()
    {
        Debug.Log("❌ ObstacleSpawner DISABLED");
    }

    void OnDestroy()
    {
        Debug.Log("💀 ObstacleSpawner DESTROYED");
    }
}
