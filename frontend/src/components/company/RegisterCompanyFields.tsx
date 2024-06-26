import { CompanySchema } from '@/lib/validators/company'
import { zodResolver } from '@hookform/resolvers/zod'
import { FC } from 'react'
import { UseFormSetValue, useForm } from 'react-hook-form'
import { Button } from '../ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '../ui/form'
import { Input } from '../ui/input'
import { PhoneInput } from '../ui/phone-input'
import { Textarea } from '../ui/textarea'
import { DialogClose, DialogFooter } from '../ui/dialog'
import { getDictionary } from '@/lib/i18n/dictionary'
import { useParams } from 'next/navigation'
import { Locale } from '../../lib/i18n/i18n.config'
import { z } from 'zod'
import { useCreateCompany, useEditCompanyById } from '@/lib/mutations'

type RegisterCompanyFieldsProps = {
  handleDialog: () => void
  parentForm: UseFormSetValue<any>
  dictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['registerCompany']['fields']
}

const RegisterCompanyFields: FC<RegisterCompanyFieldsProps> = ({
  handleDialog,
  parentForm,
  dictionary
}) => {
  const { lang } = useParams<{ lang: Locale }>()
  const formSchema = CompanySchema(lang)
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: undefined,
      serviceArea: undefined,
      email: undefined,
      website: undefined,
      phone: undefined,
      address: undefined,
      taxNumber: '+905555555555'
    }
  })

  const { title: successMessage } = dictionary.toast.success
  const errorMessage = dictionary.toast.error

  const { mutate: createCompany, isLoading: isCreateLoading } =
    useCreateCompany(successMessage, errorMessage, handleDialog, parentForm)

  return (
    <Form {...form}>
      <form
        id='registerCompany'
        onSubmit={e => {
          e.stopPropagation()
          return form.handleSubmit(event => createCompany(event))(e)
        }}
        className='space-y-4'
      >
        <div className='flex w-full flex-col items-center gap-2 md:flex-row'>
          <FormField
            control={form.control}
            name='name'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.name}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.namePlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='serviceArea'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.serviceArea}</FormLabel>
                <FormControl>
                  <Input
                    type='text'
                    placeholder={dictionary.serviceAreaPlaceholder}
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
            name='email'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.email}</FormLabel>
                <FormControl>
                  <Input
                    type='email'
                    placeholder={dictionary.emailPlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name='website'
            render={({ field }) => (
              <FormItem className='w-full md:basis-1/2'>
                <FormLabel>{dictionary.website}</FormLabel>
                <FormControl>
                  <Input
                    type='website'
                    placeholder={dictionary.websitePlaceholder}
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
            name='phone'
            render={({ field }) => (
              <FormItem className='w-full space-y-2 md:basis-1/2'>
                <FormLabel>{dictionary.phoneNumber}</FormLabel>
                <FormControl className='w-full'>
                  <PhoneInput
                    placeholder={dictionary.phoneNumberPlaceholder}
                    {...field}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        <FormField
          control={form.control}
          name='address'
          render={({ field }) => (
            <FormItem className='w-full'>
              <FormLabel>{dictionary.address}</FormLabel>
              <FormControl>
                <Textarea
                  placeholder={dictionary.addressPlaceholder}
                  className='resize-none'
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <DialogFooter className='gap-2 sm:justify-between'>
          <DialogClose asChild>
            <Button type='button' variant='secondary'>
              {dictionary.dialogClose}
            </Button>
          </DialogClose>
          <Button
            type='submit'
            isLoading={isCreateLoading}
            form='registerCompany'
          >
            {dictionary.dialogSave}
          </Button>
        </DialogFooter>
      </form>
    </Form>
  )
}

export default RegisterCompanyFields
