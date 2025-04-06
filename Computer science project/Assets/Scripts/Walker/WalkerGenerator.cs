using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid { FLOOR, WALL, EMPTY }
    public enum GenerationMode { RandomWalker, FullGrid }
    public GenerationMode currentGenerationMode;

    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap; // Single Tilemap for both floor and wall tiles
    public RuleTile Floor;
    public RuleTile Wall;
    public int playableMapWidth = 30;
    public int playableMapHeight = 30;
    public int MapWidth;
    public int MapHeight;

    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercent = 0.4f;
    public float WaitTime = 0.01f;

    //public Node nodePrefab;
    //public List<Node> nodeList;
    //public GameObject nodeParent;

    public Player playerTransform;

    private bool canDrawGizmos;

    // Add a public variable for the start position
    public Vector2 StartPosition = new Vector2(15, 15);

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = FindObjectOfType<Player>();
        MapWidth = playableMapWidth + 2;
        MapHeight = playableMapHeight + 2;
        InitialiseGrid();
    }

    public void InitialiseGrid()
    {
        MaximumWalkers = Random.Range(1, 6);
        if (GameManager.Instance == null)
        {
            FillPercent = Random.Range(0.15f, 0.4f);
        }
        else if (GameManager.Instance != null)
        {
            FillPercent = Random.Range(0.15f, 0.5f) * GameManager.Instance.difficultyMultiplier;
        }            
        gridHandler = new Grid[MapWidth, MapHeight];

        // Set all tiles to EMPTY initially
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        // Based on the selected mode, either initialize with walkers or a full grid
        if (currentGenerationMode == GenerationMode.RandomWalker)
        {
            AddWallBorder();
            Walkers = new List<WalkerObject>();
            SetRandomStartPosition();
            InitializeWalkerMode();
            SpawnPlayer();
        }
        else if (currentGenerationMode == GenerationMode.FullGrid)
        {
            AddWallBorder();
            SetRandomStartPosition();
            InitializeFullGridMode();
            SpawnPlayer();
        }
    }

    void InitializeWalkerMode()
    {
        // Clamp the start position within the bounds of the grid
        int startX = Mathf.Clamp((int)StartPosition.x, 0, MapWidth - 1);
        int startY = Mathf.Clamp((int)StartPosition.y, 0, MapHeight - 1);
        Vector3Int TileStart = new Vector3Int(startX, startY, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileStart.x, TileStart.y), Vector2.zero, 0.5f);
        gridHandler[TileStart.x, TileStart.y] = Grid.FLOOR;
        tileMap.SetTile(TileStart, Floor); // Set the floor tile on the single tilemap
        Walkers.Add(curWalker);

        TileCount++;

        // Add the wall border before floor generation

        CreateSpawnArea(new Vector2Int((int)StartPosition.x, (int)StartPosition.y));


        CreateFloors();
    }

    void InitializeFullGridMode()
    {
        for (int x = 1; x < MapWidth - 1; x++) // Skip outer walls
        {
            for (int y = 1; y < MapHeight - 1; y++) // Skip outer walls
            {
                if (gridHandler[x, y] == Grid.EMPTY)
                {
                    gridHandler[x, y] = Grid.FLOOR;
                    tileMap.SetTile(new Vector3Int(x, y, 0), Floor); // Set floor tile on the tilemap
                    TileCount++;
                }
            }
        }

        //CreateNodes();
    }

    void CreateFloors()
    {
        // The floor generation should be limited to `Grid.EMPTY` cells, 
        // ensuring that walls and the wall border are not overwritten.
        while ((float)TileCount / (float)gridHandler.Length < FillPercent)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] == Grid.EMPTY) // Only place floors on empty tiles
                {
                    tileMap.SetTile(curPos, Floor); // Set the floor tile on the tilemap
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                // Optionally, you can add any additional logic here if needed.
            }

            if ((float)TileCount / (float)gridHandler.Length >= FillPercent)
            {
                //CreateNodes();
                FillEmptyTilesWithWalls(); // Fill any remaining empty tiles with walls
                return; // Exit the function when floors are completed (no need for yield)
            }
        }
    }

    void AddWallBorder()
    {
        // Add a wall border around the map (2-tile thick border)
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                // Check if it's at the outer boundary (1-tile and 2-tile thick from edges)
                if (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1 ||
                    x == 1 || x == MapWidth - 2 || y == 1 || y == MapHeight - 2)
                {
                    // Only place a wall if the tile is empty
                    if (gridHandler[x, y] == Grid.EMPTY)
                    {
                        gridHandler[x, y] = Grid.WALL; // Update the grid state to WALL
                        tileMap.SetTile(new Vector3Int(x, y, 0), Wall); // Set the wall tile on the tilemap
                    }
                }
            }
        }
    }

    void FillEmptyTilesWithWalls()
    {
        // Loop through the entire grid and place walls on empty tiles
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (gridHandler[x, y] == Grid.EMPTY)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tileMap.SetTile(tilePosition, Wall); // Set the wall tile on the tilemap
                    gridHandler[x, y] = Grid.WALL; // Update the grid state to WALL
                }
            }
        }
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

    IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x + 1, y, 0), Wall); // Set the wall tile on the single tilemap
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x - 1, y, 0), Wall); // Set the wall tile on the single tilemap
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x, y + 1, 0), Wall); // Set the wall tile on the single tilemap
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        tileMap.SetTile(new Vector3Int(x, y - 1, 0), Wall); // Set the wall tile on the single tilemap
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }

    }

    void CreateSpawnArea(Vector2Int center)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int tilePos = new Vector2Int(center.x + x, center.y + y);

                // Ensure we're within grid bounds
                if (tilePos.x >= 0 && tilePos.x < MapWidth && tilePos.y >= 0 && tilePos.y < MapHeight)
                {
                    // Set as floor in grid
                    gridHandler[tilePos.x, tilePos.y] = Grid.FLOOR;

                    // Set the floor tile in Tilemap
                    tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), Floor);

                    TileCount++; // Increase the floor tile count
                }
            }
        }
    }

    void SetRandomStartPosition()
    {
        float startx = Random.Range(3, playableMapWidth - 2);
        float starty = Random.Range(3, playableMapHeight - 2);

        StartPosition = new Vector2(startx, starty);

        CreateSpawnArea(new Vector2Int((int)startx, (int)starty));
    }

    Vector2 GetDirection()
    {
        // Define possible movement directions (up, down, left, right)
        List<Vector2> directions = new List<Vector2>
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        // Pick a random direction
        return directions[Random.Range(0, directions.Count)];
    }

    public void SpawnPlayer()
    {
        if (playerTransform == null)
        {
            return;
        }
        else
        {
            playerTransform.transform.position = new Vector3(StartPosition.x, StartPosition.y, 0);
        }
            
    }

    public void ResetGrid()  //Clears the tilemap and resets the grid, then regenerates the grid
    {
        /*foreach (Node node in nodeList)
        {
            Destroy(node.gameObject);
        }
        nodeList.Clear();
        canDrawGizmos = false; */
        tileMap.ClearAllTiles();
        TileCount = 0;

        InitialiseGrid();
    }
}


 /*   void CreateNodes()
    {
        Debug.Log("CreateNodes Called");

        // Check if nodeParent is assigned in the Inspector
        if (nodeParent == null)
        {
            Debug.LogError("Node Parent is not assigned!");
            return;
        }

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    // Instantiate the node and set its position
                    Node node = Instantiate(nodePrefab, new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity);

                    // Set the parent of the node
                    node.transform.SetParent(nodeParent.transform);

                    nodeList.Add(node);
                }
            }
        }
        CreateConnections();
    }

    void CreateConnections()
    {
        Debug.Log("CreateConnections Called");
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = i + 1; j < nodeList.Count; j++)
            {
                // Check distance between nodes
                if (Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position) < 1.5f)
                {
                    // Simply connect without the duplicate check
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]); // If bidirectional connections are needed
                }
            }
        }

        canDrawGizmos = true;
    }

    void ConnectNodes(Node from, Node to)
    {
        if (from == to) { return; }

        from.connections.Add(to);
    }

    void SpawnAI()
    {
        Node randNode = nodeList[Random.Range(0, nodeList.Count)];

        Enemy newEnemy = Instantiate(enemy, randNode.transform.position, Quaternion.identity);

        newEnemy.currentNode = randNode;
    }

    private void OnDrawGizmos()
    {
        if (canDrawGizmos)
        {
            Gizmos.color = Color.blue;
            for(int i =0; i < nodeList.Count; i++)
            {
                for(int j = 0; j < nodeList[i].connections.Count; j++)
                {
                    Gizmos.DrawLine(nodeList[i].transform.position, nodeList[i].connections[j].transform.position);
                }
            }
        }
    }
 */