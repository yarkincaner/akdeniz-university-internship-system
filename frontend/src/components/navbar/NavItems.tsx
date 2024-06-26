'use client'

import { useMediaQuery } from '@/hooks/useMediaQuery'
import { cn } from '@/lib/utils'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
import { FC } from 'react'
import Icons from '../Icons'
import adminRouteWrapper from '../route-wrappers/AdminRouteWrapper'
import { Button } from '../ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '../ui/dropdown-menu'

const NAV_ITEMS = [
  {
    id: 0,
    title: 'Home',
    href: '/main',
    icon: <Icons.home className='mr-1.5 size-4' />
  },
  {
    id: 1,
    title: 'Companies',
    href: '/main/companies',
    icon: <Icons.company className='mr-1.5 size-4' />
  },
  {
    id: 2,
    title: 'Supervisors',
    href: '/main/employees',
    icon: <Icons.employee className='mr-1.5 size-4' />
  },
  {
    id: 3,
    title: 'Internships',
    href: '/main/internships',
    icon: <Icons.file className='mr-1.5 size-4' />
  }
]

type Props = {}

const NavItems: FC<Props> = ({}) => {
  const isDesktop = useMediaQuery('(min-width: 1165px)')
  const router = useRouter()
  const pathname = usePathname()

  // Extract the path after the language route
  const langPath = '/' + pathname.split('/').slice(2).join('/')

  if (!isDesktop) {
    return (
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant={'ghost'} size={'icon'}>
            <Icons.menu className='size-4' />
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align='end'>
          <DropdownMenuLabel>Admin routes</DropdownMenuLabel>
          <DropdownMenuSeparator />
          {NAV_ITEMS.map(item => (
            <DropdownMenuItem
              key={item.id}
              onSelect={() => {
                router.push(item.href)
              }}
              className={cn('cursor-pointer', {
                'bg-primary text-primary-foreground': langPath === item.href
              })}
            >
              {item.icon}
              {item.title}
            </DropdownMenuItem>
          ))}
        </DropdownMenuContent>
      </DropdownMenu>
    )
  }

  return (
    <nav className='rounded bg-secondary/50 p-1.5 shadow'>
      {NAV_ITEMS.map(item => (
        <Button
          key={item.id}
          asChild
          variant={'link'}
          size={'sm'}
          className={cn('text-foreground', {
            'bg-primary text-primary-foreground': langPath === item.href
          })}
        >
          <Link href={item.href} className='flex items-center space-x-1'>
            {item.icon}
            {item.title}
          </Link>
        </Button>
      ))}
    </nav>
  )
}

export default adminRouteWrapper(NavItems)
