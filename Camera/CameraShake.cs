using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        var originalPosition = transform.localPosition;

        var elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            var offsetX = Random.Range(-1f, 1f) * magnitude;
            var offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; 
        }

        transform.localPosition = originalPosition;
    }
}

