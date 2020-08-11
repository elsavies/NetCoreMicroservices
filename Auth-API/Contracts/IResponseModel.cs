using System;
using System.Collections.Generic;
using System.Text;

namespace Auth_API.Models.Contracts
{
    public interface IApiResponseModel
    {
        string Message { get; set; }
    }

    public interface IApiResponseModel<T> : IApiResponseModel
    {
        T Data { get; set; }
    }
}
