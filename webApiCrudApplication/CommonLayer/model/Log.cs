﻿namespace webApiCrudApplication.CommonLayer.model
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
