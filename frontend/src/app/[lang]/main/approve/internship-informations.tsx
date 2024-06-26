'use client'

import Loader from '@/components/Loader'
import { useDictionary } from '@/components/dictionary-provider'
import { Button } from '@/components/ui/button'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import { useApproveCompany } from '@/lib/mutations'
import { useGetInternshipByToken } from '@/lib/queries'
import { FC } from 'react'

type Props = {
  token: string
}

const InternshipInformations: FC<Props> = ({ token }) => {
  const { approvePageEmployee } = useDictionary()

  const { data, isLoading } = useGetInternshipByToken(token)

  const {
    mutate: approveCompany,
    isLoading: isApproveCompanyLoading,
    data: mutatedData
  } = useApproveCompany(approvePageEmployee.response)

  if (isLoading || !data) return <Loader />

  return (
    <>
      <Table className='rounded-lg bg-background p-4'>
        <TableHeader>
          <TableRow>
            <TableHead>{approvePageEmployee.firstName}</TableHead>
            <TableHead>{approvePageEmployee.lastName}</TableHead>
            <TableHead>{approvePageEmployee.studentEmail}</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell>{data.firstName}</TableCell>
            <TableCell>{data.lastName}</TableCell>
            <TableCell>{data.email}</TableCell>
          </TableRow>
        </TableBody>
      </Table>

      <Table className='rounded-lg bg-background p-4'>
        <TableHeader>
          <TableRow>
            <TableHead>{approvePageEmployee.birthyear}</TableHead>
            <TableHead>{approvePageEmployee.tcKimlik}</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell>{data.birthYear}</TableCell>
            <TableCell>{data.tcKimlikNo}</TableCell>
          </TableRow>
        </TableBody>
      </Table>

      <Table className='rounded-lg bg-background p-4'>
        <TableHeader>
          <TableRow>
            <TableHead>{approvePageEmployee.companyName}</TableHead>
            <TableHead>{approvePageEmployee.employeeName}</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell>{data.companyName}</TableCell>
            <TableCell>{data.employeeName}</TableCell>
          </TableRow>
        </TableBody>
      </Table>

      <Table className='rounded-lg bg-background p-4'>
        <TableHeader>
          <TableRow>
            <TableHead>{approvePageEmployee.startDate}</TableHead>
            <TableHead>{approvePageEmployee.endDate}</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell>
              {new Date(data.startDate).toLocaleDateString()}
            </TableCell>
            <TableCell>{new Date(data.endDate).toLocaleDateString()}</TableCell>
          </TableRow>
        </TableBody>
      </Table>

      {mutatedData ? (
        <></>
      ) : (
        <div className='grid grid-cols-1 gap-4 sm:grid-cols-2 sm:gap-24'>
          <Button
            onClick={() => approveCompany({ id: data.id, isApproved: true })}
            isLoading={isApproveCompanyLoading}
          >
            {approvePageEmployee.accept}
          </Button>
          <Button
            onClick={() => approveCompany({ id: data.id, isApproved: false })}
            isLoading={isApproveCompanyLoading}
            variant={'destructive'}
          >
            {approvePageEmployee.decline}
          </Button>
        </div>
      )}
    </>
  )
}

export default InternshipInformations
