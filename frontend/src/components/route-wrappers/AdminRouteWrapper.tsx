'use client'

import { useMsal } from '@azure/msal-react'
import { useRouter } from 'next/navigation'
import { useLayoutEffect } from 'react'
import authenticatedRouteWrapper from './AuthenticatedRouteWrapper'

const adminRouteWrapper = (WrappedComponent: any) => {
  const AdminWrapper = (props: any) => {
    const { accounts } = useMsal()
    const router = useRouter()
    const adminEmail = process.env.NEXT_PUBLIC_ADMIN_EMAIL

    const userEmail = accounts[0]?.username

    useLayoutEffect(() => {
      // Skip admin check if not in production
      if (process.env.NODE_ENV !== 'production') {
        return
      }

      if (userEmail !== adminEmail) {
        router.push('/main')
      }
    }, [userEmail, adminEmail, router])

    // Allow access if not on production
    if (process.env.NODE_ENV !== 'production') {
      return <WrappedComponent {...props} />
    }

    if (userEmail !== adminEmail) {
      return null
    }

    return <WrappedComponent {...props} />
  }

  return authenticatedRouteWrapper(AdminWrapper)
}

export default adminRouteWrapper
