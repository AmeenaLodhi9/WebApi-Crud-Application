﻿namespace webApiCrudApplication.CommonLayer.model
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
