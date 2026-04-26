using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BounceEffect : MonoBehaviour
{
    public float bounceHeight = 0.3f; // The height of the bounce
    public float bounceDuration = 0.4f; // The duration of the bounce

    public int bounceCount = 3; // Number of bounces

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localHeight = bounceHeight; // Use local height for the bounce
        float localDuration = bounceDuration; // Use local duration for the bounce

        for (int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(transform, startPosition, localHeight, localDuration / 2); // Wait for the bounce to complete
            localHeight *= 0.5f; // Reduce the bounce height for each subsequent bounce
            localDuration *= 0.8f; // Reduce the bounce duration for each subsequent bounce
        }

        transform.position = startPosition; // Ensure the object starts at the original position
    }

    private IEnumerator Bounce(Transform objectTransform, Vector3 start, float height, float duration)
    {
        Vector3 peak = start + Vector3.up * height; // Calculate the peak position of the bounce
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(start, peak, elapsedTime / duration ); // Move towards the peak
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }


        elapsedTime = 0f; // Reset elapsed time for the descent

         while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(peak, start, elapsedTime / duration ); // Move towards the start position
            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }
    }
}
