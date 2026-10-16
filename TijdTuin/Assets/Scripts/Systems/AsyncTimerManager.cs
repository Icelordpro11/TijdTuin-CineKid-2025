using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Provides asynchronous timer management, allowing you to start, track, and cancel
/// multiple timers running concurrently.
/// </summary>
public static class AsyncTimerManager
{
    private static readonly List<CancellationTokenSource> ActiveTimers = new();
    
    /// <summary>
    /// Invokes <paramref name="onTimerStarted"/> at the start, waits asynchronously for the specified delay,
    /// then invokes <paramref name="onTimerEnded"/> when the timer completes.
    /// </summary>
    /// <param name="delay">The delay duration to wait (supports hours, minutes, and seconds).</param>
    /// <param name="onTimerStarted">Action invoked when the timer starts.</param>
    /// <param name="onTimerEnded">Action invoked when the timer finishes.</param>
    public static async void Timer(TimeSpan delay, Action onTimerStarted, Action onTimerEnded)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        ActiveTimers.Add(cts);

        onTimerStarted?.Invoke();

        float targetTime = Time.time + (float)delay.TotalSeconds;

        try
        {
            while (Time.time < targetTime)
            {
                cts.Token.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            onTimerEnded?.Invoke();
        }
        finally
        {
            ActiveTimers.Remove(cts);
            cts.Dispose();
        }
    }
    
    /// <summary>
    /// Cancels all active timers and clears the ActiveTimers list.
    /// </summary>
    public static void StopAllTimers()
    {
        foreach (var cts in ActiveTimers)
        {
            if (!cts.IsCancellationRequested)
                cts.Cancel();
        }

        ActiveTimers.Clear();
    }
}
