'use client'

import * as React from 'react'
import { useTheme } from 'next-themes'

import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import Icons from './Icons'
import { getDictionary } from '@/lib/i18n/dictionary'
import { FC } from 'react'

type Props = {
  dictionary: Awaited<ReturnType<typeof getDictionary>>['navbar']['themes']
}

const ThemeSwitcher: FC<Props> = ({ dictionary }) => {
  const { theme, setTheme } = useTheme()

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant='ghost' size='icon'>
          <Icons.lightMode className='h-[1.2rem] w-[1.2rem] rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0' />
          <Icons.darkMode className='absolute h-[1.2rem] w-[1.2rem] rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100' />
          <span className='sr-only'>Toggle theme</span>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align='end'>
        <DropdownMenuLabel>{dictionary.title}</DropdownMenuLabel>
        <DropdownMenuRadioGroup value={theme} onValueChange={setTheme}>
          <DropdownMenuRadioItem value='light'>
            {dictionary.light}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value='dark'>
            {dictionary.dark}
          </DropdownMenuRadioItem>
          <DropdownMenuRadioItem value='system'>
            {dictionary.system}
          </DropdownMenuRadioItem>
        </DropdownMenuRadioGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}

export default ThemeSwitcher
