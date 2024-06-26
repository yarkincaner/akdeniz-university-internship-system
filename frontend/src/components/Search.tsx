'use client'

import { FC } from 'react'
import Icons from './Icons'
import { Input } from './ui/input'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { useDebouncedCallback } from 'use-debounce'
import { DEBOUNCE_DELAY } from '@/config/config'
import { cn } from '@/lib/utils'

type Props = {
  placeholder: string
  paramName: string
  pageParamName: string
  className?: string
}

const Search: FC<Props> = ({ placeholder, paramName, pageParamName, className }) => {
  const searchParams = useSearchParams()
  const pathname = usePathname()
  const { replace } = useRouter()

  const handleSearch = useDebouncedCallback(term => {
    const params = new URLSearchParams(searchParams)

    if (term) {
      params.set(paramName, term)
    } else {
      params.delete(paramName)
    }

    params.set(pageParamName, '1')
    replace(`${pathname}?${params.toString()}`)
  }, DEBOUNCE_DELAY)

  return (
    <div className='relative flex max-w-2xl items-center'>
      <Icons.search className='absolute left-2 top-1/2 size-4 -translate-y-1/2 transform' />
      <Input
        placeholder={placeholder}
        className={cn('pl-8', className)}
        defaultValue={searchParams.get(paramName)?.toString()}
        onChange={e => {
          handleSearch(e.target.value)
        }}
      />
    </div>
  )
}

export default Search
