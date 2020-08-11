using Auth_API.Models.Contracts;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Auth_API.Models.Results
{
    public class ApiResponseModel : IApiResponseModel
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        public ApiResponseModel() { }
        public ApiResponseModel(string message)
        {
            Message = message;
        }
    }

    public class ApiResponseModel<T> : IApiResponseModel<T>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        public ApiResponseModel() { }
        public ApiResponseModel(string message)
        {
            Message = message;
        }
        public ApiResponseModel(string message, T data) : this(message)
        {
            Data = data;
        }
    }

    public class ApiErrorResponseModel : ApiResponseModel<ErrorsDictionary>
    {
        public ApiErrorResponseModel() { }
        public ApiErrorResponseModel(string message) : base(message) { }
        //public ApiErrorResponseModel(string _message, ModelStateDictionary _modelState) : base(_message, new ErrorsDictionary(_modelState)) { }
    }

    public class ErrorsDictionary
    {
        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; }

        //public ErrorsDictionary(ModelStateDictionary _modelState) { Errors = _modelState.GetErrorsDictionary(); }
    }
}
