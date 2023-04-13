using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class Extensions
{
    public static IEnumerator Move(this Transform t, Vector3 pos, float duration)
    {
        Vector3 dir = pos - t.position;
        float distance = dir.magnitude;
        dir.Normalize();

        float startTime = 0;

        while (startTime < duration)
        {
            float remainingDistance = (distance * Time.deltaTime) / duration;
            t.position += dir * remainingDistance;
            startTime += Time.deltaTime;
            yield return null;
        }

        t.position = pos;
    }
}
