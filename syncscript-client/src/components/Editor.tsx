import { useEffect, useRef } from 'react'
import { CRDTDocument } from '../crdt/CRDTDocument'
import type { CRDTNode, CRDTOperation } from '../types/crdt'
import { sendOperation, onReceiveOperation } from '../hub/syncHubConnection'

interface Props {
  userId: string
  document: CRDTDocument
  nodes: CRDTNode[]
  onNodesChange: (nodes: CRDTNode[]) => void
}

function Editor({ userId, document, nodes, onNodesChange }: Props) {
  const initialized = useRef(false)

  useEffect(() => {
    if (initialized.current) return
    initialized.current = true

    onReceiveOperation((operation: CRDTOperation) => {
      document.apply(operation)
      onNodesChange([...document.getNodes()])
    })
  }, [])

  function handleKeyDown(e: React.KeyboardEvent<HTMLDivElement>) {
    e.preventDefault()

    if (e.key === 'Backspace') {
      if (nodes.length === 0) return

      const lastNode = nodes[nodes.length - 1]
      const operation: CRDTOperation = {
        type: 'Delete',
        node: lastNode,
        documentId: 'doc1'
      }
      document.apply(operation)
      onNodesChange([...document.getNodes()])
      sendOperation(operation)
      return
    }

    if (e.key.length !== 1) return

    const parentId = nodes.length === 0 ? null : nodes[nodes.length - 1].id

    const newNode: CRDTNode = {
      id: crypto.randomUUID(),
      parentId: parentId,
      value: e.key,
      tombstoned: false,
      userId: userId
    }

    const operation: CRDTOperation = {
      type: 'Insert',
      node: newNode,
      documentId: 'doc1'
    }

    document.apply(operation)
    onNodesChange([...document.getNodes()])
    sendOperation(operation)
  }

  function getColor(uid: string): string {
    return uid === userId ? 'black' : 'blue'
  }

  return (
    <div
      tabIndex={0}
      onKeyDown={handleKeyDown}
      style={{
        border: '1px solid #ccc',
        minHeight: '200px',
        padding: '10px',
        fontFamily: 'monospace',
        fontSize: '18px',
        outline: 'none',
        caretColor: 'transparent',
        cursor: 'text',
        whiteSpace: 'pre-wrap',
        wordBreak: 'break-all',
      }}
    >
      {nodes.map(node => (
        <span key={node.id} style={{ color: getColor(node.userId) }}>
          {node.value}
        </span>
      ))}
    </div>
  )
}

export default Editor