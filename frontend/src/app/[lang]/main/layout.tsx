import Navbar from '@/components/navbar/Navbar'
import { Locale } from '../../../lib/i18n/i18n.config'

export default function MainLayout({
  children,
  params
}: {
  children: React.ReactNode
  params: { lang: Locale }
}) {
  return (
    <main>
      <Navbar />
      {children}
    </main>
  )
}
