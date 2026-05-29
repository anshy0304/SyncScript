using System;
using System.Collections.Generic;
using System.Text;

namespace SyncScript.Core
{
    public class CRDTNode
    {
        public Guid Id { get; init; }
        public Guid? ParentId { get; init; }
        public char Value { get; init; }
        public bool Tombstoned { get; set; }
        public string UserId { get; init; }
        public CRDTNode(Guid id,Guid? parentid,char value,string userid)
        {
            Id = id;
            ParentId = parentid;
            Value = value;
            Tombstoned = false;
            UserId = userid;
        }


    }
}
