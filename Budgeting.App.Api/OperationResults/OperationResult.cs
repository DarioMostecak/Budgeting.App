namespace Budgeting.App.Api.OperationResults
{
    public class OperationResult<T>
    {
        public T? Payload { get; set; }
        public bool IsError { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
