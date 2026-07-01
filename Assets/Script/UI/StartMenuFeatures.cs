using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuFeatures : MonoBehaviour
{
    [Header("VFX Fire Tile")] [SerializeField]
    private GameObject vfxFireTile;
    [SerializeField] private float vfxFireTileSpeed;
    [SerializeField] private Vector3 targetPosition;

    [Header("Title Fade")] [SerializeField]
    private TextMeshPro titleText;
    
    [Header("Fade Background")] [SerializeField]
    private Image fadeBackground;

    private void Start()
    {
        StartCoroutine(WaitToNext(.5f,
            () => StartCoroutine(FireTileMove(1f)),
            () => StartCoroutine(TitleFade(0f, 1f, 1f)),
            () => StartCoroutine(FadeBackground(1f, 0f, 1f))
        ));
    }


    private IEnumerator FireTileMove(float duration)
    {
        var startPos = vfxFireTile.gameObject.transform.position;
        var timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            var timePercentage = timeElapsed / duration;
            vfxFireTile.transform.position = Vector3.Lerp(startPos, targetPosition, timePercentage);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vfxFireTile.transform.position = targetPosition;
    }

    private IEnumerator FadeBackground(float currentOpacity, float targetOpacity, float fadeDuration)
    {
        var timeElapsed = 0f;
        var current = fadeBackground.color;
        while (timeElapsed < fadeDuration)
        {
            var timePercentage = timeElapsed / fadeDuration;
            var newColor = Mathf.Lerp(currentOpacity, targetOpacity, timePercentage);
            current.a = newColor;
            fadeBackground.color = current;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        current.a = targetOpacity;
        fadeBackground.color = current;
    }

    private IEnumerator TitleFade(float currentOpacity, float targetOpacity, float fadeDuration)
    {
        var timeElapsed = 0f;
        var current = titleText.color;
        while (timeElapsed < fadeDuration)
        {
            var timePercentage = timeElapsed / fadeDuration;
            var newColor = Mathf.Lerp(currentOpacity, targetOpacity, timePercentage);
            current.a = newColor;
            titleText.color = current;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        current.a = targetOpacity;
        titleText.color = current;
    }
    
    private IEnumerator WaitToNext(float duration, Action action_1, Action action_2, Action action_3)
    {
        action_1?.Invoke();
        yield return new WaitForSeconds(duration);
        action_2?.Invoke();
        yield return new WaitForSeconds(duration);
        action_3.Invoke();
    }
}