'use client'

import Icons from '@/components/Icons'
import StatusBadge from '@/components/internship/StatusBadge'
import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { useApproveInternship } from '@/lib/mutations'
import { Internship } from '@/types/internship'
import { ColumnDef } from '@tanstack/react-table'
import { MoreHorizontal } from 'lucide-react'
import Link from 'next/link'
import { useState } from 'react'

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
    accessorKey: 'companyName',
    header: 'Company Name'
  },
  {
    accessorKey: 'employeeName',
    header: 'Supervisor Name',
    cell: function Cell({ row }) {
      const { employeeId, employeeName } = row.original
      return (
        <Button variant={'link'} asChild>
          <Link href={`/main/employees/${employeeId}`}>{employeeName}</Link>
        </Button>
      )
    }
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
  },
  {
    id: 'actions',
    cell: function Cell({ row }) {
      const { id } = row.original
      const [isDialogOpen, setIsDialogOpen] = useState<boolean>(false)
      const { mutate: approveInternship, isLoading } =
        useApproveInternship(setIsDialogOpen)
      const handleForm = (formData: FormData) => {
        const comment = formData.get('comment') as string
        approveInternship({
          id,
          isApproved: false,
          comment
        })
      }

      return (
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant={'ghost'} className='size-4 p-0'>
                <span className='sr-only'>Open menu</span>
                <MoreHorizontal className='size-4' />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align='end'>
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              <DropdownMenuItem
                onClick={() => approveInternship({ id: id, isApproved: true })}
                className='text-success'
              >
                <Icons.accepted className='mr-1.5 size-4' /> Approve
              </DropdownMenuItem>
              <DropdownMenuItem className='text-destructive'>
                <DialogTrigger className='flex items-center hover:cursor-default'>
                  <Icons.declined className='mr-1.5 size-4' /> Decline
                </DialogTrigger>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Decline internship application</DialogTitle>
              <DialogDescription>
                Please provide the reason why you are declining the internship
                application.
              </DialogDescription>
            </DialogHeader>
            <form id='decline-internship-form' action={handleForm}>
              <div className='space-y-2'>
                <Label className='w-full'>Comment</Label>
                <Textarea
                  name='comment'
                  placeholder='Given supervisor does not exists...'
                  className='w-full resize-none'
                  required
                  minLength={10}
                />
              </div>
            </form>
            <DialogFooter>
              <DialogClose asChild>
                <Button variant={'secondary'}>Cancel</Button>
              </DialogClose>
              <Button
                type='submit'
                form='decline-internship-form'
                variant={'destructive'}
                isLoading={isLoading}
              >
                Decline Internship
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      )
    }
  }
]
