using System.Collections;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    [SerializeField] private float maxScaleMultiplier = 1.5f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float pauseDuration = 0.5f;

    private Vector3 _originalScale;
    private Coroutine _scaleCoroutine;

    //TODO Make easin functions and tie them to enum handles
    //[SerializeField]
    //private EaseInEnum SelectedEaseIn = EaseIn.Quadratic;

    void Awake()
    {
        _originalScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = _originalScale;

        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        _scaleCoroutine = StartCoroutine(ScaleObject());
    }

    void OnDisable()
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
            _scaleCoroutine = null;
        }

        transform.localScale = _originalScale;
    }

    private IEnumerator ScaleObject()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < scaleDuration)
            {
                float progress = elapsedTime / scaleDuration;
                float scaleMultiplier = 1f + (Mathf.Sin(progress * Mathf.PI) * (maxScaleMultiplier - 1f));
                transform.localScale = _originalScale * scaleMultiplier;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = _originalScale;

            yield return new WaitForSeconds(pauseDuration);
        }
    }
}