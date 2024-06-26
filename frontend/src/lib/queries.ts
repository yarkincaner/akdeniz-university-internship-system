import {
  companiesRequest,
  employeesRequest,
  externalAccountRequest,
  internshipRequest,
  internshipStatusRequest,
  userRequest
} from '@/config/authConfig'
import useFetchWithMsal from '@/hooks/useFetchWithMsal'
import { Company } from '@/types/company'
import { Employee, GetEmployeeByIdViewModel } from '@/types/employee'
import {
  ExternalAccountController,
  Internship,
  InternshipStatusByInternshipId
} from '@/types/internship'
import { MyResponse, PagedResponse } from '@/types/responses'
import { SearchParamsType } from '@/types/searchParamsType'
import { User } from '@/types/user'
import axios, { AxiosError } from 'axios'
import { Dispatch, SetStateAction } from 'react'
import { useQuery } from 'react-query'
import { toast } from 'sonner'
import { getPublicHolidays } from './utils'

const defaultSearchParams: SearchParamsType = {
  searchString: '',
  pageSize: '',
  pageNumber: ''
}

export const useGetAllCompanies = (
  isEnabled: boolean = true,
  searchParams: SearchParamsType = defaultSearchParams
) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-all-companies', searchParams.searchString],
    queryFn: async () => {
      let query = new URL(companiesRequest.apiUser.endpoint)

      const { pageNumber, searchString } = searchParams
      if (searchString) {
        query.searchParams.set('SearchString', searchString)
      }
      if (pageNumber) {
        query.searchParams.set('PageNumber', pageNumber)
      }

      const response = await execute('GET', query.toString())
      return response as PagedResponse<Company>
    },
    enabled: isEnabled
  })
}

export const useGetAllEmployees = (
  searchParams: SearchParamsType = defaultSearchParams
) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-all-employees', searchParams],
    queryFn: async () => {
      let query = new URL(employeesRequest.apiUser.endpoint)

      const { pageNumber, searchString } = searchParams
      if (searchString) {
        query.searchParams.set('SearchString', searchString)
      }
      if (pageNumber) {
        query.searchParams.set('PageNumber', pageNumber)
      }

      const response = await execute('GET', query.toString())
      return response as PagedResponse<Employee>
    }
  })
}

export const useGetEmployeeById = (employeeId: number) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-employee-by-id', employeeId],
    queryFn: async () => {
      const requestUrl = new URL(
        `${employeesRequest.apiUser.endpoint}/${employeeId}`
      )
      const response = await execute('GET', requestUrl.toString())

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response as MyResponse<GetEmployeeByIdViewModel>
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    }
  })
}

export const useGetEmployeesByCompanyId = (
  isEnabled: boolean,
  companyId: number,
  searchParams: SearchParamsType = defaultSearchParams,
  errorMessages: { title: string; desc: string }
) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-all-employees-by-company-id', companyId, searchParams],
    queryFn: async ({ queryKey }) => {
      const [_key, companyId] = queryKey
      let query = new URL(
        `${employeesRequest.apiUser.endpoint}/Company/${companyId}`
      )

      const { pageNumber, searchString } = searchParams
      if (searchString) {
        query.searchParams.set('SearchString', searchString)
      }
      if (pageNumber) {
        query.searchParams.set('PageNumber', pageNumber)
      }

      const response = await execute('GET', query.toString())
      return response as PagedResponse<Employee>
    },
    onError: () => {
      toast.error(errorMessages.title, {
        description: errorMessages.desc
      })
    },
    enabled: isEnabled
  })
}

export const useGetAllInternships = (
  searchParams: SearchParamsType = defaultSearchParams,
  messages?: {
    error: { title: string; desc: string }
  }
) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-all-internships', searchParams],
    queryFn: async () => {
      let query = new URL(internshipRequest.apiUser.endpoint)

      const { pageNumber, searchString } = searchParams
      if (searchString) {
        query.searchParams.set('SearchString', searchString)
      }
      if (pageNumber) {
        query.searchParams.set('PageNumber', pageNumber)
      }

      const response = await execute('GET', query.toString())
      return response as PagedResponse<Internship>
    },
    onError: () => {
      toast.error('messages.error.title', {
        description: 'messages.error.desc'
      })
    }
  })
}

export const useGetInternshipsByUserId = (messages: {
  error: { title: string; desc: string }
}) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-internships-by-userId'],
    queryFn: async () => {
      const query = `${internshipRequest.apiUser.endpoint}/UserId`
      const response = await execute('GET', query)
      return response.data as Internship[]
    },
    onError: () => {
      toast.error(messages.error.title, {
        description: messages.error.desc
      })
    }
  })
}

export const useGetInternshipById = (internshipId: number) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-internship', internshipId],
    queryFn: async () => {
      const query = new URL(
        `${internshipRequest.apiUser.endpoint}/Internship/${internshipId}`
      )
      const response = await execute('GET', query.toString())
      return response
    },
    onError: () => {
      toast.error('Something went wrong!', {
        description: 'Please try again later!'
      })
    }
  })
}

export const useGetInternshipByToken = (token: string) => {
  return useQuery({
    queryKey: ['get-internship-by-token', token],
    queryFn: async () => {
      const query = new URL(
        `${externalAccountRequest.apiUser.endpoint}/Decode-Token`
      )
      query.searchParams.set('token', token)
      const response = await axios.get(query.toString())
      return response.data.data as ExternalAccountController
    },
    onError: (err: AxiosError) => {
      toast.error(err.name, {
        description: err.message
      })
    }
  })
}

export const useGetInternshipStatusesByInternshipId = (
  internshipId: number
) => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['get-internship-statuses-by-internship-id', internshipId],
    queryFn: async () => {
      const query = new URL(
        `${internshipStatusRequest.apiUser.endpoint}/Internship/${internshipId}`
      )
      const response = await execute('GET', query.toString())
      return response.data as InternshipStatusByInternshipId[]
    },
    onError: err => {
      toast.error('Something went wrong!', {
        description: 'Please try again later.'
      })
    }
  })
}

export const useGetUser = (dialogState: Dispatch<SetStateAction<boolean>>) => {
  const { execute } = useFetchWithMsal()

  return useQuery({
    queryKey: ['get-user'],
    queryFn: async () => {
      const query = `${userRequest.apiUser.endpoint}/GetAuthenticatedUser`
      const response = await execute('GET', query)

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response.data as User
    },
    onError: () => {
      toast.error('User Error!')
    },
    onSuccess: data => {
      if (data.tcKimlikNo === 0 || data.birthYear === 0) {
        dialogState(true)
      }
    }
  })
}

export const useGetPublicHolidays = () => {
  return useQuery({
    queryKey: ['get-public-holidays'],
    queryFn: async () => {
      const publicHolidays = await getPublicHolidays()
      return publicHolidays as Date[]
    },
    onError: err => {
      toast.error('Could not fetch public holidays!')
    }
  })
}

export const useListBlobs = () => {
  const { execute } = useFetchWithMsal()
  return useQuery({
    queryKey: ['list-blobs'],
    queryFn: async () => {
      // const query = new URL(`${fileRequest.apiUser.endpoint}/listBlobs`)
      const query = new URL('https://localhost:9001/api/File/listBlobs')
      const response = await execute('GET', query.toString())
      return response as string[]
    },
    onError: error => {
      console.error(error)
    }
  })
}
