using UnityEngine;
using System.Collections;

namespace UVWorld {

    public abstract class AbstractUVWWorld : MonoBehaviour {
        public abstract Vector3 World (Vector3 uvw);
    }
}
