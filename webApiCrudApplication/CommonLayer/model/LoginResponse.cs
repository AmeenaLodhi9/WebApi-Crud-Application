﻿namespace webApiCrudApplication.CommonLayer.model
{
    public class LoginResponse
    {

        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }
}
