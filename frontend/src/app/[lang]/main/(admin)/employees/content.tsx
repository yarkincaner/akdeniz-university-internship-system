'use client'

import Paginator from '@/components/Paginator'
import Search from '@/components/Search'
import { DataTable } from '@/components/data-table/data-table'
import adminRouteWrapper from '@/components/route-wrappers/AdminRouteWrapper'
import { useGetAllEmployees } from '@/lib/queries'
import { RowSelectionState } from '@tanstack/react-table'
import { FC, Suspense, useState } from 'react'
import { Columns } from './columns'

type Props = {
  searchParams?: {
    SearchString?: string
    PageNumber?: string
    PageSize?: string
  }
}

const Content: FC<Props> = ({ searchParams }) => {
  const { data, isLoading, error } = useGetAllEmployees({
    searchString: searchParams?.SearchString,
    pageNumber: searchParams?.PageNumber,
    pageSize: searchParams?.PageSize
  })

  const [selectedRows, setSelectedRows] = useState<RowSelectionState>({})

  if (error) return <h1>Error</h1>
  return (
    <>
      <div className='flex items-center justify-between py-4'>
        <Suspense>
          <Search
            placeholder='Search by name...'
            paramName='SearchString'
            pageParamName='PageNumber'
          />
        </Suspense>
        <Suspense>
          <Paginator
            pageNumber={
              searchParams?.PageNumber ? searchParams.PageNumber : '1'
            }
            totalPages={data ? data.totalPages : 1}
            pageParamName='PageNumber'
            isLoading={isLoading}
          />
        </Suspense>
      </div>
      <DataTable
        columns={Columns}
        data={data ? data.data : []}
        isLoading={isLoading}
        rowSelection={selectedRows}
        setRowSelection={setSelectedRows}
      />
    </>
  )
}

export default adminRouteWrapper(Content)
