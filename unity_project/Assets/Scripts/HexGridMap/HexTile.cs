using System;
using UnityEngine;

namespace HexGridMap
{
    /// <summary>
    /// Class to create and drive a tile on the hexagonal grid map.
    ///
    /// Algorithms are based on the detailed guide from Red Blob Games: https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    [RequireComponent(typeof(HexTileRenderer))]
    public class HexTile : MonoBehaviour
    {
        public HexTileState state = HexTileState.Inactive;
        private Vector2Int coordinates = Vector2Int.zero;

        private HexTileRenderer hRenderer;

        public static readonly Vector2Int[] Neighbors =
        {
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 0),
        };

        private void Awake()
        {
            hRenderer = GetComponent<HexTileRenderer>();
        }

        public String GetCoordinatesId()
        {
            return $"{coordinates.x}:{coordinates.y}";
        }

        public void SetCoordinates(Vector2Int nCoordinates, float gridSize)
        {
            coordinates = nCoordinates;
            float q = coordinates.x;
            float r = coordinates.y;
            float width = gridSize * Mathf.Sqrt(3);
            float height = gridSize * 2f;
            bool offset = r % 2 != 0;
            transform.localPosition = new Vector3(
                q * width + Mathf.Sign(r) * (offset ? width / 2f : 0),
                0f,
                -(r * height * (3f / 4f))
            );
        }

        public void DrawTile(float size, float height, Material material, bool animated)
        {
            if (hRenderer != null && !animated)
            {
                hRenderer.DrawMesh(size, height, material);
            }
            else
            {
                // TODO: Create an animation
            }
        }
    }

    public enum HexTileState
    {
        Inactive,
        Selectable,
        Active
    }
}