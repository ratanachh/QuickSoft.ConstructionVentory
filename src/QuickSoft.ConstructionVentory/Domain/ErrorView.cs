namespace QuickSoft.ConstructionVentory.Domain
{
    public class ErrorView
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}