using System.Collections;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float startScale = 0.7f;
    [SerializeField] private float overshootScale = 1.08f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(ShowAnimation());
    }

    public void Hide()
    {
        if (!gameObject.activeSelf)
            return;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(HideAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        float time = 0f;

        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.one * startScale;

        // Pop to slightly larger than normal
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / duration);
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);

            canvasGroup.alpha = t;

            float scale = Mathf.Lerp(
                startScale,
                overshootScale,
                smoothT
            );

            rectTransform.localScale = Vector3.one * scale;

            yield return null;
        }

        // Small settle from 1.08 -> 1.0
        time = 0f;
        float settleDuration = 0.1f;

        while (time < settleDuration)
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / settleDuration);

            rectTransform.localScale = Vector3.one *
                Mathf.Lerp(overshootScale, 1f, t);

            yield return null;
        }

        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        animationCoroutine = null;
    }

    private IEnumerator HideAnimation()
    {
        float time = 0f;
        float hideDuration = 0.15f;

        Vector3 currentScale = rectTransform.localScale;

        while (time < hideDuration)
        {
            time += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(time / hideDuration);

            canvasGroup.alpha = 1f - t;

            rectTransform.localScale = Vector3.Lerp(
                currentScale,
                Vector3.one * 0.8f,
                t
            );

            yield return null;
        }

        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.one;

        gameObject.SetActive(false);

        animationCoroutine = null;
    }
}