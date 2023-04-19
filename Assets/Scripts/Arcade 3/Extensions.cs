using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class Extensions
{
    public static IEnumerator AninMove(this Transform t, Vector3 pos, float duration)
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

    public static IEnumerator AninScale(this Transform t, Vector3 scale, float duration)
    {
        Vector3 direction = scale - t.localScale;
        float size = direction.magnitude;
        direction.Normalize();

        float startTime = 0;

        while (startTime < duration)
        {
            float remainingDistance = (size * Time.deltaTime) / duration;
            t.localScale += direction * remainingDistance;
            startTime += Time.deltaTime;
            yield return null;
        }
        t.localScale = scale;
    }
}
