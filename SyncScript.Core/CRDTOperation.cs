using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SyncScript.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationType
    {
        Insert,
        Delete
    }
    public class CRDTOperation
    {
        public OperationType Type { get; init; }
        public CRDTNode Node { get; init; }
        public string DocumentId { get; init; }
        
        public CRDTOperation(OperationType type,CRDTNode node,string documentid)
        {
            Type = type;
            Node = node;
            DocumentId = documentid;
        }
    }
}
