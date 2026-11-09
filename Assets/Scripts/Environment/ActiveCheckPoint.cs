using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class ActiveCheckPoint : MonoBehaviour
    {
        public string checkpointID;
        [Header("Tree VFX")]
        [SerializeField] private MeshRenderer _treeMeshRenderer;
        [SerializeField] private MeshRenderer _leafMeshRenderer1;
        [SerializeField] private MeshRenderer _leafMeshRenderer2;

        [Header("Tree Materials For CheckPoint")]
        [SerializeField] private Material _treeMaterial;
        [SerializeField] private Material _leafMaterial1;
        [SerializeField] private Material _leafMaterial2;
        [SerializeField] private AnimationCurve _changeEmissionColorCurve;

        [Header("CheckPoint VFX")]
        [SerializeField] private ParticleSystem _checkPointVFX;

        [Header("Fade Img")]
        [SerializeField] private Image _fadeBackground;

        private bool _isFirstTime = true;
        private float _currentTime;
        private Color _treeEmissionColor;
        private Material _treeMeshMaterial;

        private void Start()
        {
            _treeMeshMaterial = _treeMeshRenderer.material;
            _treeEmissionColor = _treeMeshMaterial.GetColor("_EmissionColor");
        }

        public void SubscribeEvent()
        {
            WorldUIManager.Instance.ActiveSystemUIEvent += HandleCheckPoint;
        }

        public void UnsubscribeEvent()
        {
            WorldUIManager.Instance.ActiveSystemUIEvent -= HandleCheckPoint;
        }

        private void HandleCheckPoint()
        {
            if (!_isFirstTime)
            {
                ActiveCheckPointAnotherTime();
                return;
            }

            ActiveCheckPointFirstTime();
        }

        private void ActiveCheckPointFirstTime()
        {
            if (!_isFirstTime) return;
            StartCoroutine(FadeBackgroundSystemUI(() =>
                {
                    _isFirstTime = false;
                    _checkPointVFX.Play();
                    StartCoroutine(ChangeMaterialsCoroutine());
                    _leafMeshRenderer1.material = _leafMaterial1;
                    _leafMeshRenderer2.material = _leafMaterial2;
                },
                ActiveCheckPointAnotherTime,
                4f
                ));
        }

        private void ActiveCheckPointAnotherTime()
        {
            StartCoroutine(FadeBackgroundSystemUI(
                () => StartCoroutine(FadeBackground(1f, 1f)),
                () =>
                {
                    StartCoroutine(FadeBackground(1f, 0f));
                    WorldUIManager.Instance.HandleActiveSystemUI();
                },
                1f
            ));

        }

        private IEnumerator ChangeMaterialsCoroutine()
        {
            while (_currentTime < 3)
            {
                _currentTime += Time.deltaTime;
                var t = Mathf.Clamp01(_currentTime / 3);
                var currentIntensity = _changeEmissionColorCurve.Evaluate(t);
                _treeEmissionColor = new Color(255 / 255, 216 / 255, 0f);

                var targetIntensity = 3f;

                var finalColor = _treeEmissionColor * Mathf.Pow(2f, targetIntensity);

                var finalIntensity = finalColor * _treeEmissionColor;

                _treeMeshMaterial.SetColor("_EmissionColor", finalIntensity * currentIntensity);

                yield return null;
            }
        }

        private IEnumerator FadeBackground(float duration, float targetOpacity)
        {
            var currentColor = _fadeBackground.color;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                var t = elapsedTime / duration;

                var newOpacity = Mathf.Lerp(currentColor.a, targetOpacity, t);
                currentColor.a = newOpacity;
                _fadeBackground.color = currentColor;
                yield return null;
            }

            currentColor.a = targetOpacity;
            _fadeBackground.color = currentColor;
        }

        private IEnumerator FadeBackgroundSystemUI(Action action_1, Action action_2, float time)
        {
            action_1?.Invoke();
            yield return new WaitForSeconds(time);
            action_2?.Invoke();
        }
    }
}
