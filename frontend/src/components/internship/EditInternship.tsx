'use client'

import { Locale } from '@/lib/i18n/i18n.config'
import { useEditInternship } from '@/lib/mutations'
import { useGetAllCompanies, useGetEmployeesByCompanyId } from '@/lib/queries'
import { cn } from '@/lib/utils'
import { EditInternshipSchema } from '@/lib/validators/internship'
import { Internship } from '@/types/internship'
import { zodResolver } from '@hookform/resolvers/zod'
import { format } from 'date-fns'
import { enGB, tr } from 'date-fns/locale'
import { CalendarIcon } from 'lucide-react'
import { useParams, useSearchParams } from 'next/navigation'
import { FC, useState } from 'react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import Icons from '../Icons'
import RegisterCompany from '../company/RegisterCompany'
import { useDictionary } from '../dictionary-provider'
import RegisterEmployee from '../employee/RegisterEmployee'
import { Button } from '../ui/button'
import { Calendar } from '../ui/calendar'
import { Combobox } from '../ui/combobox'
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '../ui/dialog'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '../ui/form'
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover'
import { Input } from '../ui/input'

type Props = {
  values: Internship
}

const EditInternship: FC<Props> = ({ values }) => {
  const [open, setOpen] = useState<boolean>(false)
  const { lang } = useParams<{ lang: Locale }>()
  const dateLocale = lang === 'en' ? enGB : tr

  const searchParams = useSearchParams()

  const {
    addInternship: dictionary,
    registerCompany: companyDictionary,
    registerEmployee: employeeDictionary
  } = useDictionary()

  const formSchema = EditInternshipSchema(lang)
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      employeeId: values.employeeId,
      endDate: new Date(values.endDate),
      startDate: new Date(values.startDate),
      totalDays: values.totalDays
    }
  })

  const { data: employeesQueryResult, isLoading: isEmployeesLoading } =
    useGetEmployeesByCompanyId(
      true,
      values.companyId,
      {
        searchString: searchParams.get('employee'),
        pageNumber: searchParams.get('employeePage')
      },
      {
        title: dictionary.toast.employee.error.title,
        desc: dictionary.toast.employee.error.desc
      }
    )

  const { mutate: editInternship, isLoading: isEditInternshipLoading } =
    useEditInternship(setOpen)

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button
          size={'icon'}
          className='absolute -right-2.5 -top-2.5 z-10 rounded-full'
          onClick={() => setOpen(prev => !prev)}
        >
          <Icons.edit className='size-4' />
        </Button>
      </DialogTrigger>
      <DialogContent className='max-h-screen overflow-y-scroll md:max-w-screen-md'>
        <DialogHeader>
          <DialogTitle>{dictionary.title}</DialogTitle>
          <DialogDescription>{dictionary.desc}</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form
            id='edit-internship'
            className='space-y-4'
            onSubmit={e => {
              e.stopPropagation()
              return form.handleSubmit(event =>
                // console.log('edit internship', event)
                editInternship({
                  id: values.id,
                  ...event
                })
              )(e)
            }}
          >
            <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
              <FormField
                control={form.control}
                name='employeeId'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.employerLabel}</FormLabel>
                    <Combobox
                      onChange={field.onChange}
                      variant='employee'
                      options={employeesQueryResult?.data!}
                      isLoading={isEmployeesLoading}
                      value={field.value ?? ''}
                      addItemComponent={
                        <RegisterEmployee
                          companyId={values.companyId}
                          companyName={values.companyName}
                          parentForm={form.setValue}
                          dictionary={employeeDictionary}
                        />
                      }
                      notFoundMessage={dictionary.fields.employeeNotFound}
                      placeholder={dictionary.fields.employerPlaceholder}
                      searchParam='employee'
                      pageParam='employeePage'
                      pageNumber={
                        searchParams.get('employeePage')
                          ? searchParams.get('employeePage')
                          : '1'
                      }
                      totalPages={
                        employeesQueryResult
                          ? employeesQueryResult.totalPages
                          : 1
                      }
                    />
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='totalDays'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.totalDaysLabel}</FormLabel>
                    <FormControl>
                      <Input
                        type='number'
                        defaultValue={field.value}
                        placeholder={dictionary.fields.totalDaysPlaceholder}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
              <FormField
                control={form.control}
                name='startDate'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.startDateLabel}</FormLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <FormControl>
                          <Button
                            variant={'outline'}
                            className={cn(
                              'w-full pl-3 text-left font-normal',
                              !field.value && 'text-muted-foreground'
                            )}
                          >
                            {field.value ? (
                              format(field.value, 'PPP', {
                                locale: dateLocale
                              })
                            ) : (
                              <span>
                                {dictionary.fields.startDatePlaceholder}
                              </span>
                            )}
                            <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                      <PopoverContent className='w-auto p-0' align='start'>
                        <Calendar
                          mode='single'
                          selected={field.value}
                          onSelect={date => {
                            if (date) {
                              const utcDate = new Date(
                                Date.UTC(
                                  date.getFullYear(),
                                  date.getMonth(),
                                  date.getDate()
                                )
                              )
                              field.onChange(utcDate)
                            } else {
                              field.onChange(date)
                            }
                          }}
                          locale={dateLocale}
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='endDate'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.endDateLabel}</FormLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <FormControl>
                          <Button
                            variant={'outline'}
                            className={cn(
                              'w-full pl-3 text-left font-normal',
                              !field.value && 'text-muted-foreground'
                            )}
                          >
                            {field.value ? (
                              format(field.value, 'PPP', {
                                locale: dateLocale
                              })
                            ) : (
                              <span>
                                {dictionary.fields.endDatePlaceholder}
                              </span>
                            )}
                            <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                      <PopoverContent className='w-auto p-0' align='start'>
                        <Calendar
                          mode='single'
                          selected={field.value}
                          onSelect={date => {
                            if (date) {
                              const utcDate = new Date(
                                Date.UTC(
                                  date.getFullYear(),
                                  date.getMonth(),
                                  date.getDate()
                                )
                              )
                              field.onChange(utcDate)
                            } else {
                              field.onChange(date)
                            }
                          }}
                          locale={dateLocale}
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <DialogFooter className='gap-2 sm:justify-between'>
              <DialogClose asChild>
                <Button type='button' variant='secondary'>
                  {dictionary.fields.dialogClose}
                </Button>
              </DialogClose>
              <Button
                type='submit'
                form='edit-internship'
                isLoading={isEditInternshipLoading}
              >
                {dictionary.fields.dialogSave}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}

export default EditInternship
