using UnityEngine;
using System.Collections;

namespace UVWorld {

    [RequireComponent(typeof(Collider))]
    public class RayUVWorld : AbstractUVWorld {
        public Camera targetCam;

        Collider _attachedCollider;

    	void Awake() {
            _attachedCollider = GetComponent<Collider> ();
        }
        public override bool World(Vector2 uv, out Vector3 pos, out Vector3 normal) {
            uv = Extrude (uv);
            var ray = targetCam.ViewportPointToRay (uv);

            RaycastHit hit;
            if (_attachedCollider.Raycast (ray, out hit, float.MaxValue)) {
                pos = hit.point;
                normal = hit.normal;
                return true;
            }

            pos = Vector3.zero;
            normal = Vector3.up;
            return false;
        }
    }
}
