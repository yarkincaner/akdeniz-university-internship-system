export type Company = {
  id: number
  name: string
  address: string
  serviceArea: string
  phone: string
  fax?: string
  email: string
  website: string
  taxNumber?: string
  approvedBy: string
  isApproved: boolean
}
