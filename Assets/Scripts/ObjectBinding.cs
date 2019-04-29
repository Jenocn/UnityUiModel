using System.Collections.Generic;
using UnityEngine;

public class ObjectBinding : MonoBehaviour {

    [System.Serializable]
    public class Node {
        public string key;
        public GameObject value;
    }

    public Node[] nodes;
    public bool importChildrenByName = false;

    private Dictionary<string, GameObject> _objects = new Dictionary<string, GameObject> ();

    private void Awake () {
        foreach (var item in nodes) {
            _objects.Add (item.key, item.value);
        }
        if (importChildrenByName) {
            _ImportTransformChildsByName (transform);
        }
    }

    private void _ImportTransformChildsByName (Transform parent) {
        var childCount = parent.childCount;
        for (int i = 0; i < childCount; ++i) {
            var child = parent.GetChild (i);
            _objects.Add (child.gameObject.name, child.gameObject);
            if (child.childCount > 0) {
                _ImportTransformChildsByName (child);
            }
        }
    }

    public GameObject Query (string key) {
        GameObject ret = null;
        _objects.TryGetValue (key, out ret);
        return ret;
    }

    public T QueryComponent<T> (string key) where T : Component {
        var obj = Query (key);
        if (obj) {
            return obj.GetComponent<T> ();
        }
        return null;
    }

}