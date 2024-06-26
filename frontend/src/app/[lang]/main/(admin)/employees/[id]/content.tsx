'use client'

import Loader from '@/components/Loader'
import { useGetEmployeeById } from '@/lib/queries'
import { FC } from 'react'
import SupervisedInternships from './supervised-internships'
import adminRouteWrapper from '@/components/route-wrappers/AdminRouteWrapper'

type Props = {
  id: number
}

const Content: FC<Props> = ({ id }) => {
  const { data, isLoading } = useGetEmployeeById(id)

  if (isLoading) return <Loader />
  if (!data?.data) return <h1>Employee not found!</h1>

  const employee = data.data
  return (
    <div>
      <section className='grid grid-cols-1 rounded bg-secondary/50 p-4 text-sm shadow md:grid-cols-2 md:text-base'>
        <div>
          <div className='flex items-center space-x-1'>
            <p className='font-semibold'>First Name:</p>
            <p>{employee.firstName}</p>
          </div>
          <div className='flex items-center space-x-1'>
            <p className='font-semibold'>Last Name:</p>
            <p>{employee.lastName}</p>
          </div>
          <div className='flex items-center space-x-1'>
            <p className='font-semibold'>E-mail:</p>
            <p>{employee.email}</p>
          </div>
        </div>
        <div>
          <div className='flex items-center space-x-1 md:justify-end'>
            <p className='font-semibold'>Company:</p>
            <p>{employee.companyName}</p>
          </div>
          <div className='flex items-center space-x-1 md:justify-end'>
            <p className='font-semibold'>Title:</p>
            <p>{employee.title}</p>
          </div>
        </div>
      </section>
      <section className='mt-3 flex flex-col space-y-3 rounded bg-secondary/50 p-4 shadow md:mt-6 md:space-y-6'>
        <h4 className='font-semibold md:text-xl'>Supervised Internships</h4>
        <SupervisedInternships internships={employee.internships} />
      </section>
    </div>
  )
}

export default adminRouteWrapper(Content)
