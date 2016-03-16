using UnityEngine;
using System.Collections;

namespace UVWorld {

    public abstract class AbstractUVWorld : MonoBehaviour {
        public abstract bool World (Vector2 uv, out Vector3 pos, out Vector3 normal);
    }
}
