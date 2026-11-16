using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
public class AfterimageEmitter3D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer[] _sources;
    [SerializeField] private Material _ghostMaterial;

    [Header("References")]
    [SerializeField, Min(0.01f)] private float _spawnInterval;
    [SerializeField, Min(0.01f)] private float _emissionDuration;
    [SerializeField, Min(0.01f)] private float _ghostLifeTime;
    [SerializeField] private Color _ghostColor = new(0.3f, 0.7f, 1f, 0.6f);
    private Coroutine _emissionRoutine;

    private void Awake()
    {
        if (_sources == null || _sources.Length == 0)
            _sources = GetComponentsInChildren<SkinnedMeshRenderer>(true);
    }

    public void Play()
    {
        if (_ghostMaterial == null || _sources.Length == 0)
        {
            Debug.LogWarning("Afterimage is not configured.");
            return;
        }

        if (_emissionRoutine != null)
        {
            StopCoroutine(_emissionRoutine);
        }

        _emissionRoutine = StartCoroutine(Emit());
    }


    private IEnumerator Emit()
    {
        float elapsed = 0f;

        while (elapsed < _emissionDuration)
        {
            CaptureSnapshot();

            yield return new WaitForSeconds(_spawnInterval);
            elapsed += _spawnInterval;
        }
        _emissionRoutine = null;
    }

    private void CaptureSnapshot()
    {
        foreach (var source in _sources)
        {
            if (source == null || !source.enabled || !source.gameObject.activeInHierarchy) continue;

            Mesh bakedMesh = new();
            source.BakeMesh(bakedMesh, true);

            GameObject ghost = new("Afterimage Ghost");

            Transform ghostTransform = ghost.transform;

            ghostTransform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
            ghostTransform.localScale = source.transform.lossyScale;


            MeshFilter meshFilter = ghost.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = bakedMesh;

            MeshRenderer meshRenderer = ghost.AddComponent<MeshRenderer>();

            Material[] materials = new Material[bakedMesh.subMeshCount];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = _ghostMaterial;
            }
            meshRenderer.sharedMaterials = materials;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;


            AfterimageGhost afterimage = ghost.AddComponent<AfterimageGhost>();

            afterimage.Initialize(bakedMesh, meshRenderer, _ghostColor, _ghostLifeTime);
        }
    }
}
