import Loader from '@/components/Loader'
import StatusBadge from '@/components/internship/StatusBadge'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import { useGetInternshipStatusesByInternshipId } from '@/lib/queries'
import { FC } from 'react'

type Props = {
  internshipId: number
}

const History: FC<Props> = ({ internshipId }) => {
  const { data: statuses, isLoading } =
    useGetInternshipStatusesByInternshipId(internshipId)

  if (isLoading) {
    return <Loader />
  }

  return (
    <section className='order-last bg-secondary/50 p-4 xl:order-first'>
      <h4 className='mb-12 text-start text-xl font-bold'>Staj tarihi</h4>
      <div className='rounded-md border'>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Staj Durumu</TableHead>
              <TableHead>Tarih</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {statuses?.map(status => (
              <TableRow key={status.id}>
                <TableCell className='max-w-24'>
                  <StatusBadge status={status.statusName} />
                </TableCell>
                <TableCell>
                  <p>{new Date(status.created).toLocaleDateString()}</p>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </section>
  )
}

export default History
