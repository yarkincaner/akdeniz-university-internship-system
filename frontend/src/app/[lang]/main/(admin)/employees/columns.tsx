'use client'

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger
} from '@/components/ui/alert-dialog'
import { Button, buttonVariants } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { employeesRequest } from '@/config/authConfig'
import useFetchWithMsal from '@/hooks/useFetchWithMsal'
import { ColumnDef } from '@tanstack/react-table'
import { ArrowUpDown, MoreHorizontal } from 'lucide-react'
import { useState } from 'react'
import { Locale } from '@/lib/i18n/i18n.config'
import { useParams } from 'next/navigation'
import { useQuery } from 'react-query'
import { getDictionary } from '@/lib/i18n/dictionary'
import Loader from '@/components/Loader'
import EditEmployeeFields from '@/components/employee/EditEmployeeFields'
import { cn } from '@/lib/utils'
import Icons from '@/components/Icons'
import { Employee } from '@/types/employee'

export const Columns: ColumnDef<Employee>[] = [
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
    header: ({ column }) => {
      return (
        <Button
          variant='ghost'
          className='pl-0'
          onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
        >
          ID
          <ArrowUpDown className='ml-2 h-4 w-4' />
        </Button>
      )
    }
  },
  {
    accessorKey: 'firstName',
    header: 'First name'
  },
  {
    accessorKey: 'lastName',
    header: 'Last name'
  },
  {
    accessorKey: 'email',
    header: 'Email',
    cell: function Cell({ row }) {
      const { email } = row.original
      return (
        <a
          href={`mailto:${email}`}
          className={cn(buttonVariants({ variant: 'link' }), 'px-0')}
        >
          {email}
        </a>
      )
    }
  },
  {
    accessorKey: 'title',
    header: 'Title'
  },
  {
    accessorKey: 'companyName',
    header: 'Company'
  },
  {
    id: 'actions',
    cell: function Cell({ row }) {
      const employee = row.original
      const { execute, isLoading } = useFetchWithMsal()
      const [open, setOpen] = useState<boolean>(false)
      const { lang } = useParams<{ lang: Locale }>()

      const handleDialog = () => {
        setOpen(prev => !prev)
      }

      const handleDelete = async () => {
        try {
          const query = `${employeesRequest.apiUser.endpoint}/${employee.id}`
          const response = await execute('DELETE', query)
          return response
        } catch (e) {
          console.error(e)
        }
      }

      const { data: dictionary, isLoading: isDictionaryLoading } = useQuery({
        queryKey: ['get-employee-dictionary', lang],
        queryFn: async () => {
          const {
            registerEmployee,
            page: {
              main: { employees }
            }
          } = await getDictionary(lang)
          return {
            registerEmployee,
            employees
          }
        }
      })

      if (isDictionaryLoading) return <Loader />
      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant='ghost' className='h-8 w-8 p-0'>
              <span className='sr-only'>Open menu</span>
              <MoreHorizontal className='h-4 w-4' />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align='end'>
            <DropdownMenuLabel>
              {dictionary?.employees.dropdownLabel}
            </DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() =>
                navigator.clipboard.writeText(employee.id.toString())
              }
            >
              <Icons.copy className='mr-2 size-4' />
              {dictionary?.employees.copyEmployeeId}
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem>
              {dictionary?.employees.viewEmployee}
            </DropdownMenuItem>
            <Dialog open={open} onOpenChange={handleDialog}>
              <DialogTrigger asChild>
                <DropdownMenuItem
                  className='gap-2'
                  onSelect={e => e.preventDefault()}
                >
                  <Icons.edit className='mr-2 size-4' />
                  {dictionary?.employees.editEmployee}
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className='max-h-screen overflow-y-scroll md:max-w-screen-md'>
                <DialogHeader>
                  <DialogTitle>
                    {employee.companyName} -{' '}
                    {dictionary?.employees.editEmployee}
                  </DialogTitle>
                  <DialogDescription>
                    {dictionary?.employees.dialogDesc}
                  </DialogDescription>
                </DialogHeader>
                <EditEmployeeFields
                  handleDialog={handleDialog}
                  values={employee}
                  dictionary={dictionary!.registerEmployee.fields}
                />
              </DialogContent>
            </Dialog>
            <DropdownMenuSeparator />
            <AlertDialog>
              <AlertDialogTrigger asChild>
                <DropdownMenuItem
                  className='gap-2'
                  onSelect={e => e.preventDefault()}
                >
                  <Icons.delete className='mr-2 size-4' />
                  {dictionary?.employees.deleteEmployee}
                </DropdownMenuItem>
              </AlertDialogTrigger>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>
                    {dictionary?.employees.alertTitle}
                  </AlertDialogTitle>
                  <AlertDialogDescription>
                    {dictionary?.employees.alertDesc}
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>
                    {dictionary?.employees.alertCancel}
                  </AlertDialogCancel>
                  <AlertDialogAction
                    onClick={handleDelete}
                    disabled={isLoading}
                  >
                    {dictionary?.employees.alertDelete}
                  </AlertDialogAction>
                </AlertDialogFooter>
              </AlertDialogContent>
            </AlertDialog>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    }
  }
]
