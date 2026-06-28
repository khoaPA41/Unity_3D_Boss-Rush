using System;
using System.Collections;
using UnityEngine;

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
    
    private bool _isFirstTime = true;
    private float currentTime;
    private Color treeEmissionColor;
    private Material treeMeshMaterial;
    private void Start()
    {
        WorldUIManager.instance.ActiveSystemUIEvent += ActiveCheckPointFirstTime;
        treeMeshMaterial =  treeMeshRenderer.material;
        treeEmissionColor = treeMeshMaterial.GetColor("_EmissionColor");
       
    }
    private void OnDisable()
    {
        WorldUIManager.instance.ActiveSystemUIEvent -= ActiveCheckPointFirstTime;
    }

    private void ActiveCheckPointFirstTime()
    {
        if (!_isFirstTime) return;

        _isFirstTime = false;
        _checkPointVFX.Play();
        StartCoroutine(ChangeMaterialsCoroutine());
        leafMeshRenderer1.material = _leafMaterial1;
        leafMeshRenderer2.material = _leafMaterial2;
    }
    
    private IEnumerator ChangeMaterialsCoroutine(){
        while (currentTime < 4)
        {
            currentTime += Time.deltaTime;
            var t = Mathf.Clamp01(currentTime / 4);
            var currentIntensity = _changeEmissionColorCurve.Evaluate(t);
            treeEmissionColor = new Color(255 / 255, 216 / 255, 0f);
            
            var targetIntensity = 3f;
            
            var finalColor = treeEmissionColor * Mathf.Pow(2f, targetIntensity);

            var finalIntensity = finalColor * treeEmissionColor;
            
            treeMeshMaterial.SetColor("_EmissionColor", finalIntensity * currentIntensity);
            
            yield return null;
        } 
    }
}
