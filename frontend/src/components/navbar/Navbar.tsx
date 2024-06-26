'use client'

import Link from 'next/link'
import { Suspense } from 'react'
import Avatar from '../Avatar'
import { useDictionary } from '../dictionary-provider'
import NavItems from './NavItems'

const Navbar = () => {
  const {
    navbar,
    page: {
      main: { user: userDictionary }
    }
  } = useDictionary()

  return (
    <div className='container inset-x-0 top-0 grid h-auto grid-cols-2 items-center justify-between gap-4 py-4'>
      <Link
        href='/main'
        className='flex items-center justify-start gap-2 text-nowrap'
      >
        <div className='max-w-[40px] py-1 md:max-w-[60px]'>
          <img
            className='inline-block rounded-full object-cover'
            src='/akdeniz-university-logo.png'
            alt='Akdeniz University logo'
          />
        </div>
        <h3 className='hidden font-semibold md:inline'>{navbar.title}</h3>
      </Link>
      <div className='flex items-center gap-4 justify-self-end'>
        <NavItems />
        <Suspense>
          <div className='justify-self-end'>
            <Avatar
              selectLabel={navbar.selectLabel}
              turkish={navbar.turkish}
              english={navbar.english}
              signoutDictionary={userDictionary}
              themesDictionary={navbar.themes}
            />
          </div>
        </Suspense>
      </div>
    </div>
  )
}

export default Navbar
