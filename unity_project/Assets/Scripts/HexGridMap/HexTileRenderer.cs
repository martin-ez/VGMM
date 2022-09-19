using System.Collections.Generic;
using UnityEngine;

namespace HexGridMap
{
    /// <summary>
    /// Class to procedurally generate a 2d or 3d hexagonal mesh
    ///
    /// Based on the video by Game Dev Guide: https://youtu.be/EPaSmQ2vtek
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class HexTileRenderer : MonoBehaviour
    {
        private Mesh mesh;
        private MeshFilter mFilter;
        private MeshRenderer mRenderer;
        private MeshCollider mCollider;
        private List<MeshFace> mFaces;

        private void Awake()
        {
            mFilter = GetComponent<MeshFilter>();
            mRenderer = GetComponent<MeshRenderer>();
            mCollider = GetComponent<MeshCollider>();

            mesh = new Mesh
            {
                name = "HexTile"
            };
            mFilter.mesh = mesh;
            mCollider.sharedMesh = mesh;
        }

        public void DrawMesh(float size, float height, Material material)
        {
            mRenderer.sharedMaterial = material;
            mesh.Clear();
            DrawMeshFaces(size, height);
            CombineMeshFaces();
        }

        private void DrawMeshFaces(float size, float height)
        {
            mFaces = new List<MeshFace>();
            // Top faces
            for (int point = 0; point < 6; point++)
            {
                mFaces.Add(CreateMeshFace(0, size, height, height, point));
            }

            if (Mathf.Abs(height) > 0.01f)
            {
                // Bottom faces
                for (int point = 0; point < 6; point++)
                {
                    mFaces.Add(CreateMeshFace(0, size, 0, 0, point, true));
                }
                // Inner faces
                for (int point = 0; point < 6; point++)
                {
                    mFaces.Add(CreateMeshFace(size, size, height, 0, point, true));
                }
                // Outer faces
                for (int point = 0; point < 6; point++)
                {
                    mFaces.Add(CreateMeshFace(0, 0, height, 0, point));
                }
            }
        }

        private void CombineMeshFaces()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < mFaces.Count; i++)
            {
                vertices.AddRange(mFaces[i].Vertices);
                uvs.AddRange(mFaces[i].Uvs);

                int offset = (4 * i);
                foreach (int triangle in mFaces[i].Triangles)
                {
                    triangles.Add(triangle + offset);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
        }
        
        private static Vector3 GetPoint(float size, float height, int index)
        {
            float angleDeg = 60 * index - 30;
            float angleRad = Mathf.PI / 180f * angleDeg;
            return new Vector3(size * Mathf.Cos(angleRad), height, size * Mathf.Sin(angleRad));
        }

        private static MeshFace CreateMeshFace(float innerRad, float outerRad, float heightA, float heightB, int point,
            bool reverse = false)
        {
            Vector3 pointA = GetPoint(innerRad, heightB, point);
            Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
            Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
            Vector3 pointD = GetPoint(outerRad, heightA, point);

            List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
            List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
            List<Vector2> uvs = new List<Vector2>() { Vector2.zero, Vector2.right, Vector2.one, Vector2.up };
            if (reverse) vertices.Reverse();

            return new MeshFace(vertices, triangles, uvs);
        }
    }

    public struct MeshFace
    {
        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector2> Uvs { get; private set; }

        public MeshFace(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
        {
            this.Vertices = vertices;
            this.Triangles = triangles;
            this.Uvs = uvs;
        }
    }
}