using System;
using System.Collections.Generic;
using System.Text;

namespace SyncScript.Core
{
    public enum OperationType
    {
        INSERT,
        DELETE
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
