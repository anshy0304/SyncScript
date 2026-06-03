import { useState } from 'react'
import NameInput from './components/NameInput'
import Editor from './components/Editor'
import { CRDTDocument } from './crdt/CRDTDocument'
import type { CRDTNode } from './types/crdt'
import { startConnection } from './hub/syncHubConnection'

const document = new CRDTDocument()

function App() {
  const [userId, setUserId] = useState<string | null>(null)
  const [nodes, setNodes] = useState<CRDTNode[]>([])

  async function handleNameSubmit(name: string) {
    await startConnection()
    setUserId(name)
  }

  if (userId === null) {
    return <NameInput onNameSubmit={handleNameSubmit} />
  }

  return (
    <div style={{ padding: '20px' }}>
      <h1>SyncScript</h1>
      <p>Editing as: <strong>{userId}</strong></p>
      <Editor
        userId={userId}
        document={document}
        nodes={nodes}
        onNodesChange={setNodes}
      />
    </div>
  )
}

export default App