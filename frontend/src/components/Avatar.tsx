'use client'

import { getDictionary } from '@/lib/i18n/dictionary'
import { useMsal } from '@azure/msal-react'
import { useTheme } from 'next-themes'
import {
  useParams,
  usePathname,
  useRouter,
  useSearchParams
} from 'next/navigation'
import { FC } from 'react'
import { Locale } from '../lib/i18n/i18n.config'
import Icons from './Icons'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuPortal,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuSub,
  DropdownMenuSubContent,
  DropdownMenuSubTrigger,
  DropdownMenuTrigger
} from './ui/dropdown-menu'

type Props = {
  selectLabel: string
  turkish: string
  english: string
  signoutDictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['page']['main']['user']
  themesDictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['navbar']['themes']
}

const Avatar: FC<Props> = ({
  selectLabel,
  turkish,
  english,
  signoutDictionary,
  themesDictionary
}) => {
  const { theme, setTheme } = useTheme()
  const { instance } = useMsal()
  const account = instance.getActiveAccount()
  const { lang } = useParams<{ lang: Locale }>()

  const handleLogOut = (e: Event) => {
    e.preventDefault()
    instance.logout({
      account: instance.getActiveAccount()
    })
  }

  const router = useRouter()
  const pathName = usePathname()
  const searchParams = useSearchParams()

  const handleRedirect = (locale: string) => {
    const params = new URLSearchParams(searchParams.toString())
    const path = redirectedPathName(locale)
    const newUrl = `${path}${searchParams.toString() ? `?${params}` : ''}`
    router.push(newUrl)
  }

  const redirectedPathName = (locale: string) => {
    if (!pathName) return '/'
    const segments = pathName.split('/')
    segments[1] = locale
    return segments.join('/')
  }

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <div className='cursor-pointer rounded-full bg-secondary p-3 text-secondary-foreground shadow transition-colors hover:bg-foreground/10'>
          <Icons.employee className='size-4' />
        </div>
      </DropdownMenuTrigger>
      <DropdownMenuContent align='end'>
        <DropdownMenuLabel>{account?.name}</DropdownMenuLabel>
        <DropdownMenuLabel className='w-[200px] truncate py-0'>
          {account?.username}
        </DropdownMenuLabel>

        <DropdownMenuSeparator />

        <DropdownMenuSub>
          <DropdownMenuSubTrigger>
            {lang === 'en' ? (
              <Icons.ukFlag className='mr-2 size-4' />
            ) : (
              <Icons.trFlag className='mr-2 size-4' />
            )}
            <span>{selectLabel}</span>
          </DropdownMenuSubTrigger>
          <DropdownMenuPortal>
            <DropdownMenuSubContent>
              <DropdownMenuRadioGroup
                value={lang}
                onValueChange={(value: string) => handleRedirect(value)}
              >
                <DropdownMenuRadioItem value='tr'>
                  <Icons.trFlag className='mr-2 size-4' />
                  {turkish}
                </DropdownMenuRadioItem>
                <DropdownMenuRadioItem value='en'>
                  <Icons.ukFlag className='mr-2 size-4' />
                  {english}
                </DropdownMenuRadioItem>
              </DropdownMenuRadioGroup>
            </DropdownMenuSubContent>
          </DropdownMenuPortal>
        </DropdownMenuSub>
        <DropdownMenuSub>
          <DropdownMenuSubTrigger>
            <Icons.lightMode className='mr-2 size-4 rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0' />
            <Icons.darkMode className='absolute mr-2 size-4 rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100' />
            <span>{themesDictionary.title}</span>
          </DropdownMenuSubTrigger>
          <DropdownMenuPortal>
            <DropdownMenuSubContent>
              <DropdownMenuRadioGroup value={theme} onValueChange={setTheme}>
                <DropdownMenuRadioItem value='light'>
                  <Icons.lightMode className='mr-2 size-4' />
                  {themesDictionary.light}
                </DropdownMenuRadioItem>
                <DropdownMenuRadioItem value='dark'>
                  <Icons.darkMode className='mr-2 size-4' />
                  {themesDictionary.dark}
                </DropdownMenuRadioItem>
                <DropdownMenuRadioItem value='system'>
                  <Icons.systemMode className='mr-2 size-4' />
                  {themesDictionary.system}
                </DropdownMenuRadioItem>
              </DropdownMenuRadioGroup>
            </DropdownMenuSubContent>
          </DropdownMenuPortal>
        </DropdownMenuSub>

        <DropdownMenuSeparator />

        <DropdownMenuItem
          onSelect={handleLogOut}
          className='cursor-pointer text-destructive'
        >
          <Icons.logout className='mr-2 size-4' />
          {signoutDictionary.logoutButtonLabel}
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}

export default Avatar
