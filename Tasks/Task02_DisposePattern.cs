using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task02_DisposePattern : ICodingTask
{
    public int Id => 2;
    public string Title => "Fix the IDisposable implementation";
    public string Category => "Resource Management";

    public string Description => """
        MyClass owns a native buffer, a SafeHandle and a Stream, but its
        dispose implementation has several defects. Find and fix every bug so
        that: the finalizer is correct, managed + unmanaged resources are
        released exactly once, double-dispose is a safe no-op, and using the
        object after disposal throws ObjectDisposedException.
        """;

    public string StarterCode => """
        class MyClass : IDisposable {
            private IntPtr _nativeBuffer;
            private SafeHandle _handle;
            private Stream _stream;
            private bool _disposed = false;

            public MyClass() { /* allocate the three resources */ }

            ~MyClass() { Dispose(true); }              // bug: passes 'true'
            public virtual void Dispose() { Dispose(true); }   // bug: virtual, no SuppressFinalize

            private void Dispose(bool disposing) {     // bug: not protected/virtual
                Helper.DeallocateBuffer(_nativeBuffer);// bug: runs even on 2nd call
                if (disposing) { _handle?.Dispose(); } // bug: _stream never disposed
                _disposed = true;
            }

            public void DoSomething() { }              // bug: no disposed check
        }
        """;

    public string SolutionExplanation => """
        Six fixes against the canonical Dispose pattern:
        • The finalizer must pass false (no managed objects during finalization):
          ~MyClass() => Dispose(false);
        • Public Dispose() must not be virtual and must call
          GC.SuppressFinalize(this) so a disposed object skips finalization.
        • Dispose(bool) should be protected virtual so derived types extend it.
        • Guard with `if (_disposed) return;` so the native buffer is freed once
          and double-dispose is a no-op.
        • Dispose every managed resource (_handle AND _stream) in the
          `disposing` branch.
        • Public methods call ObjectDisposedException.ThrowIf(_disposed, this).
        """;

    public string SolutionCode => """
        class MyClass : IDisposable {
            private IntPtr _nativeBuffer;
            private SafeHandle _handle;
            private Stream _stream;
            private bool _disposed;

            public MyClass() { /* allocate */ }

            public void Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing) {
                if (_disposed) return;
                if (disposing) {            // managed
                    _handle?.Dispose();
                    _stream?.Dispose();
                }
                Helper.DeallocateBuffer(_nativeBuffer);   // unmanaged
                _nativeBuffer = IntPtr.Zero;
                _disposed = true;
            }

            ~MyClass() => Dispose(false);

            public void DoSomething()
                => ObjectDisposedException.ThrowIf(_disposed, this);
        }
        """;
}
