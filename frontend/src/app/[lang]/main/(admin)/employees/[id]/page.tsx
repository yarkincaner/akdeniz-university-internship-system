import { FC } from 'react'
import Content from './content'

type Props = {
  params: {
    id: number
  }
}

const Page: FC<Props> = ({ params }) => {
  return (
    <main className='container mx-auto py-10'>
      <Content id={params.id} />
    </main>
  )
}

export default Page
