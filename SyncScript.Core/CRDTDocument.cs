using System;
using System.Collections.Generic;
using System.Text;

namespace SyncScript.Core
{
    public class CRDTDocument
    {
        private readonly LinkedList<CRDTNode> _nodes = new();
        private readonly Dictionary<Guid, LinkedListNode<CRDTNode>> _index = new();
        public void Insert(CRDTNode node)
        {
            if (_index.ContainsKey(node.Id)) return;
            if(node.ParentId == null)
            {
                var listNode = _nodes.AddFirst(node);
                _index[node.Id] = listNode;
            }
            else
            {
                if(!_index.TryGetValue(node.ParentId.Value,out var parentListNode))
                {
                    throw new InvalidOperationException("Parent node not found");
                }
                var postion = parentListNode;
                while(postion.Next != null && postion.Next.Value.ParentId  == node.ParentId
                    && postion.Next.Value.Id.CompareTo(node.Id) < 0)
                {
                    postion = postion.Next;
                }

                var listNode = _nodes.AddAfter(parentListNode, node);
                    _index[node.Id] = listNode;
                
            }
            
        }
        public void Delete(Guid nodeId)
        {
            if(_index.TryGetValue(nodeId,out var listNode))
            {
                listNode.Value.Tombstoned = true;
            }
        }

        public void Apply(CRDTOperation operation)
        {
            switch (operation.Type)
            {
                case OperationType.Insert:
                    Insert(operation.Node);
                    break;
                    Delete(operation.Node.Id);
                    break;
            }
        }
        public string GetText()
        {
            var result = new System.Text.StringBuilder();
            foreach(var node in _nodes)
            {
                if (!node.Tombstoned)
                    result.Append(node.Value);
            }
            return result.ToString();
        }
    }
}
