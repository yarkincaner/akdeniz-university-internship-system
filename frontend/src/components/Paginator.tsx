'use client'

import { FC, FormEvent, useState } from 'react'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { Button } from './ui/button'
import Icons from './Icons'
import { Input } from './ui/input'
import Loader from './Loader'

type Props = {
  pageNumber: string
  totalPages: number
  pageParamName: string
  isLoading: boolean
}

const Paginator: FC<Props> = ({
  pageNumber,
  totalPages,
  pageParamName,
  isLoading
}) => {
  const searchParams = useSearchParams()
  const pathname = usePathname()
  const { replace } = useRouter()
  const pageNumberAsNumber = parseInt(pageNumber)
  const [inputValue, setInputValue] = useState<string>(pageNumber)

  const handlePage = (page: number) => {
    if (isNaN(pageNumberAsNumber)) return
    if (page > totalPages || page < 1) return
    const params = new URLSearchParams(searchParams)
    params.set(pageParamName, page.toString())
    replace(`${pathname}?${params}`)
    setInputValue(page.toString())
  }

  const handleInput = () => {
    if (inputValue.length <= 0) return
    if (inputValue === pageNumber) return
    handlePage(parseInt(inputValue))
  }

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    e.stopPropagation()
    handleInput()
  }

  return (
    <div className='p-2'>
      <nav
        className='flex items-center justify-center space-x-1'
        role='navigation'
        aria-label='Pagination'
      >
        <Button
          variant={'ghost'}
          className='p-1'
          size={'sm'}
          rel='prev'
          onClick={() => handlePage(pageNumberAsNumber - 1)}
          disabled={pageNumberAsNumber - 1 < 1 || isLoading}
        >
          <span className='sr-only'>Previous</span>
          <Icons.prev className='size-4' />
        </Button>
        <div className='flex items-center justify-center'>
          <form onSubmit={handleSubmit} id='handle-pagination-form'>
            <Input
              type='text'
              value={inputValue}
              onChange={e => {
                setInputValue(e.target.value)
              }}
              className='size-8 rounded-lg border px-1 text-center text-sm shadow-sm focus:outline-0 focus:ring-1 focus:ring-gray-500'
            />
          </form>
          <div className='mx-2 text-sm text-gray-600'>/</div>
          <div className='text-sm text-gray-600'>
            {isLoading ? <Loader /> : totalPages}
          </div>
        </div>
        <Button
          variant={'ghost'}
          className='p-1'
          size={'sm'}
          rel='next'
          onClick={() => handlePage(pageNumberAsNumber + 1)}
          disabled={pageNumberAsNumber + 1 > totalPages || isLoading}
        >
          <span className='sr-only'>Next</span>
          <Icons.next className='size-4' />
        </Button>
      </nav>
    </div>
  )
}

export default Paginator
