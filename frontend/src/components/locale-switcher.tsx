'use client'

import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue
} from './ui/select'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { Locale } from '../lib/i18n/i18n.config'
import Icons from './Icons'

const LocaleSwitcher = ({
  lang,
  selectLabel,
  turkish,
  english
}: {
  lang: string
  selectLabel: string
  turkish: string
  english: string
}) => {
  const router = useRouter()
  const pathName = usePathname()
  const searchParams = useSearchParams()

  const redirectedPathName = (locale: Locale) => {
    if (!pathName) return '/'
    const segments = pathName.split('/')
    segments[1] = locale
    return segments.join('/')
  }

  return (
    <Select
      onValueChange={(value: Locale) => {
        const params = new URLSearchParams(searchParams.toString())
        const newPath = redirectedPathName(value)
        const newUrl = `${newPath}${searchParams.toString() ? `?${params}` : ''}`
        return router.push(newUrl)
      }}
      defaultValue={lang}
    >
      <SelectTrigger className='w-auto gap-4'>
        <SelectValue />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectLabel>{selectLabel}</SelectLabel>
          <SelectItem value='tr' key={'tr'}>
            <div className='flex items-center'>
              <Icons.trFlag className='mr-2 size-4' /> {turkish}
            </div>
          </SelectItem>
          <SelectItem value='en' key={'en'}>
            <div className='flex items-center'>
              <Icons.ukFlag className='mr-2 size-4' /> {english}
            </div>
          </SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
  )
}

export default LocaleSwitcher
