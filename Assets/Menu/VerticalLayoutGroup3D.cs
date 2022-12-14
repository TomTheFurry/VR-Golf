using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLayoutGroup3D : MonoBehaviour
{
    [Serializable]
    struct Padding {
        public float Top;
        public float Bottom;
        public float LeftAndRight;
    }

    [SerializeField] Padding padding;
    [SerializeField] float Spacing = 0.1f;

    int childNumber = 0;

    private void Update() {
        if (transform.childCount != childNumber) {
            childNumber = transform.childCount;

            Bounds bounds = GetComponent<MeshRenderer>().bounds;
            Vector3 position = bounds.center;
            position.y = bounds.max.y - padding.Top;

            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform) {
                children.Add(child);
                position.z = child.position.z;

                Vector3 childHeight = child.GetComponent<MeshRenderer>().bounds.size.y * Vector3.up;

                if ((position - childHeight - Spacing * Vector3.up).y < bounds.min.y + padding.Bottom) {
                    Bounds cBounds = child.GetComponent<MeshRenderer>().bounds;
                    Vector3 deltaX = (cBounds.size.x / 2f + Spacing / 2f) * Vector3.right;
                    if ((position + deltaX * 2).x >= bounds.max.x - padding.LeftAndRight)
                        break;
                    foreach (Transform c in children) {
                        c.position -= deltaX;
                    }
                    position += deltaX;
                    position.y = bounds.max.y - padding.Top;
                }
                position -= Spacing * Vector3.up;
                position -= childHeight / 2f;
                child.position = position;
                position -= childHeight / 2f;
            }
        }
    }
}
