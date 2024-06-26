'use client'

import { FC } from 'react'
import {
  Document,
  Font,
  Image,
  Page,
  StyleSheet,
  Text
} from '@react-pdf/renderer'
import {
  DataTableCell,
  Table,
  TableBody,
  TableCell,
  TableHeader
} from '@david.kucsai/react-pdf-table'
import { Internship } from '@/types/internship'

// Türkçe karakterler için font değişikliği
Font.register({
  family: 'Roboto',
  src: 'https://cdnjs.cloudflare.com/ajax/libs/ink/3.1.10/fonts/Roboto/roboto-light-webfont.ttf'
})

const styles = StyleSheet.create({
  logo: {
    maxHeight: '100px',
    maxWidth: '100px',
    padding: '4px'
  },
  page: {
    display: 'flex',
    alignItems: 'center',
    fontFamily: 'Roboto',
    fontSize: 11,
    padding: '8px'
  },
  section: {
    margin: 10,
    padding: 10
  },
  viewer: {
    width: '100%', //the pdf viewer will take up all of the width and height
    minHeight: '100%'
  }
})

type Props = {
  internship: Internship[]
}

const PDFView: FC<Props> = ({ internship }) => {
  const imageSrc =
    process.env.NODE_ENV === 'development'
      ? 'http://localhost:3000/akdeniz-university-logo.png'
      : `${process.env.VERCEL_URL}/akdeniz-university-logo.png`

  return (
    <Document>
      <Page style={styles.page}>
        <Image src={imageSrc} style={styles.logo} />
        <Text
          textAnchor='middle'
          style={{ fontWeight: 'bold', fontSize: '20px' }}
        >
          Akdeniz Üniversitesi Staj Sistemi
        </Text>
        <Text
          textAnchor='middle'
          style={{ fontWeight: 'bold', fontSize: '14px' }}
        >
          Oluşturulan stajlar
        </Text>
        {/* @ts-expect-error */}
        <Table zebra data={internship}>
          {/* @ts-expect-error */}
          <TableHeader textAlign={'center'}>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>Student Tc Kimlik No</TableCell>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>Student Id</TableCell>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>Student Name</TableCell>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>Company Name</TableCell>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>Start Date</TableCell>
            {/* @ts-expect-error */}
            <TableCell style={styles.cell}>End Date</TableCell>
          </TableHeader>
          {/* @ts-expect-error */}
          <TableBody textAlign='center'>
            <DataTableCell getContent={r => r.userTcKimlikNo} />
            <DataTableCell
              getContent={r => {
                const userId = r.userEmail.substring(
                  0,
                  r.userEmail.indexOf('@')
                )
                return userId
              }}
            />
            <DataTableCell getContent={r => r.userName} />
            <DataTableCell getContent={r => r.companyName} />
            <DataTableCell
              getContent={r => {
                const startDate = new Date(r.startDate).toLocaleDateString()
                return startDate
              }}
            />
            <DataTableCell
              getContent={r => {
                const endDate = new Date(r.endDate).toLocaleDateString()
                return endDate
              }}
            />
          </TableBody>
        </Table>
      </Page>
    </Document>
  )
}

export default PDFView
