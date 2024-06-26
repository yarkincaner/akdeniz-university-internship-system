import type { Metadata } from 'next'
import { Locale, i18n } from '../../lib/i18n/i18n.config'
import { Inter } from 'next/font/google'
import '@/styles/globals.css'
import Providers from '@/components/Providers'
import { Toaster } from '@/components/ui/sonner'
import DictionaryProvider from '@/components/dictionary-provider'
import { getDictionary } from '@/lib/i18n/dictionary'

const inter = Inter({ subsets: ['latin'] })

const defaultUrl = process.env.VERCEL_URL
  ? `https://${process.env.VERCEL_URL}`
  : 'http://localhost:3000'

const englishMetadata: Metadata = {
  title: {
    template: '%s | Internship System',
    default: 'Akdeniz University | Internship System'
  },
  description: 'The official internship tracking system for Akdeniz University',
  metadataBase: new URL(defaultUrl)
}
const turkishMetadata: Metadata = {
  title: {
    template: '%s | Staj Sistemi',
    default: 'Akdeniz Üniversitesi | Staj Sistemi'
  },
  description: 'Akdeniz Üniversitesi resmi staj takip sitesi',
  metadataBase: new URL(defaultUrl)
}

export async function generateMetadata({
  params
}: {
  params: { lang: Locale }
}) {
  return params.lang === 'en' ? englishMetadata : turkishMetadata
}

export async function generateStaticParams() {
  return i18n.locales.map(locale => ({ lang: locale }))
}

export default async function RootLayout({
  children,
  params
}: Readonly<{
  children: React.ReactNode
  params: { lang: Locale }
}>) {
  const dictionary = await getDictionary(params.lang)

  return (
    <html
      lang={params.lang}
      className='bg-background text-foreground antialiased'
    >
      <body
        className={`${inter.className} flex min-h-screen flex-col bg-background antialiased`}
      >
        <DictionaryProvider dictionary={dictionary}>
          <Providers>
            {children}
            <Toaster richColors />
          </Providers>
        </DictionaryProvider>
      </body>
    </html>
  )
}
