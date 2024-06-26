import Internships from '@/components/internship/Internships'
import { Locale } from '../../../lib/i18n/i18n.config'
import { getDictionary } from '@/lib/i18n/dictionary'
import InternshipForm from '@/components/internship/InternshipForm'
import { FC, Suspense } from 'react'
import { Metadata } from 'next'
import FillUserInformation from '@/components/FillUserInformation'

export const metadata: Metadata = {
  title: 'Dashboard'
}

type Props = {
  params: {
    lang: Locale
  }
  searchParams: { [key: string]: string | string[] | undefined }
}

const Main: FC<Props> = async ({ params, searchParams }) => {
  const {
    page: { main },
    addInternship,
    registerCompany,
    registerEmployee,
    fillUserInformation
  } = await getDictionary(params.lang)

  return (
    <main className='container flex flex-col items-center gap-4 pt-12'>
      <section className='flex w-full flex-grow flex-col gap-2 rounded-lg bg-secondary/50 p-4 md:items-start'>
        <Suspense>
          <InternshipForm
            lang={params.lang}
            dictionary={addInternship}
            companyDictionary={registerCompany}
            employeeDictionary={registerEmployee}
            searchParams={searchParams}
          />
        </Suspense>
        <FillUserInformation dictionary={fillUserInformation} />
        <Internships dictionary={main.internships} />
      </section>
    </main>
  )
}

export default Main
