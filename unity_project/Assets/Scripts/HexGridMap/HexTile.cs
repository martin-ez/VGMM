using System;
using Controllers;
using UnityEngine;

namespace HexGridMap
{
    /// <summary>
    /// Class to create and drive a tile on the hexagonal grid map.
    ///
    /// Algorithms are based on the detailed guide from Red Blob Games: https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    [RequireComponent(typeof(HexTileRenderer))]
    public class HexTile : PointerSelectable
    {
        [Header("Selectable tiles")] [SerializeField]
        public Material selectableMaterial;

        [SerializeField] public float selectableSize = 9f;
        [SerializeField] public float selectableHeight = 0f;

        [Header("Active tiles")] [SerializeField]
        public Material activeMaterial;

        [SerializeField] public float activeSize = 10f;
        [SerializeField] public float activeHeight = 1f;

        [Header("State")] [SerializeField] public HexTileState state = HexTileState.Inactive;

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

        public void SetCoordinates(Vector2Int nCoordinates)
        {
            coordinates = nCoordinates;
            float q = coordinates.x;
            float r = coordinates.y;
            float width = activeSize * Mathf.Sqrt(3);
            float height = activeSize * 2f;
            bool offset = r % 2 != 0;
            transform.localPosition = new Vector3(
                q * width + Mathf.Sign(r) * (offset ? width / 2f : 0),
                0f,
                -(r * height * (3f / 4f))
            );
        }

        public void DrawTile(bool animated)
        {
            if (hRenderer != null)
            {
                float size = state == HexTileState.Selectable ? selectableSize : activeSize;
                float height = state == HexTileState.Selectable ? selectableHeight : activeHeight;
                Material material = state == HexTileState.Selectable ? selectableMaterial : activeMaterial;

                if (animated) AnimateDrawMesh(size, height, material);
                else hRenderer.DrawMesh(size, height, material);
            }
        }

        private void AnimateDrawMesh(float size, float height, Material material)
        {
            // TODO: Create an animation
        }

        protected override void OnPointerClick()
        {
            Debug.Log($"Hex: ${GetCoordinatesId()} - Pointer click");
        }

        protected override void OnPointerEnter()
        {
            Debug.Log($"Hex: ${GetCoordinatesId()} - Pointer enter");
        }

        protected override void OnPointerExit()
        {
            Debug.Log($"Hex: ${GetCoordinatesId()} - Pointer exit");
        }
    }

    public enum HexTileState
    {
        Inactive,
        Selectable,
        Active
    }
}