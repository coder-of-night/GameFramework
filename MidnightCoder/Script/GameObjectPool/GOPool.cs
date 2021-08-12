using System;
using System.Collections.Generic;
using UnityEngine;

namespace MidnightCoder.Game {
    public class GOPool {
        //
        // Fields
        //
        public GameObject goParent;

        public GameObject goSource;

        public List<GOPoolElement> list = new List<GOPoolElement> ();

        //
        // Constructors
        //
        public GOPool (GameObject source, GameObject parent = null) {
            if (parent != null)
            {
                this.goParent = parent;
            }
            else
            {
                this.goParent = new GameObject("_pool_" + source.name);
                this.goParent.transform.position = default(Vector3);
            }
            this.goSource = UnityEngine.Object.Instantiate<GameObject> (source);
            this.goSource.SetActive (false);
            this.goSource.transform.parent = this.goParent.transform;
            this.goSource.name = this.goSource.name.Replace ("(Clone)", string.Empty);
        }

        //
        // Methods
        //
        public bool Destroy (GameObject go) {
            if (go == null || this.goParent == null) {
                return false;
            }
            foreach (GOPoolElement current in this.list) {
                if (current.isUsed) {
                    if (current.go == go) {
                        current.isUsed = false;
                        current.go.SetActive (false);
                        current.go.transform.parent = this.goParent.transform;
                        current.go.transform.localScale = Vector3.one;
                        return true;
                    }
                }
            }
            return false;
        }

        public void DestroyPool () {
            Logger.Log ("DestroyPool {0}", new object[] {
                this.goParent.name
            });
            this.list.Clear ();
            UnityEngine.Object.Destroy (this.goParent);
        }

        public GameObject Instantiate () {
            if (this.goParent == null) {
                return null;
            }
            foreach (GOPoolElement current in this.list) {
                if (!current.isUsed) {
                    current.isUsed = true;
                    GameObject go = current.go;
                    return go;
                }
            }
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject> (this.goSource);
            if (gameObject != null) {
                GOPoolElement gOPoolElement = new GOPoolElement (gameObject);
                gOPoolElement.isUsed = true;
                this.list.Add (gOPoolElement);
                gameObject.transform.parent = this.goParent.transform;
                gameObject.name = gameObject.name.Replace("(Clone)", string.Empty).TrimEnd();
                gameObject.SetActive(false);
                return gameObject;
            }
            return null;
        }
    }
}