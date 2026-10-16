using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Diagnostics;
using UnityEngine.Serialization;

/// <summary>
/// <c>PerformanceDebugger</c> is a Unity MonoBehaviour script that monitors and displays real-time performance statistics such as FPS,
/// memory usage, object count, draw calls, and CPU time.
/// </summary>
public class PerformanceDebugger : MonoBehaviour
{
    [Header("Settings Values")]
    [SerializeField, Range(0,1)] private float _updateInterval = 0.25f;
    [SerializeField] private TextMeshPro _debugText;

    private const bool _isActive = true;
    private float _deltaTime;
    private Stopwatch _stopwatch;

    private void Awake()
    {
        _stopwatch = new Stopwatch();
    }

    private void Start() => PerformanceMonitor_Loop().Forget();

    private void Update() => _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

    private async UniTaskVoid PerformanceMonitor_Loop()
    {
        while (true)
        {
            if (_isActive)
            {
                _stopwatch.Restart();
                UpdatePerformance_Stats();
                _stopwatch.Stop();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_updateInterval));
        }
    }
    private void UpdatePerformance_Stats()
    {
        var fps = 1.0f / _deltaTime;
        var totalMemoryLog = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
        var objectCount = Get_ObjectCount();
        var cpuTime = _deltaTime * 1000f;
        float drawCalls = Get_DrawCallEstimate();
    
        var performanceStats =
            $"<b>Performance Stats</b>\n" +
            $"==================\n" +
            $"FPS: {fps:F1}\n" +
            $"Memory: {totalMemoryLog} MB\n" +
            $"Objects: {objectCount}\n";

        var renderingStats =
            $"\n<b>Rendering</b>\n" +
            $"==================\n" +
            $"DrawCalls {drawCalls} ms\n" +
            $"CPU: {cpuTime:F2} ms\n";

        _debugText.text = performanceStats + renderingStats;
    }
    
    private static int Get_ObjectCount() => FindObjectsByType<GameObject>(FindObjectsSortMode.None).Length;
    
    private static int Get_DrawCallEstimate()
    {
        FrameTimingManager.CaptureFrameTimings();
        var frameTimings = new FrameTiming[1];
    
        if (FrameTimingManager.GetLatestTimings(1, frameTimings) > 0)
            return (int)frameTimings[0].gpuFrameTime; 
    
        return -1;
    }
}