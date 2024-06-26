import { Internship } from './internship'

export type MyDocument = {
  internshipId: number
  internship: Internship
  documentId: number
  documentType: DocumentType
  documentURL: string
}

export type MyDocumentType = {
  name: string
}

export enum DocumentTypeName {
  InternshipReport = 1,
  TurnitinReport = 2
}
