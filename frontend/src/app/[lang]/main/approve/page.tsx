import { Locale } from '@/lib/i18n/i18n.config'
import { getDictionary } from '@/lib/i18n/dictionary'
import { Button } from '@/components/ui/button'
import { notFound } from 'next/navigation'
import InternshipInformations from './internship-informations'
import { Suspense } from 'react'

type ApprovePageProps = {
  params: {
    lang: Locale
  }
  searchParams: {
    [key: string]: string | string[] | undefined
  }
}

const ApprovePage = async ({ params, searchParams }: ApprovePageProps) => {
  const { token } = searchParams

  if (!token || typeof token !== 'string') {
    return notFound()
  }

  const { approvePageEmployee } = await getDictionary(params.lang)

  return (
    <section className='container my-12 min-h-[calc(100vh-13rem)] rounded-lg bg-secondary/50'>
      <div className='flex flex-col space-y-6 p-4 md:p-12'>
        <h1 className='text-center text-2xl'>{approvePageEmployee.title}</h1>
        <Suspense>
          <InternshipInformations token={token} />
        </Suspense>
      </div>
    </section>
  )
}

export default ApprovePage
