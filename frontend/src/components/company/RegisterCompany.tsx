'use client'

import { FC, useState } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '../ui/dialog'
import { Button } from '../ui/button'
import RegisterCompanyFields from './RegisterCompanyFields'
import { UseFormSetValue } from 'react-hook-form'
import { getDictionary } from '@/lib/i18n/dictionary'
import Icons from '../Icons'

type RegisterCompanyProps = {
  form: UseFormSetValue<any>
  dictionary: Awaited<ReturnType<typeof getDictionary>>['registerCompany']
}

const RegisterCompany: FC<RegisterCompanyProps> = ({ form, dictionary }) => {
  const [open, setOpen] = useState<boolean>(false)

  const handleDialog = () => {
    setOpen(prev => !prev)
  }

  return (
    <Dialog open={open} onOpenChange={handleDialog}>
      <DialogTrigger asChild>
        <Button variant='ghost' className='w-full'>
          <Icons.plus className='mr-2 size-4' /> {dictionary.dialogTitle}
        </Button>
      </DialogTrigger>
      <DialogContent className='max-h-screen overflow-y-scroll md:max-w-screen-md'>
        <DialogHeader>
          <DialogTitle>{dictionary.dialogTitle}</DialogTitle>
          <DialogDescription>{dictionary.dialogDesc}</DialogDescription>
        </DialogHeader>
        <RegisterCompanyFields
          parentForm={form}
          handleDialog={handleDialog}
          dictionary={dictionary.fields}
        />
      </DialogContent>
    </Dialog>
  )
}

export default RegisterCompany
