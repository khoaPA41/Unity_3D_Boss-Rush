using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActiveCheckPoint : MonoBehaviour
{
    [Header("Tree VFX")]
    [SerializeField] private MeshRenderer treeMeshRenderer;
    [SerializeField] private MeshRenderer leafMeshRenderer1;
    [SerializeField] private MeshRenderer leafMeshRenderer2;
    
    [Header("Tree Materials For CheckPoint")]
    [SerializeField] private Material _treeMaterial;
    [SerializeField] private Material _leafMaterial1;
    [SerializeField] private Material _leafMaterial2;
    [SerializeField] private AnimationCurve _changeEmissionColorCurve;
    
    [Header("Tree VFX")]
    [SerializeField] private ParticleSystem _shockWave;
    [SerializeField] private ParticleSystem _checkPointVFX;

    [Header("Fade Img")]
    [SerializeField] private Image fadeBackground;
    
    private bool _isFirstTime = true;
    private float currentTime;
    private Color treeEmissionColor;
    private Material treeMeshMaterial;
    private void Start()
    {
        WorldUIManager.instance.ActiveSystemUIEvent += HandleCheckPoint;
        WorldUIManager.instance.ActiveFadeOnEvent += ActiveFadeOnEvent;
        WorldUIManager.instance.ActiveFadeOutEvent += ActiveFadeOutEvent;
        treeMeshMaterial =  treeMeshRenderer.material;
        treeEmissionColor = treeMeshMaterial.GetColor("_EmissionColor");
       
    }
    private void OnDisable()
    {
        WorldUIManager.instance.ActiveSystemUIEvent -= HandleCheckPoint;
        WorldUIManager.instance.ActiveFadeOnEvent -= ActiveFadeOnEvent;
        WorldUIManager.instance.ActiveFadeOutEvent -= ActiveFadeOutEvent;
    }

    private void HandleCheckPoint()
    {
        // if (_isFirstTime)
        // {
        //     ActiveCheckPointFirstTime();
        //     // return;
        // }
        
        // ActiveCheckPointNextTime();
    }

    private void ActiveCheckPointNextTime()
    {
        StartCoroutine(FadeBackground(1f, 1f));
        // WorldUIManager.instance.HandleActiveSystemUI();
        // StartCoroutine(FadeBackground(1f, 0f));
        
    }
    
    private void ActiveFadeOnEvent()
    {
        StartCoroutine(FadeBackground(1f, 1f));

    }

    private void ActiveFadeOutEvent()
    {
        StartCoroutine(FadeBackground(1f, 0f));

    }
    
    private void ActiveCheckPointFirstTime()
    {
        if (!_isFirstTime) return;
        _isFirstTime = false;
        _checkPointVFX.Play();
        StartCoroutine(ChangeMaterialsCoroutine());
        leafMeshRenderer1.material = _leafMaterial1;
        leafMeshRenderer2.material = _leafMaterial2;
        WorldUIManager.instance.HandleActiveSystemUI();
    }
    
    private IEnumerator ChangeMaterialsCoroutine(){
        while (currentTime < 3)
        {
            currentTime += Time.deltaTime;
            var t = Mathf.Clamp01(currentTime / 3);
            var currentIntensity = _changeEmissionColorCurve.Evaluate(t);
            treeEmissionColor = new Color(255 / 255, 216 / 255, 0f);
            
            var targetIntensity = 3f;
            
            var finalColor = treeEmissionColor * Mathf.Pow(2f, targetIntensity);

            var finalIntensity = finalColor * treeEmissionColor;
            
            treeMeshMaterial.SetColor("_EmissionColor", finalIntensity * currentIntensity);
            
            yield return null;
        } 
    }

    private IEnumerator FadeBackground(float duration, float targetOpacity)
    {
        var currentColor = fadeBackground.color;
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            var t =  elapsedTime / duration;
            
            var newOpacity = Mathf.Lerp(currentColor.a, targetOpacity, t);
            currentColor.a = newOpacity;
            fadeBackground.color = currentColor;
            yield return null;
        }

        currentColor.a = targetOpacity;
        fadeBackground.color = currentColor;
    }


}
