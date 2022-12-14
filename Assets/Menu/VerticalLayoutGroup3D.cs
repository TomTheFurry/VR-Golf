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
            foreach (Transform child in transform) {
                position -= Spacing * Vector3.up;

                Vector3 childHeight = child.GetComponent<MeshRenderer>().bounds.size.y * Vector3.up;

                if ((position - childHeight).y < bounds.min.y + padding.Bottom) {
                    continue;
                }
                position -= childHeight / 2f;
                child.position = position;
                position -= childHeight / 2f;
            }
        }
    }
}
