'use client'
import { Button } from './ui/button'
import { FC, useState } from 'react'
import { useMsal } from '@azure/msal-react'
import { loginRequest } from '@/config/authConfig'
import { useRouter } from 'next/navigation'

type LoginProps = {
  buttonLabel: string
}

const Login: FC<LoginProps> = ({ buttonLabel }) => {
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const { instance } = useMsal()
  const router = useRouter()

  const handleLoginRedirect = async () => {
    setIsLoading(true)
    await instance
      .loginPopup(loginRequest)
      .then(response => {
        instance.setActiveAccount(response.account)
      })
      .catch(e => {
        console.log(e)
      })
    setIsLoading(false)
    router.push('/main')
  }

  return (
    <div>
      <Button
        onClick={handleLoginRedirect}
        size='stretched'
        isLoading={isLoading}
        className='flex w-full rounded-sm bg-gray-900 px-24 font-bold  uppercase text-white shadow-md transition duration-300 hover:bg-gray-700 hover:shadow-lg '
      >
        {buttonLabel}
      </Button>
    </div>
  )
}

export default Login
