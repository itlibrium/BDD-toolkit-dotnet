using System;

namespace ITLIBRIUM.BddToolkit
{
    public readonly struct Result : IEquatable<Result>
    {
        public bool IsSuccessful { get; }
        public bool Failed => !IsSuccessful;
        public Exception Exception { get; }

        public static Result Success() => new Result(true, default);
        
        public static Result Failure(Exception exception) => new Result(false, exception);
        
        private Result(bool isSuccessful, Exception exception)
        {
            IsSuccessful = isSuccessful;
            Exception = exception;
        }

        public bool Equals(Result other) => 
            (IsSuccessful, Exception).Equals((other.IsSuccessful, other.Exception));
        public override bool Equals(object obj) => obj is Result other && Equals(other);
        public override int GetHashCode() => (IsSuccessful, FailureReason: Exception).GetHashCode();
    }
}