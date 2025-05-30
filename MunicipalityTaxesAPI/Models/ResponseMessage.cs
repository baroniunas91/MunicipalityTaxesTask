﻿using System.Text.Json.Serialization;

namespace MunicipalityTaxesAPI.Models
{
    public class ResponseMessage
    {
        public ResponseMessage(string message)
        {
            Message = message;
        }

        [JsonPropertyName("message")] public string Message { get; set; }
    }
}
