'use client'

import { FC } from 'react'
import InternshipCard from './InternshipCard'
import Icons from '@/components/Icons'
import { getDictionary } from '@/lib/i18n/dictionary'
import Loader from '../Loader'
import { useGetInternshipsByUserId } from '@/lib/queries'

type InternshipsProps = {
  dictionary: Awaited<
    ReturnType<typeof getDictionary>
  >['page']['main']['internships']
}

const Internships: FC<InternshipsProps> = ({ dictionary }) => {
  const { data: internshipsQuery, isLoading } = useGetInternshipsByUserId({
    error: { title: dictionary.error.title, desc: dictionary.error.desc }
  })

  if (isLoading) return <Loader />
  if (internshipsQuery && internshipsQuery.length > 0) {
    return (
      <ul className='grid w-full grid-cols-1 gap-2 xl:grid-cols-2'>
        {internshipsQuery.map(internship => (
          <li key={internship.id}>
            <InternshipCard internship={internship} />
          </li>
        ))}
      </ul>
    )
  }

  return (
    <div className='flex w-full flex-grow flex-col items-center justify-center gap-2'>
      <Icons.noData className='size-32' />
      <h3 className='text-sm text-accent-foreground md:text-lg'>
        {dictionary.notFound}
      </h3>
    </div>
  )
}

export default Internships
