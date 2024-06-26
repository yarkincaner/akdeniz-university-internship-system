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
import { companiesRequest } from '@/config/authConfig'
import useFetchWithMsal from '@/hooks/useFetchWithMsal'
import { ColumnDef } from '@tanstack/react-table'
import { ArrowUpDown, MoreHorizontal, Pencil } from 'lucide-react'
import { useParams } from 'next/navigation'
import { useState } from 'react'
import { Locale } from '@/lib/i18n/i18n.config'
import { getDictionary } from '@/lib/i18n/dictionary'
import { useQuery } from 'react-query'
import Loader from '@/components/Loader'
import EditCompanyFields from '@/components/company/EditCompanyFields'
import { cn } from '@/lib/utils'
import Icons from '@/components/Icons'
import { useApproveCompanyById } from '@/lib/mutations'
import { Company } from '@/types/company'

// TODO: Add columns CreatedBy CreatedDate etc.
export const Columns: ColumnDef<Company>[] = [
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
    accessorKey: 'name',
    header: ({ column }) => {
      return (
        <Button
          variant='ghost'
          className='pl-0'
          onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
        >
          Name
          <ArrowUpDown className='ml-2 h-4 w-4' />
        </Button>
      )
    }
  },
  {
    accessorKey: 'address',
    header: 'Address',
    cell: ({ row }) => {
      return (
        <div className='flex min-w-0 max-w-[100px] md:max-w-[400px]'>
          <span className='truncate'>{row.getValue('address')}</span>
        </div>
      )
    }
  },
  {
    accessorKey: 'serviceArea',
    header: ({ column }) => {
      return (
        <Button
          variant='ghost'
          className='pl-0'
          onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
        >
          Service Area
          <ArrowUpDown className='ml-2 h-4 w-4' />
        </Button>
      )
    }
  },
  {
    accessorKey: 'phone',
    header: 'Phone'
  },
  {
    accessorKey: 'fax',
    header: 'Fax'
  },
  {
    accessorKey: 'taxNumber',
    header: 'Tax'
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
    accessorKey: 'website',
    header: 'Website',
    cell: function Cell({ row }) {
      const { website } = row.original
      return (
        <a
          href={website}
          className={cn(buttonVariants({ variant: 'link' }), 'px-0')}
          rel='noopener noreferrer'
          target='_blank'
        >
          {website}
        </a>
      )
    }
  },
  {
    accessorKey: 'approvedBy',
    header: 'Approved By'
  },
  {
    accessorKey: 'isApproved',
    header: () => {
      return <p className='text-nowrap'>Is Approved</p>
    },
    cell: function Cell({ row }) {
      if (row.original.isApproved) {
        return (
          <div className='flex w-full items-center justify-center rounded-full bg-success p-2 text-success-foreground'>
            <Icons.accepted className='mr-2 size-4' />
            Approved
          </div>
        )
      }

      return (
        <div className='flex w-full items-center justify-center rounded-full bg-destructive p-2 text-destructive-foreground'>
          <Icons.declined className='mr-2 size-4' />
          Declined
        </div>
      )
    }
  },
  {
    id: 'actions',
    cell: function Cell({ row }) {
      const company = row.original
      const { execute, isLoading } = useFetchWithMsal()
      const [open, setOpen] = useState<boolean>(false)
      const { lang } = useParams<{ lang: Locale }>()

      const handleDialog = () => {
        setOpen(prev => !prev)
      }

      const handleDelete = async () => {
        try {
          const query = `${companiesRequest.apiUser.endpoint}/${company.id}`
          const response = await execute('DELETE', query)
          return response
        } catch (e) {
          console.error(e)
        }
      }

      const { mutate: approveCompany, isLoading: isApproveLoading } =
        useApproveCompanyById(company.id, company.name)

      const { data: dictionary, isLoading: isDictionaryLoading } = useQuery({
        queryKey: ['get-company-dictionary', lang],
        queryFn: async () => {
          const {
            registerCompany,
            page: {
              main: { companies }
            }
          } = await getDictionary(lang)
          return {
            registerCompany,
            companies
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
              {dictionary?.companies.dropdownLabel}
            </DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() =>
                navigator.clipboard.writeText(company.id.toString())
              }
            >
              <Icons.copy className='mr-2 size-4' />
              {dictionary?.companies.copyCompanyId}
            </DropdownMenuItem>
            <DropdownMenuItem
              onClick={() =>
                navigator.clipboard.writeText(company.address.toString())
              }
            >
              <Icons.copy className='mr-2 size-4' />
              {dictionary?.companies.copyCompanyAddress}
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <Dialog open={open} onOpenChange={handleDialog}>
              <DialogTrigger asChild>
                <DropdownMenuItem onSelect={e => e.preventDefault()}>
                  <Icons.edit className='mr-2 size-4' />
                  {dictionary?.companies.editCompany}
                </DropdownMenuItem>
              </DialogTrigger>
              <DialogContent className='max-h-screen overflow-y-scroll md:max-w-screen-md'>
                <DialogHeader>
                  <DialogTitle>{dictionary?.companies.editCompany}</DialogTitle>
                  <DialogDescription>
                    {dictionary?.companies.dialogDesc}
                  </DialogDescription>
                </DialogHeader>
                <EditCompanyFields
                  handleDialog={handleDialog}
                  values={company}
                  dictionary={dictionary!.registerCompany.fields}
                />
              </DialogContent>
            </Dialog>
            <DropdownMenuItem
              className='text-success'
              onSelect={e => {
                e.preventDefault()
                approveCompany()
              }}
            >
              <Icons.accepted className='mr-2 size-4' />
              Approve
            </DropdownMenuItem>
            <AlertDialog>
              <AlertDialogTrigger asChild>
                <DropdownMenuItem
                  className='cursor-pointer text-destructive'
                  onSelect={e => e.preventDefault()}
                >
                  <Icons.delete className='mr-2 size-4' />
                  {dictionary?.companies.deleteCompany}
                </DropdownMenuItem>
              </AlertDialogTrigger>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>
                    {dictionary?.companies.alertTitle}
                  </AlertDialogTitle>
                  <AlertDialogDescription>
                    {dictionary?.companies.alertDesc}
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>
                    {dictionary?.companies.alertCancel}
                  </AlertDialogCancel>
                  <AlertDialogAction
                    onClick={handleDelete}
                    disabled={isLoading}
                  >
                    {dictionary?.companies.alertDelete}
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
