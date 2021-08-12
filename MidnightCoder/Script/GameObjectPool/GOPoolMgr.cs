using System;
using System.Collections.Generic;
using UnityEngine;

namespace MidnightCoder.Game {
    public class GOPoolMgr {
        //
        // Fields
        //
        public Dictionary<string, GOPool> pools = new Dictionary<string, GOPool> ();

        //
        // Constructors
        //
        public GOPoolMgr () {
            this.pools.Clear ();
        }

        //
        // Methods
        //
        public void AddPool (GameObject prefab, GameObject parent = null) {
            if (prefab == null) {
                return;
            }
            string key = this.SanitizeName (prefab.name);
            if (this.pools.ContainsKey (key)) {
                return;
            }
            this.pools.Add (this.SanitizeName (prefab.name), new GOPool (prefab,parent));
        }

        public bool Destroy (GameObject go) {
            if (go == null) {
                return false;
            }
            string key = this.SanitizeName (go.name);
            if (this.pools.ContainsKey (key) && this.pools[key] != null && this.pools[key].Destroy (go)) {
                return true;
            }
            Destroy (go);
            return false;
        }

        public void DestroyAllPools () {
            foreach (GOPool current in this.pools.Values) {
                current.DestroyPool ();
            }
            this.pools.Clear ();
        }
        public void DestroyPool(GameObject prefab)
        {
            if (prefab == null)
            {
                return;
            }
            string key = this.SanitizeName(prefab.name);
            if (this.pools.ContainsKey(key))
            {
                this.pools.Remove(key);
            }
        }
        public GameObject Instantiate (GameObject prefab, bool addToPoolIfMissing = true) {
            if (prefab == null) {
                return null;
            }
            string key = this.SanitizeName (prefab.name);
            if (!this.pools.ContainsKey (key)) {
                if (!addToPoolIfMissing) {
                    return UnityEngine.Object.Instantiate<GameObject> (prefab);
                }
                this.AddPool (prefab);
                return UnityEngine.Object.Instantiate<GameObject> (prefab);
            }
            if (this.pools[key] == null) {
                this.pools[key] = new GOPool (prefab);
            }
            return this.pools[key].Instantiate ();
        }
        public GameObject Instantiate (string key, bool addToPoolIfMissing = true) {
            if (key == "" || !this.pools.ContainsKey (key) || this.pools[key] == null) {
                return null;
            }
            return this.pools[key].Instantiate ();
        }
        protected string SanitizeName (string name) {
            return name.Replace ("(Clone)", string.Empty).TrimEnd ();
        }
    }
}