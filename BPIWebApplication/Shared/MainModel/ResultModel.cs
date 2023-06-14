using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPIWebApplication.Shared.MainModel
{
    public class ResultModel<T>
    {
        public T? Data { get; set; }
        public bool isSuccess { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class TMSResultModel
    {
        public string data { get; set; } = string.Empty;
        public ErrorResult errorResult { get; set; } = new();
    }

    public class ErrorResult
    {
        public string errorCode { get; set; } = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
    }
}
