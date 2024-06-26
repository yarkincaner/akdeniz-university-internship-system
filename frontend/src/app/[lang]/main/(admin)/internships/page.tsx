import { FC } from 'react'
import Content from './content'

type Props = {
  searchParams?: {
    SearchString?: string
    PageNumber?: string
    PageSize?: string
  }
}

const Page: FC<Props> = ({ searchParams }) => {
  return (
    <div className='container mx-auto py-10'>
      <Content searchParams={searchParams} />
    </div>
  )
}

export default Page
