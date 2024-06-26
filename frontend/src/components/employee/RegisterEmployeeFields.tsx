'use client'

import { EmployeeSchema } from '@/lib/validators/employee'
import { zodResolver } from '@hookform/resolvers/zod'
import { FC } from 'react'
import { UseFormSetValue, useForm } from 'react-hook-form'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '../ui/form'
import { Input } from '../ui/input'
import { DialogClose, DialogFooter } from '../ui/dialog'
import { Button } from '../ui/button'
import { getDictionary } from '@/lib/i18n/dictionary'
import { Locale } from '../../lib/i18n/i18n.config'
import { useParams } from 'next/navigation'
import { z } from 'zod'
import { useCreateEmployee } from '@/lib/mutations'

type Props = {
  companyId: number
  handleDialog: () => void
  parentForm?: UseFormSetValue<any>
  dictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['registerEmployee']['fields']
}

const RegisterEmployeeFields: FC<Props> = ({
  companyId,
  handleDialog,
  parentForm,
  dictionary
}) => {
  const { lang } = useParams<{ lang: Locale }>()
  const formSchema = EmployeeSchema(lang)
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      firstName: undefined,
      lastName: undefined,
      title: undefined,
      email: undefined,
      companyId: companyId
    }
  })

  const { title: successMessage } = dictionary.toast.success
  const errorMessage = dictionary.toast.error

  const { mutate: createEmployee, isLoading: isCreateLoading } =
    useCreateEmployee(successMessage, errorMessage, handleDialog, parentForm)

  return (
    <Form {...form}>
      <form
        id='registerEmployee'
        onSubmit={e => {
          e.stopPropagation()
          return form.handleSubmit(event => createEmployee(event))(e)
        }}
        className='space-y-4'
      >
        <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
          <FormField
            control={form.control}
            name='firstName'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.firstName}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.firstNamePlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='lastName'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.lastName}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.lastNamePlaceholder}
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
            name='title'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.title}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.titlePlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='email'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.email}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.emailPlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        <DialogFooter className='gap-2 sm:justify-between'>
          <DialogClose asChild>
            <Button type='button' variant='secondary'>
              {dictionary.dialogClose}
            </Button>
          </DialogClose>
          <Button
            type='submit'
            isLoading={isCreateLoading}
            form='registerEmployee'
          >
            {dictionary.dialogSave}
          </Button>
        </DialogFooter>
      </form>
    </Form>
  )
}

export default RegisterEmployeeFields
