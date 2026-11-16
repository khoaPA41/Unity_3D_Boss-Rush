using UnityEngine;

public class AfterimageGhost : MonoBehaviour
{
    private static readonly int GhostColorID = Shader.PropertyToID("_GhostColor");

    private Mesh _snapshotMesh;
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _propertyBlock;


    private Color _starColor;
    private float _lifeTime;
    private float _elapsed;


    public void Initialize(Mesh snapshotMesh, MeshRenderer smeshRenderer, Color starColor, float lifeTime
    )
    {
        _snapshotMesh = snapshotMesh;
        _meshRenderer = smeshRenderer;
        _propertyBlock = new();
        _starColor = starColor;
        _lifeTime = Mathf.Max(0.01f, lifeTime);

        ApplyColor(_starColor);
    }

    private void OnDestroy()
    {
        if (_snapshotMesh != null)
            Destroy(_snapshotMesh);
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;

        var t = Mathf.Clamp01(_elapsed / _lifeTime);

        Color newColor = _starColor;
        newColor.a *= 1 - Mathf.SmoothStep(1, 0, t);

        ApplyColor(newColor);

        if (t >= 1f)
            Destroy(gameObject);
    }

    private void ApplyColor(Color color)
    {
        _propertyBlock.SetColor(GhostColorID, color);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
    }
}
