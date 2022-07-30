using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FakeShadow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float groundPositionY;
    [SerializeField] private float scaleVsDistancePARAM;

    void Update()
    {
        Vector3 position = target.position;

        position.y = groundPositionY;
        transform.position = position;

        float distance = target.position.y - groundPositionY;
        float scale = 1 - distance * scaleVsDistancePARAM;
        transform.localScale = Vector3.one * scale;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(Vector3.up * groundPositionY, Vector3.right * 40);
    }
#endif
}
