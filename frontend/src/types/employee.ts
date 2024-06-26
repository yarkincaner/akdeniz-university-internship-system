import { Internship } from './internship'

export type Employee = {
  id: number
  firstName: string
  lastName: string
  title: string
  email: string
  companyId: number
  companyName: string
}

export type GetEmployeeByIdViewModel = {
  id: number
  firstName: string
  lastName: string
  title: string
  email: string
  companyId: number
  companyName: string
  internships: Internship[]
}
