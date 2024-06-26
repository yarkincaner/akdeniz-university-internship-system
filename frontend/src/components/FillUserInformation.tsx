'use client'

import { FC, useState } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from './ui/dialog'
import { Button } from './ui/button'
import { useGetUser } from '@/lib/queries'
import { Input } from './ui/input'
import { Label } from './ui/label'
import { useUpdateUserInformation } from '@/lib/mutations'
import { getDictionary } from '@/lib/i18n/dictionary'

type Props = {
  dictionary: Awaited<ReturnType<typeof getDictionary>>['fillUserInformation']
}

const FillUserInformation: FC<Props> = ({ dictionary }) => {
  const [open, setOpen] = useState<boolean>(false)
  const { data, isLoading } = useGetUser(setOpen)

  const { mutate: updateUser, isLoading: isSubmitLoading } =
    useUpdateUserInformation(setOpen)

  return (
    <Dialog open={open}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{dictionary.title}</DialogTitle>
          <DialogDescription>{dictionary.description}</DialogDescription>
        </DialogHeader>
        <form id='user-information-form' action={updateUser}>
          <div className='flex flex-col space-y-2'>
            <div className='grid grid-cols-5 items-center'>
              <Label className='col-span-2'>{dictionary.fields.tcKimlik}</Label>
              <Input type='number' name='tcKimlikNo' className='col-span-3' />
            </div>
            <div className='grid grid-cols-5 items-center'>
              <Label className='col-span-2'>
                {dictionary.fields.birthyear}
              </Label>
              <Input type='number' name='birthyear' className='col-span-3' />
            </div>
          </div>
        </form>
        <DialogFooter>
          <Button
            type='submit'
            form='user-information-form'
            isLoading={isSubmitLoading}
          >
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}

export default FillUserInformation
