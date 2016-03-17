using UnityEngine;
using System.Collections;

namespace UVWorld {

    public abstract class AbstractUVWWorld : MonoBehaviour {
        public Vector3 extrude = Vector3.one;

        public abstract Vector3 World (Vector3 uvw, bool extrude = true);

        protected Vector3 Extrude(Vector3 uvw) {
            return new Vector3 (
                extrude.x * (uvw.x - 0.5f) + 0.5f,
                extrude.y * (uvw.y - 0.5f) + 0.5f,
                extrude.z * (uvw.z - 0.5f) + 0.5f);
        }
    }
}
