import { Status } from './status'

export type Internship = {
  id: number
  companyId: number
  companyName: string
  employeeId: number
  employeeName: string
  userId: number
  userName: string
  userEmail: string
  userTcKimlikNo: number
  startDate: Date
  endDate: Date
  totalDays: number
  insuranceType: InsuranceType
  statusName: Status
  comment: string
}

export type ExternalAccountController = {
  id: number
  firstName: string
  lastName: string
  email: string
  tcKimlikNo: number
  birthYear: number
  companyName: string
  employeeName: string
  startDate: Date
  endDate: Date
}

export type InternshipStatusByInternshipId = {
  id: number
  internshipId: number
  statusId: number
  statusName: Status
  comment: string
  created: Date
}

export enum InsuranceType {
  School = 1,
  Company = 2
}
