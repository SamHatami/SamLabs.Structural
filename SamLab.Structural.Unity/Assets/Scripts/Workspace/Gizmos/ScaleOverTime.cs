using System.Collections;
using UnityEngine;

namespace Workspace.Gizmos
{
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

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = _originalScale;

            if (_scaleCoroutine != null)
                StopCoroutine(_scaleCoroutine);

            _scaleCoroutine = StartCoroutine(ScaleObject());
        }

        private void OnDisable()
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
                var elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    var progress = elapsedTime / scaleDuration;
                    var scaleMultiplier = 1f + 1.5f * (Mathf.Sin(progress * Mathf.PI) * (maxScaleMultiplier - 1f));
                    transform.localScale = _originalScale * scaleMultiplier;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                transform.localScale = _originalScale;

                yield return new WaitForSeconds(pauseDuration);
            }
        }
    }
}