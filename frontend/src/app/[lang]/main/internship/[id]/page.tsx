import { Metadata } from 'next'
import { FC } from 'react'
import Content from './content'

export const metadata: Metadata = {
  title: 'Internship'
}

type InternshipPageProps = {
  params: {
    id: number
  }
}

const InternshipPage: FC<InternshipPageProps> = ({ params }) => {
  const { id } = params

  return (
    <main className='container flex flex-row items-center justify-center py-12 md:flex-col'>
      <Content internshipId={id} />
    </main>
  )
}

export default InternshipPage
