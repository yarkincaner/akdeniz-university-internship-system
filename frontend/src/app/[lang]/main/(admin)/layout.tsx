import { FC, ReactNode } from 'react'

type Props = {
  children: ReactNode
}

const Layout: FC<Props> = ({ children }) => {
  // TODO: Role based access yapılacak role adminse görüntüleyebilecek
  return <>{children}</>
}

export default Layout
