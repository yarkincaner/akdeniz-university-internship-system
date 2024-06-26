import { useState, useCallback } from 'react'

import { InteractionType } from '@azure/msal-browser'
import { useMsal, useMsalAuthentication } from '@azure/msal-react'
import { Method } from 'axios'
import { tokenRequest } from '@/config/authConfig'

const useFetchWithMsal = () => {
  const { instance } = useMsal()
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const [data, setData] = useState()
  const [error, setError] = useState<any>()

  const { result, error: msalError } = useMsalAuthentication(
    InteractionType.Silent,
    tokenRequest,
    {
      ...instance.getActiveAccount()
    }
  )

  /**
   * Custom hook to call a web API using bearer token obtained from MSAL
   * @param {Method} method
   * @param {string} endpoint
   * @param {any} body
   * @returns
   */
  const execute = async (method: Method, endpoint: string, body?: any) => {
    if (msalError) {
      setError(msalError)
      console.log(msalError)
      return
    }

    const { accessToken } = await instance.acquireTokenSilent(tokenRequest)

    if (instance.getActiveAccount()) {
      try {
        let response = null

        const headers = new Headers()
        const bearer = `Bearer ${accessToken}`
        headers.append('Authorization', bearer)

        // Only append Content-Type if the body is not FormData
        if (!(body instanceof FormData)) {
          headers.append('Content-Type', 'application/json')
        }

        let options: RequestInit = {
          method: method,
          headers: headers,
          body: body instanceof FormData ? body : JSON.stringify(body)
        }

        setIsLoading(true)

        response = await (await fetch(endpoint, options)).json()
        setData(response)

        setIsLoading(false)
        return response
      } catch (e: any) {
        setError(e)
        setIsLoading(false)
        throw e
      }
    }
  }

  return {
    isLoading,
    error,
    data,
    execute: useCallback(execute, [result, msalError, instance]) // to avoid infinite calls when inside a `useEffect`,
  }
}

export default useFetchWithMsal
