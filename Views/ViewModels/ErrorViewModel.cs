using System;

namespace LampWebStore.Views.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId { get => !String.IsNullOrEmpty(RequestId); }
    }
}