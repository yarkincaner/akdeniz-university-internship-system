import { Loader2 } from 'lucide-react'
import { FC } from 'react'

type Props = {}

const Loader: FC<Props> = ({}) => {
  return (
    <div className='flex w-full items-center justify-center'>
      <Loader2 className='size-8 animate-spin text-primary' />
    </div>
  )
}

export default Loader
