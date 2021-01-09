using System;

namespace ChatApp.Service
{
    [Serializable]
    public class APIResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public int ActivePage { get; set; }
        public bool IsSuccess { get; set; }
    } 
}
