using System;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public readonly struct WhenActionResult : IEquatable<WhenActionResult>
    {
        public bool IsSuccessful { get; }
        public Exception Exception { get; }
            
        public static WhenActionResult Success() => new WhenActionResult(true, default);
        public static WhenActionResult Failure(Exception exception) => new WhenActionResult(false, exception);

        private WhenActionResult(bool isSuccessful, Exception exception)
        {
            IsSuccessful = isSuccessful;
            Exception = exception;
        }

        public bool Equals(WhenActionResult other) =>
            (IsSuccessful, Exception).Equals((other.IsSuccessful, other.Exception));
        public override bool Equals(object obj) => obj is WhenActionResult other && Equals(other);
        public override int GetHashCode() => (IsSuccessful, Exception).GetHashCode();
    }
}