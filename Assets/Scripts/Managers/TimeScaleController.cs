using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    public static TimeScaleController Instance;

    [Header("Time To Freeze")]
    [SerializeField] private float _freezeTime;
    [SerializeField] private float _freezeDuration;

    [Header("Time To Freeze")]
    [SerializeField] private float _slowTime;
    [SerializeField] private float _slowDuration;

    private float _hitStopEndTime;
    private float _slowMoEndTime;

    private void Awake()
    {
        Debug.Log("Time");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TimeFreeHit()
    {
        _hitStopEndTime = Time.unscaledTime + _freezeDuration;
    }

    public void TimeSlowMo()
    {
        _slowMoEndTime = Time.unscaledTime + _slowDuration;
    }

    private void Update()
    {
        var currentTime = Time.unscaledTime;

        if (currentTime < _hitStopEndTime) Time.timeScale = _freezeTime;
        else if (currentTime < _slowMoEndTime) Time.timeScale = _slowTime;
        else Time.timeScale = 1;
    }
}
