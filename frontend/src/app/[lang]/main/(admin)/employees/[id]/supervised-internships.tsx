'use client'

import { DataTable } from '@/components/data-table/data-table'
import { Internship } from '@/types/internship'
import { FC, useState } from 'react'
import { Columns } from './columns'
import { RowSelectionState } from '@tanstack/react-table'

type Props = {
  internships: Internship[]
}

const SupervisedInternships: FC<Props> = ({ internships }) => {
  const [selectedRows, setSelectedRows] = useState<RowSelectionState>({})
  return (
    <DataTable
      columns={Columns}
      data={internships}
      isLoading={false}
      rowSelection={selectedRows}
      setRowSelection={setSelectedRows}
    />
  )
}

export default SupervisedInternships
