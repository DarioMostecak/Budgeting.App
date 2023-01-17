namespace Budgeting.Web.App.OperationResults
{
    public class OperationResult<T>
    {
        public T Payload { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
