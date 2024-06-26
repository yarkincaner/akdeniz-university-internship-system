'use client'
import { Check, ChevronsUpDown } from 'lucide-react'
import { Command, CommandGroup, CommandItem, CommandList } from './command'
import { cn } from '@/lib/utils'
import React, { Suspense } from 'react'
import { Popover, PopoverContent, PopoverTrigger } from './popover'
import { Button } from './button'
import { FormControl } from './form'
import Search from '../Search'
import Loader from '../Loader'
import Paginator from '../Paginator'
import { Company } from '@/types/company'
import { Employee } from '@/types/employee'

const ComboboxItem = React.forwardRef<
  React.ElementRef<typeof CommandItem>,
  React.ComponentPropsWithoutRef<typeof CommandItem> & {
    label: string
    isSelected: boolean
  }
>(({ className, label, onSelect, isSelected, ...props }, ref) => (
  <CommandItem
    ref={ref}
    className={cn('gap-2 text-sm', className)}
    onSelect={onSelect}
    {...props}
  >
    <span>{label}</span>
    <Check
      className={`ml-auto h-4 w-4 ${isSelected ? 'opacity-100' : 'opacity-0'}`}
    />
  </CommandItem>
))
ComboboxItem.displayName = 'ComboboxItem'

type CompanyProps = {
  variant: 'company'
  options: Company[]
}

type EmployeeProps = {
  variant: 'employee'
  options: Employee[]
}

type ComboboxProps = {
  disabled?: boolean
  isLoading: boolean
  value: number
  onChange: (value: number) => void
  placeholder: string
  addItemComponent: React.ReactNode
  notFoundMessage: string
  searchParam: string
  pageParam: string
  pageNumber: string | null
  totalPages: number
} & (CompanyProps | EmployeeProps)

const Combobox = (props: ComboboxProps) => {
  const [open, setOpen] = React.useState(false)
  const [selectedItemLabel, setSelectedItemLabel] = React.useState<string>('')

  const renderOptions = React.useMemo(() => {
    if (props.options && props.options.length > 0) {
      if (props.variant === 'company') {
        return props.options.map(option => (
          <ComboboxItem
            key={option.id}
            onSelect={() => {
              props.onChange(option.id)
              setOpen(false)
            }}
            label={option.name}
            isSelected={option.id === props.value}
          />
        ))
      } else {
        return props.options.map(option => (
          <ComboboxItem
            key={option.id}
            onSelect={() => {
              props.onChange(option.id)
              setOpen(false)
            }}
            label={`${option.firstName} ${option.lastName}`}
            isSelected={option.id === props.value}
          />
        ))
      }
    }

    return (
      <div className='flex flex-col justify-center gap-2 p-4 text-center text-sm'>
        {props.notFoundMessage}
        {props.addItemComponent}
      </div>
    )
  }, [
    props.options,
    props.value,
    props.onChange,
    setOpen,
    props.addItemComponent
  ])

  React.useEffect(() => {
    if (props.variant === 'company') {
      const company = props.options?.find(option => option.id === props.value)
      if (company) {
        setSelectedItemLabel(company?.name)
      } else {
        setSelectedItemLabel('')
      }
    } else {
      const employee = props.options?.find(option => option.id === props.value)
      if (employee) {
        setSelectedItemLabel(`${employee?.firstName} ${employee?.lastName}`)
      } else {
        setSelectedItemLabel('')
      }
    }
  }, [props.value])

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <FormControl>
          <Button
            variant='outline'
            role='combobox'
            className={cn('w-full justify-between')}
            disabled={props.disabled}
          >
            <span>
              {selectedItemLabel.length > 0
                ? selectedItemLabel
                : props.placeholder}
            </span>
            <ChevronsUpDown className={`${props.disabled ? 'hidden' : ''}`} />
          </Button>
        </FormControl>
      </PopoverTrigger>
      <PopoverContent className='w-[300px] p-0'>
        <Command>
          <CommandList>
            <Suspense>
              <Search
                placeholder={props.placeholder}
                paramName={props.searchParam}
                pageParamName={props.pageParam}
                className='rounded-none border-x-0 border-t-0 focus-visible:ring-0'
              />
            </Suspense>
            <CommandGroup>
              {props.isLoading ? <Loader /> : renderOptions}
            </CommandGroup>
          </CommandList>
        </Command>
        <Suspense>
          <Paginator
            pageParamName={props.pageParam}
            pageNumber={props.pageNumber ? props.pageNumber : '1'}
            totalPages={props.totalPages}
            isLoading={props.isLoading}
          />
        </Suspense>
      </PopoverContent>
    </Popover>
  )
}

export { Combobox }
