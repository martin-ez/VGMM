using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexGridMap
{
    public class HexGrid : MonoBehaviour
    {
        [Header("Tiles")] [SerializeField] public GameObject tilePrefab;

        private Dictionary<String, HexTile> grid;

        private void Awake()
        {
            grid = new Dictionary<string, HexTile>();
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            HexTile hub = CreateTile(Vector2Int.zero, HexTileState.Active, false);
            grid.Add(hub.GetCoordinatesId(), hub);

            foreach (var vector in HexTile.Neighbors)
            {
                HexTile tile = CreateTile(Vector2Int.zero + vector, HexTileState.Selectable, false);
                grid.Add(tile.GetCoordinatesId(), tile);
            }
        }

        private HexTile CreateTile(Vector2Int coordinates, HexTileState state, bool animated)
        {
            GameObject tileObject = Instantiate(tilePrefab, Vector3.down, Quaternion.identity, transform);
            tileObject.name = $"Tile:{coordinates.x}:{coordinates.y}";
            HexTile tile = tileObject.GetComponent<HexTile>();
            tile.state = state;
            tile.SetCoordinates(coordinates);
            tile.DrawTile(animated);
            return tile;
        }
    }
}