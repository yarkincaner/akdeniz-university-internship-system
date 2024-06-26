'use client'

import { useIsAuthenticated } from '@azure/msal-react'
import { useRouter } from 'next/navigation'
import { useLayoutEffect } from 'react'

const authenticatedRouteWrapper = (WrappedComponent: any) => {
  const AuthenticatedWrapper = (props: any) => {
    const isAuthenticated = useIsAuthenticated()
    const router = useRouter()

    useLayoutEffect(() => {
      if (!isAuthenticated) {
        router.push('/')
      }
    }, [isAuthenticated, router])

    if (!isAuthenticated) return null

    return <WrappedComponent {...props} />
  }

  return AuthenticatedWrapper
}

export default authenticatedRouteWrapper
