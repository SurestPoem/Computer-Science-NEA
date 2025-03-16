using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY
    }

    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap; // Single Tilemap for both floor and wall tiles
    public Tile Floor;
    public Tile Wall;
    public int MapWidth = 30;
    public int MapHeight = 30;

    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercent = 0.4f;
    public float WaitTime = 0.5f;

    // Add a public variable for the start position
    public Vector2 StartPosition = new Vector2(15, 15);

    // Start is called before the first frame update
    void Start()
    {
        InitialiseGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitialiseGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        // Clamp the start position within the bounds of the grid
        int startX = Mathf.Clamp((int)StartPosition.x, 0, MapWidth - 1);
        int startY = Mathf.Clamp((int)StartPosition.y, 0, MapHeight - 1);
        Vector3Int TileStart = new Vector3Int(startX, startY, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileStart.x, TileStart.y), Vector2.zero, 0.5f);
        gridHandler[TileStart.x, TileStart.y] = Grid.FLOOR;
        tileMap.SetTile(TileStart, Floor); // Set the floor tile on the single tilemap
        Walkers.Add(curWalker);

        TileCount++;

        StartCoroutine(CreateFloors());
    }

    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercent)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(curPos, Floor); // Set the floor tile on the single tilemap
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
                yield return new WaitForSeconds(WaitTime);
            }

            if ((float)TileCount / (float)gridHandler.Length >= FillPercent)
            {
                FillEmptyTilesWithWalls(); // This can be placed here if the floor generation is done.
                yield break; // Exit after floors are completed
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

    void FillEmptyTilesWithWalls()
    {
        // Loop through the entire grid
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                // Check if the current tile is empty
                if (gridHandler[x, y] == Grid.EMPTY)
                {
                    // Set the tile to a wall
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tileMap.SetTile(tilePosition, Wall); // Set the wall tile on the single tilemap
                    gridHandler[x, y] = Grid.WALL; // Update the grid state
                }
            }
        }
    }
}