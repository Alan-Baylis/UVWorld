using UnityEngine;
using System.Collections;

namespace UVWorld {

    [ExecuteInEditMode]
    public class FrustumUVWWorld : AbstractUVWWorld {
        public enum DebugModeEnum { GizmoSelected = 0, Gizmo }
        public enum FitEnum { Frastum = 0, Far }
        public static readonly Vector3[] FRASTUM_VERTICES = new Vector3[]{
            new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 0f),
            new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 1f), new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f),
        };
        public static readonly int[] FRASTUM_QUADS = new int[] { 
            0, 2, 3, 1,  4, 0, 1, 5,  1, 3, 7, 5,  4, 6, 2, 0,  2, 6, 7, 3,  5, 7, 6, 4
        };

        public DebugModeEnum debugMode;
        public FitEnum fit;
        public Camera targetCam;
        public float nearPlane = 1f;
        public float farPlane = 100f;
        public Color frastumColor = Color.gray;

        Mesh _frastumMesh;
        Vector3[] _vertices;
        void OnEnable() {
            _vertices = new Vector3[8];
            _frastumMesh = new Mesh ();
            _frastumMesh.vertices = _vertices;
            _frastumMesh.normals = new Vector3[8];
            _frastumMesh.SetIndices (FRASTUM_QUADS, MeshTopology.Quads, 0);
            _frastumMesh.RecalculateBounds ();
            _frastumMesh.RecalculateNormals ();
        }
        void OnDisable() {
            DestroyImmediate (_frastumMesh);
        }
        void OnDrawGizmos() {
            if (debugMode == DebugModeEnum.Gizmo)
                DrawGizmos ();
        }
        void OnDrawGizmosSelected() {
            if (debugMode == DebugModeEnum.GizmoSelected)
                DrawGizmos ();
        }

        #region implemented abstract members of AbstractUVWWorld
        public override Vector3 World (Vector3 uvw) {
            switch (fit) {
            case FitEnum.Far:
                return WorldFar (uvw);
            default:
                return WorldFrastum (uvw);
            }
        }
        #endregion

        public Vector3 WorldFrastum(Vector3 uvw) {
            uvw.z = Mathf.Lerp (nearPlane, farPlane, uvw.z);
            return targetCam.ViewportToWorldPoint (uvw);
        }
        public Vector3 WorldFar(Vector3 uvw) {
            var z = Mathf.Lerp (nearPlane, farPlane, uvw.z);
            uvw.z = farPlane;
            var localPos = targetCam.transform.InverseTransformPoint (targetCam.ViewportToWorldPoint (uvw));
            localPos.z = z;
            return targetCam.transform.TransformPoint (localPos);
        }

        void UpdateFrastumMesh() {
            for (var i = 0; i < FRASTUM_VERTICES.Length; i++)
                _vertices [i] = World (FRASTUM_VERTICES [i]);
            _frastumMesh.vertices = _vertices;
            _frastumMesh.RecalculateBounds ();
            _frastumMesh.RecalculateNormals ();
        }

        void DrawGizmos () {
            if (targetCam == null || _vertices == null || _frastumMesh == null)
                return;
            UpdateFrastumMesh ();
            Gizmos.color = frastumColor;
            Gizmos.DrawMesh (_frastumMesh);
            Gizmos.color = 1.1f * frastumColor;
            Gizmos.DrawWireMesh (_frastumMesh);
        }
    }
}
