import StatusBadge from '@/components/internship/StatusBadge'
import { Checkbox } from '@/components/ui/checkbox'
import { Internship } from '@/types/internship'
import { ColumnDef } from '@tanstack/react-table'

export const Columns: ColumnDef<Internship>[] = [
  {
    id: 'select',
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && 'indeterminate')
        }
        onCheckedChange={value => table.toggleAllPageRowsSelected(!!value)}
        aria-label='Select all'
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={value => row.toggleSelected(!!value)}
        aria-label='Select row'
      />
    ),
    enableSorting: false,
    enableHiding: false
  },
  {
    accessorKey: 'id',
    header: 'Id'
  },
  {
    accessorKey: 'userEmail',
    header: 'Student Number',
    cell: function Cell({ row }) {
      const { userEmail } = row.original
      const studentId = userEmail.substring(0, userEmail.indexOf('@'))
      return <>{studentId}</>
    }
  },
  {
    accessorKey: 'userName',
    header: 'Student Name'
  },
  {
    accessorKey: 'startDate',
    header: 'Start Date'
  },
  {
    accessorKey: 'endDate',
    header: 'End Date'
  },
  {
    accessorKey: 'totalDays',
    header: 'Total Days'
  },
  {
    accessorKey: 'insuranceType',
    header: 'Insurance Type',
    cell: function Cell({ row }) {
      const { insuranceType } = row.original
      switch (insuranceType) {
        case 1:
          return <span>School</span>
        case 2:
          return <span>Company</span>
        default:
          return <span>Undefined insurance type</span>
      }
    }
  },
  {
    accessorKey: 'statusName',
    header: 'Status',
    cell: function Cell({ row }) {
      const { statusName } = row.original
      return <StatusBadge status={statusName} />
    }
  },
  {
    accessorKey: 'comment',
    header: 'Status comment',
    cell: function Cell({ row }) {
      const { comment } = row.original
      return (
        <div className='flex min-w-0 max-w-[300px] items-center'>
          <span className='max-w-fit truncate'>{comment}</span>
        </div>
      )
    }
  }
]
