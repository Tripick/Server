﻿using Microsoft.AspNetCore.Mvc;

namespace TripickServer.Utils
{
    public class ServerResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public ServerResponse()
        {
            this.IsSuccess = true;
            this.Message = string.Empty;
        }
        public ServerResponse(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Result = default(T);
        }
        public ServerResponse(T t)
        {
            this.IsSuccess = true;
            this.Message = string.Empty;
            this.Result = t;
        }

        public static JsonResult ToJson()
        {
            return new JsonResult(new ServerResponse<T>());
        }

        public static JsonResult ToJson(bool isSuccess, string message)
        {
            return new JsonResult(new ServerResponse<T>(isSuccess, message));
        }

        public static JsonResult ToJson(T t)
        {
            return new JsonResult(new ServerResponse<T>(t));
        }
    }
}
