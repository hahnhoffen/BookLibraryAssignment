namespace BookLibrary.Application.Common
{
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Result { get; private set; }

        public static OperationResult<T> SuccessResult(T result, string message = "")
        {
            return new OperationResult<T>
            {
                Success = true,
                Message = message,
                Result = result
            };
        }

        public static OperationResult<T> FailureResult(string message)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message,
                Result = default
            };
        }
    }
}
