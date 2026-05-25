using CodingTasks.Core;

namespace CodingTasks.Solutions.S02;

// Canonical IDisposable pattern over a native-resource stand-in: finalizer
// passes false, Dispose() suppresses finalization, Dispose(bool) is guarded so
// it runs once, and public members throw after disposal.
public sealed class Resource : IDisposable
{
    private bool _disposed;
    private bool _nativeAllocated = true;

    public void Use()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Console.WriteLine("Use() -> ok");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        // (managed cleanup would go in the `disposing` branch)
        if (_nativeAllocated)
        {
            Console.WriteLine("native buffer freed");
            _nativeAllocated = false;
        }
        _disposed = true;
    }

    ~Resource() => Dispose(false);
}

public sealed class Solution02_DisposePattern : ISolution
{
    public int Id => 2;

    public void Run()
    {
        var r = new Resource();
        r.Use();
        r.Dispose();
        r.Dispose();        // idempotent: buffer is NOT freed a second time
        try
        {
            r.Use();
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"use-after-dispose blocked: {ex.GetType().Name}");
        }
    }
}
