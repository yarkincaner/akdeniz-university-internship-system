import { Locale } from '../../lib/i18n/i18n.config'
import Image from 'next/image'
import img from '../../../public/akdeniz-university-logo.png'
import { getDictionary } from '@/lib/i18n/dictionary'
import Login from '@/components/Login'
import LocaleSwitcher from '@/components/locale-switcher'
import Link from 'next/link'
import { Suspense } from 'react'

export default async function Home({
  params: { lang }
}: {
  params: { lang: Locale }
}) {
  const {
    page: { login },
    navbar
  } = await getDictionary(lang)

  return (
    <div className='container flex min-w-full flex-grow items-center justify-center bg-gradient-to-tl from-blue-500 via-orange-300 to-blue-500'>
      <div className='flex max-w-4xl flex-grow flex-col rounded-md bg-white shadow-lg md:flex-row md:justify-center'>
        <div className='flex basis-1/2 flex-col items-center'>
          <div className='flex w-full items-center justify-center gap-2 rounded-t-md bg-gray-900 p-8 md:flex-col md:gap-4 md:rounded-l-md md:rounded-tr-none md:p-24'>
            <div className='flex max-w-[75px] items-center justify-center md:max-w-[150px]'>
              <Image
                priority
                src={img}
                width={2103}
                height={2103}
                quality={100}
                className='block'
                alt='Akdeniz University logo'
              />
            </div>
            <div className='flex flex-col items-center justify-center'>
              <h1 className='text-center text-lg font-medium text-white md:text-2xl'>
                {login.title}
              </h1>
            </div>
          </div>
        </div>
        <div className='flex basis-1/2 flex-col justify-center gap-2 p-4 md:px-8'>
          <div className='flex w-full justify-end'>
            <Suspense>
              <LocaleSwitcher
                lang={lang}
                selectLabel={navbar.selectLabel}
                turkish={navbar.turkish}
                english={navbar.english}
              />
            </Suspense>
          </div>

          <div className='space-y-4'>
            <div>
              <h3 className='text-lg font-bold text-gray-600'>
                {login.loginTitle}
              </h3>
              <p className='text-sm text-gray-600'>{login.loginDesc}</p>
            </div>
            <div className='flex w-full flex-col space-y-2 rounded-md'>
              <Login buttonLabel={login.buttonLabel} />
              <Link
                href='https://ekampus.akdeniz.edu.tr/hesap/sifre.aspx'
                target='_blank'
                rel='noopener noreferrer'
                className='text-center text-sm text-gray-600 transition-all duration-300 hover:font-medium'
              >
                {login.forgotPassword}
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
