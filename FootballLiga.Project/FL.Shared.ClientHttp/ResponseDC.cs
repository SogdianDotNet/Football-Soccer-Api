using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FL.Shared.ClientHttp
{
    [Serializable]
    [DataContract]
    public class ResponseDC<T>
    {
        public ResponseDC()
        {
            ErrorMessages = new List<ErrorMessageDC>();
        }

        [DataMember(Name = "TotalSize")]
        public int? TotalSize { get; set; }
        [DataMember(Name = "Status")]
        public string Status { get; set; }
        [DataMember(Name = "Records")]
        public T Value { get; set; } = default(T);
        [DataMember(Name = "ErrorMessages")]
        public List<ErrorMessageDC> ErrorMessages { get; set; }
        public bool IsValid
        {
            get
            {
                return ErrorMessages == null || !ErrorMessages.Any();
            }
        }

        public void AddError(string message, string description)
        {
            if (ErrorMessages == null)
            {
                ErrorMessages = new List<ErrorMessageDC>
                {
                    new ErrorMessageDC
                    {
                        Message = message,
                        Description = description
                    }
                };
            }
            else
            {
                ErrorMessages.Add(new ErrorMessageDC
                {
                    Message = message,
                    Description = description
                });
            }
        }
    }

    [Serializable]
    [DataContract]
    public class ErrorMessageDC
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }
    }
}
