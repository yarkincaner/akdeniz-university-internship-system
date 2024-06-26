'use client'

import { InternshipSchema } from '@/lib/validators/internship'
import { zodResolver } from '@hookform/resolvers/zod'
import { FC, useState } from 'react'
import { useForm } from 'react-hook-form'
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
import { Button } from '../ui/button'
import { CalendarIcon } from 'lucide-react'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '../ui/form'
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover'
import { cn } from '@/lib/utils'
import { Combobox } from '../ui/combobox'
import { format } from 'date-fns'
import { Calendar } from '../ui/calendar'
import RegisterCompany from '../company/RegisterCompany'
import { type getDictionary } from '@/lib/i18n/dictionary'
import RegisterEmployee from '../employee/RegisterEmployee'
import { tr, enGB } from 'date-fns/locale'
import { Locale } from '../../lib/i18n/i18n.config'
import { z } from 'zod'
import Icons from '../Icons'
import { useGetAllCompanies, useGetEmployeesByCompanyId } from '@/lib/queries'
import { useCreateInternship } from '@/lib/mutations'
import { Input } from '../ui/input'
import { RadioGroup, RadioGroupItem } from '../ui/radio-group'
import { InsuranceType } from '@/types/internship'

type InternshipFormProps = {
  lang: Locale
  dictionary: Awaited<ReturnType<typeof getDictionary>>['addInternship']
  companyDictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['registerCompany']
  employeeDictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['registerEmployee']
  searchParams?: {
    company?: string
    companyPage?: string
    employee?: string
    employeePage?: string
  }
}

const InternshipForm: FC<InternshipFormProps> = ({
  lang,
  dictionary,
  companyDictionary,
  employeeDictionary,
  searchParams
}) => {
  const [open, setOpen] = useState<boolean>(false)
  const handleDialog = () => {
    setOpen(prev => !prev)
  }
  const dateLocale = lang === 'en' ? enGB : tr

  const formSchema = InternshipSchema(lang)
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      companyId: undefined,
      employeeId: undefined,
      startDate: undefined,
      endDate: undefined,
      totalDays: 30,
      insuranceType: 'school'
    }
  })

  const successMessage = dictionary.toast.internship.success
  const errorMessage = dictionary.toast.internship.error

  const { mutate: submitForm, isLoading } = useCreateInternship(
    successMessage,
    errorMessage,
    handleDialog
  )

  const { data: companiesQueryResult, isLoading: isCompaniesLoading } =
    useGetAllCompanies(open, {
      searchString: searchParams?.company,
      pageNumber: searchParams?.companyPage
    })

  const { data: employeesQueryResult, isLoading: isEmployeesLoading } =
    useGetEmployeesByCompanyId(
      form.watch().companyId !== undefined,
      form.watch().companyId,
      {
        searchString: searchParams?.employee,
        pageNumber: searchParams?.employeePage
      },
      {
        title: dictionary.toast.employee.error.title,
        desc: dictionary.toast.employee.error.desc
      }
    )

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant='ghost' className='w-fit pl-1'>
          <Icons.plus className='mr-2 size-4' /> {dictionary.title}
        </Button>
      </DialogTrigger>
      <DialogContent className='max-h-screen overflow-y-scroll md:max-w-screen-md'>
        <DialogHeader>
          <DialogTitle>{dictionary.title}</DialogTitle>
          <DialogDescription>{dictionary.desc}</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form
            id='createInternship'
            onSubmit={e => {
              e.stopPropagation()
              return form.handleSubmit(event => submitForm(event))(e)
            }}
            className='space-y-4'
          >
            <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
              <FormField
                control={form.control}
                name='companyId'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.companyLabel}</FormLabel>
                    <Combobox
                      onChange={value => {
                        field.onChange(value)
                        form.setValue('employeeId', 0)
                      }}
                      variant='company'
                      options={companiesQueryResult?.data!}
                      disabled={!companiesQueryResult}
                      isLoading={isCompaniesLoading}
                      value={field.value ?? ''}
                      addItemComponent={
                        <RegisterCompany
                          form={form.setValue}
                          dictionary={companyDictionary}
                        />
                      }
                      notFoundMessage={dictionary.fields.companyNotFound}
                      placeholder={dictionary.fields.companyPlaceholder}
                      searchParam='company'
                      pageParam='companyPage'
                      pageNumber={
                        searchParams?.companyPage
                          ? searchParams.companyPage
                          : '1'
                      }
                      totalPages={
                        companiesQueryResult
                          ? companiesQueryResult.totalPages
                          : 1
                      }
                    />
                    <FormMessage />
                  </FormItem>
                )}
              />
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
                      disabled={form.getValues('companyId') === undefined}
                      isLoading={isEmployeesLoading}
                      value={field.value ?? ''}
                      addItemComponent={
                        <RegisterEmployee
                          companyId={form.watch().companyId}
                          companyName={
                            companiesQueryResult?.data?.find(
                              company => company.id === form.watch().companyId
                            )?.name!
                          }
                          parentForm={form.setValue}
                          dictionary={employeeDictionary}
                        />
                      }
                      notFoundMessage={dictionary.fields.employeeNotFound}
                      placeholder={dictionary.fields.employerPlaceholder}
                      searchParam='employee'
                      pageParam='employeePage'
                      pageNumber={
                        searchParams?.employeePage
                          ? searchParams.employeePage
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
                              format(field.value, 'PPP', { locale: dateLocale })
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
                              format(field.value, 'PPP', { locale: dateLocale })
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
            <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
              <FormField
                control={form.control}
                name='totalDays'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>{dictionary.fields.totalDaysLabel}</FormLabel>
                    <FormControl>
                      <Input
                        type='number'
                        placeholder={dictionary.fields.totalDaysPlaceholder}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='insuranceType'
                render={({ field }) => (
                  <FormItem className='w-full md:basis-1/2'>
                    <FormLabel>
                      {dictionary.fields.insuranceTypeLabel}
                    </FormLabel>
                    <FormControl>
                      <RadioGroup
                        onValueChange={field.onChange}
                        defaultValue={field.value}
                        className='flex items-center space-x-2'
                      >
                        <FormItem className='flex items-center space-x-2 space-y-0'>
                          <FormControl>
                            <RadioGroupItem value={'school'} />
                          </FormControl>
                          <FormLabel>
                            {dictionary.fields.insuranceTypeSchool}
                          </FormLabel>
                        </FormItem>
                        <FormItem className='flex items-center space-x-2 space-y-0'>
                          <FormControl>
                            <RadioGroupItem value={'company'} />
                          </FormControl>
                          <FormLabel>
                            {dictionary.fields.insuranceTypeCompany}
                          </FormLabel>
                        </FormItem>
                      </RadioGroup>
                    </FormControl>
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
                form='createInternship'
                isLoading={isLoading}
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

export default InternshipForm
