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
import RegisterEmployeeFields from './RegisterEmployeeFields'
import { UseFormSetValue } from 'react-hook-form'
import { getDictionary } from '@/lib/i18n/dictionary'
import Icons from '../Icons'

type Props = {
  companyId: number
  companyName: string
  parentForm: UseFormSetValue<any>
  dictionary: Awaited<ReturnType<typeof getDictionary>>['registerEmployee']
}

const RegisterEmployee: FC<Props> = ({
  companyId,
  companyName,
  parentForm,
  dictionary
}) => {
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
          <DialogTitle>
            {companyName} - {dictionary.dialogTitle}
          </DialogTitle>
          <DialogDescription>{dictionary.dialogDesc}</DialogDescription>
        </DialogHeader>
        <RegisterEmployeeFields
          handleDialog={handleDialog}
          companyId={companyId}
          parentForm={parentForm}
          dictionary={dictionary.fields}
        />
      </DialogContent>
    </Dialog>
  )
}

export default RegisterEmployee
