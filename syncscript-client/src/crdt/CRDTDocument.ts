import type{ CRDTNode, CRDTOperation } from '../types/crdt';

export class CRDTDocument {
  private nodes: CRDTNode[] = []

  insert(node: CRDTNode): void {
    if (this.nodes.find(n => n.id === node.id)) return

    if (node.parentId === null) {
      this.nodes.unshift(node)
    } else {
      const parentIndex = this.nodes.findIndex(n => n.id === node.parentId)
      if (parentIndex === -1) throw new Error('Parent node not found')

      let position = parentIndex
      while (
        position + 1 < this.nodes.length &&
        this.nodes[position + 1].parentId === node.parentId &&
        this.nodes[position + 1].id < node.id
      ) {
        position++
      }

      this.nodes.splice(position + 1, 0, node)
    }
  }

  delete(nodeId: string): void {
    const node = this.nodes.find(n => n.id === nodeId)
    if (node) node.tombstoned = true
  }

  apply(operation: CRDTOperation): void {
    if (operation.type === 'Insert') {
      this.insert(operation.node)
    } else {
      this.delete(operation.node.id)
    }
  }

  getNodes(): CRDTNode[] {
    return this.nodes.filter(n => !n.tombstoned)
  }
}