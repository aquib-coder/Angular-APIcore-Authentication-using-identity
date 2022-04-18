using API_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Core.Model
{
    public class ResponseModel
    {
        public ResponseModel(ResponseCode RCode, string responseMessage, object dataset)
        {
            ResponseMessage = responseMessage;
            this.ResponseCode = RCode;
            Dataset = dataset;
        }
        public string ResponseMessage { get; set; }
        public ResponseCode ResponseCode { get; set; }
        public object Dataset { set; get; }
    }
}
