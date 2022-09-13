using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexGridMap
{
    public class HexGrid : MonoBehaviour
    {
        [Header("Selectable tiles")] [SerializeField]
        public Material selectableMaterial;

        [SerializeField] public float selectableSize = 9f;
        [SerializeField] public float selectableHeight = 0f;

        [Header("Active tiles")] [SerializeField]
        public Material activeMaterial;

        [SerializeField] public float activeSize = 10f;
        [SerializeField] public float activeHeight = 1f;

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
            GameObject tileObject = new GameObject($"Tile:{coordinates.x}:{coordinates.y}");
            tileObject.transform.SetParent(transform);
            tileObject.AddComponent<MeshFilter>();
            tileObject.AddComponent<MeshRenderer>();
            tileObject.AddComponent<HexTileRenderer>();
            HexTile tile = tileObject.AddComponent<HexTile>();
            tile.state = state;
            tile.SetCoordinates(coordinates, activeSize);
            tile.DrawTile(
                state == HexTileState.Selectable ? selectableSize : activeSize,
                state == HexTileState.Selectable ? selectableHeight : activeHeight,
                state == HexTileState.Selectable ? selectableMaterial : activeMaterial,
                animated
            );
            return tile;
        }
    }
}