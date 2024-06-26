'use client'

import {
  companiesRequest,
  employeesRequest,
  externalAccountRequest,
  fileRequest,
  internshipRequest,
  userRequest
} from '@/config/authConfig'
import useFetchWithMsal from '@/hooks/useFetchWithMsal'
import { DocumentTypeName } from '@/types/document'
import { InsuranceType } from '@/types/internship'
import axios, { AxiosError } from 'axios'
import { Dispatch, SetStateAction } from 'react'
import { UseFormSetValue } from 'react-hook-form'
import { useMutation, useQueryClient } from 'react-query'
import { toast } from 'sonner'

export const useCreateCompany = (
  successMessage: string,
  errorMessage: { title: string; desc: string },
  handleDialog: () => void,
  parentForm?: UseFormSetValue<any>
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['create-company'],
    mutationFn: async (formValues: any) => {
      const response = await execute(
        'POST',
        companiesRequest.apiUser.endpoint,
        formValues
      )

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        console.error(error.message)
      }
      toast.error(errorMessage.title, {
        description: errorMessage.desc
      })
    },
    onSuccess: async data => {
      await queryClient.invalidateQueries(['get-all-companies'])
      toast.success(successMessage)
      if (parentForm) {
        parentForm('companyId', data.data)
      }
      handleDialog()
    }
  })
}

export const useDeleteCompany = (
  id: number,
  handleDialog: () => void,
  messages: { success: string; error: { title: string; desc: string } }
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['delete-company', id],
    mutationFn: async () => {
      const query = `${companiesRequest.apiUser.endpoint}/${id}`
      const response = await execute('DELETE', query)
      return response
    },
    onError: () => {
      const { title, desc } = messages.error
      toast.error(title, {
        description: desc
      })
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-all-companies'])
      toast.success(messages.success)
      handleDialog()
    }
  })
}

export const useEditCompanyById = (
  id: number,
  successMessage: string,
  errorMessage: { title: string; desc: string },
  handleDialog: () => void
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['edit-company'],
    mutationFn: async (formValues: any) => {
      const query = `${companiesRequest.apiUser.endpoint}/${id}`
      const response = await execute('PUT', query, {
        id,
        ...formValues
      })
      return response
    },
    onError: () => {
      toast.error(errorMessage.title, {
        description: errorMessage.desc
      })
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-all-companies'])
      toast.success(successMessage)
      handleDialog()
    }
  })
}

export const useApproveCompanyById = (id: number, name: string) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['approve-company', id],
    mutationFn: async () => {
      const endpoint = new URL(`${companiesRequest.apiUser.endpoint}/Approve`)
      const response = await execute('PUT', endpoint.toString(), {
        id,
        isApproved: true
      })
      return response
    },
    onError: () => {
      toast.error('Something went wrong!', {
        description: 'Please try again later.'
      })
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-all-companies'])
      toast.success(`Approved ${name}`)
    }
  })
}

export const useCreateEmployee = (
  successMessage: string,
  errorMessage: { title: string; desc: string },
  handleDialog: () => void,
  parentForm?: UseFormSetValue<any>
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['create-employee'],
    mutationFn: async (formValues: any) => {
      const response = await execute(
        'POST',
        employeesRequest.apiUser.endpoint,
        formValues
      )

      return response
    },
    onError: () => {
      toast.error(errorMessage.title, {
        description: errorMessage.desc
      })
    },
    onSuccess: async data => {
      await queryClient.invalidateQueries(['get-all-employees-by-company-id'])
      toast.success(successMessage)
      if (parentForm) {
        parentForm('employeeId', data.data)
      }
      handleDialog()
    }
  })
}

export const useEditEmployeeById = (
  id: number | undefined,
  successMessage: string,
  errorMessage: { title: string; desc: string },
  handleDialog: () => void
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['edit-employee'],
    mutationFn: async (formValues: any) => {
      const query = `${employeesRequest.apiUser.endpoint}/${id}`
      const response = await execute('PUT', query, {
        id,
        ...formValues
      })
      return response
    },
    onError: () => {
      toast.error(errorMessage.title, {
        description: errorMessage.desc
      })
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-all-employees'])
      toast.success(successMessage)
      handleDialog()
    }
  })
}

export const useCreateInternship = (
  successMessage: { title: string; desc: string },
  errorMessage: { title: string; desc: string },
  handleDialog: () => void
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['create-internship'],
    mutationFn: async (formValues: any) => {
      let insuranceType =
        formValues.insuranceType === 'school'
          ? InsuranceType.School
          : InsuranceType.Company

      const requestPayload = {
        ...formValues,
        insuranceType
      }

      const response = await execute(
        'POST',
        internshipRequest.apiUser.endpoint,
        requestPayload
      )

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-internships-by-userId'])
      toast.success(successMessage.title, {
        description: successMessage.desc
      })
      handleDialog()
    }
  })
}

export const useApproveInternship = (
  setIsDialogOpen: Dispatch<SetStateAction<boolean>>
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['approve-internship'],
    mutationFn: async (requestBody: {
      id: number
      isApproved: boolean
      comment?: string
    }) => {
      const query = new URL(
        `${internshipRequest.apiUser.endpoint}/ApproveByInternshipCommittee`
      )
      const response = await execute('PUT', query.toString(), requestBody)

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries(['get-all-internships'])
      toast.success('Successfully updated internship')
      setIsDialogOpen(prev => !prev)
    }
  })
}

export const useEditInternship = (
  handleDialog: Dispatch<SetStateAction<boolean>>
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['edit-internship'],
    mutationFn: async (requestBody: {
      id: number
      employeeId: number
      startDate: Date
      endDate: Date
      totalDays: number
    }) => {
      const requestUrl = new URL(
        `${internshipRequest.apiUser.endpoint}/${requestBody.id}`
      )
      const response = await execute('PUT', requestUrl.toString(), requestBody)

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries(['get-internships-by-userId'])
      toast.success('Internship edited successfully')
      handleDialog(prev => !prev)
    }
  })
}

export const useApproveCompany = (toastNotification: string) => {
  return useMutation({
    mutationKey: ['approve-company'],
    mutationFn: async (data: { id: number; isApproved: boolean }) => {
      const query = new URL(
        `${externalAccountRequest.apiUser.endpoint}/Approve-Company`
      )
      const response = await axios.post(query.toString(), data)

      return response
    },
    onError: (error: AxiosError) => {
      toast.error(error.name, {
        description: error.message
      })
    },
    onSuccess: () => {
      toast.success(toastNotification)
    }
  })
}

export const useUpdateUserInformation = (
  dialogState: Dispatch<SetStateAction<boolean>>
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['update-user'],
    mutationFn: async (formData: FormData) => {
      const tcKimlikNo = formData.get('tcKimlikNo')
      const birthyear = formData.get('birthyear')

      const endpoint = new URL(userRequest.apiUser.endpoint)
      const response = await execute('PUT', endpoint.toString(), {
        tcKimlikNo,
        birthyear
      })

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries(['get-user'])
      toast.success('User updated successfully!')
      dialogState(false)
    }
  })
}

export const useUploadFile = (
  internshipId: number,
  documentType: DocumentTypeName
) => {
  const { execute } = useFetchWithMsal()
  const queryClient = useQueryClient()

  return useMutation({
    mutationKey: ['upload-file'],
    mutationFn: async (formData: FormData) => {
      const query = new URL(fileRequest.apiUser.endpoint)
      query.searchParams.set('InternshipId', internshipId.toString())
      query.searchParams.set(
        'DocumentTypeId',
        documentType.valueOf().toString()
      )
      const response = await execute('POST', query.toString(), formData)

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onSuccess: () => {
      toast.success('Uploaded successfully!')
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    }
  })
}

export const useDownloadAsSpreadsheet = () => {
  const { execute } = useFetchWithMsal()
  return useMutation({
    mutationKey: ['download-as-spreadsheet'],
    mutationFn: async (internshipIds: number[]) => {
      const query = new URL(
        `${internshipRequest.apiUser.endpoint}/CreateSpreadsheet`
      )
      const response = await execute('POST', query.toString(), internshipIds)

      if (!response.succeeded) {
        throw new Error(response.Message)
      }

      return response
    },
    onError: error => {
      if (error instanceof Error) {
        toast.error(error.message)
      }
    },
    onSuccess: () => {
      toast.success('Downloaded successfully!')
    }
  })
}
