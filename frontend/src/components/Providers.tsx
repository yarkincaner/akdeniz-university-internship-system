'use client'
import { msalConfig } from '@/config/authConfig'
import { EventType, PublicClientApplication } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { QueryClient, QueryClientProvider } from 'react-query'
import { ReactNode } from 'react'
import { twentyFourHoursInMs } from '@/config/config'
import { ThemeProvider } from './theme-provider'

export const msalInstance = new PublicClientApplication(msalConfig)

msalInstance.enableAccountStorageEvents()

msalInstance.addEventCallback(event => {
  try {
    //@ts-ignore
    if (event.eventType === EventType.LOGIN_SUCCESS && event.payload?.account) {
      if (process.env.NODE_ENV === 'development') {
        console.log(event.payload)
      }
      //@ts-ignore
      msalInstance.setActiveAccount(event.payload?.account)
    }
  } catch (error) {
    console.error('Something wrong in msalInstance.addEventCallback - ', error)
  }
})

type Props = {
  children: ReactNode
}

const Providers = (props: Props) => {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: twentyFourHoursInMs
      }
    }
  })
  return (
    <MsalProvider instance={msalInstance}>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider
          attribute='class'
          defaultTheme='system'
          enableSystem
          disableTransitionOnChange
        >
          {props.children}
        </ThemeProvider>
      </QueryClientProvider>
    </MsalProvider>
  )
}

export default Providers
