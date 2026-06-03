export interface CRDTNode{
    id:string
    parentId:string | null
    value:string
    tombstoned:boolean
    userId:string
}

export type OperationType = 'Insert' | 'Delete'
export interface CRDTOperation{
    type:OperationType
    node:CRDTNode
    documentId:string
}