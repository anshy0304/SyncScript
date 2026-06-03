import * as signalR from '@microsoft/signalr'
import type { CRDTOperation } from '../types/crdt'

const connection = new signalR.HubConnectionBuilder()
  .withUrl('https://localhost:7295/synchub')
  .withAutomaticReconnect()
  .build()

export async function startConnection(): Promise<void> {
  await connection.start()
  console.log('Connected to SyncHub')
}

export function sendOperation(operation: CRDTOperation): void {
  connection.invoke('SendOperation', operation)
}

export function onReceiveOperation(callback: (operation: CRDTOperation) => void): void {
  connection.on('ReceiveOperation', callback)
}

export default connection