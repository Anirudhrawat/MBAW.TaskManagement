using System.Net;

namespace MBAW.TaskManagement.Shared.Models
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public List<string> Messages { get; set; }

        public Response(HttpStatusCode statusCode, T data, List<string> messages)
        {
            StatusCode = (int)statusCode;
            Data = data;
            Messages = messages;
        }

        public Response(T data)
            : this(HttpStatusCode.OK, data, new List<string>()
            {
                "Success"
            })
        {
        }
    }
}
