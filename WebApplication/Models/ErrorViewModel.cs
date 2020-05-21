using System;

namespace WebApplication.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public override string ToString()
        {
            return $"{nameof(RequestId)}: {RequestId}, {nameof(ShowRequestId)}: {ShowRequestId}";
        }
    }
}