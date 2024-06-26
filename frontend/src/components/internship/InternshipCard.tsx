import { TOOLTIP_DELAY } from '@/config/config'
import { getInternshipProgress, getWorkDays } from '@/lib/utils'
import { Internship } from '@/types/internship'
import Link from 'next/link'
import { FC } from 'react'
import Icons from '../Icons'
import { useDictionary } from '../dictionary-provider'
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle
} from '../ui/card'
import { Progress } from '../ui/progress'
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger
} from '../ui/tooltip'
import EditInternship from './EditInternship'
import StatusBadge from './StatusBadge'

type Props = {
  internship: Internship
}

const InternshipCard: FC<Props> = ({ internship }) => {
  const {
    id,
    companyName,
    employeeName,
    comment,
    statusName,
    startDate,
    endDate
  } = internship

  const {
    page: {
      main: {
        internships: { internshipCard: dictionary }
      }
    }
  } = useDictionary()

  const isEditable = statusName.toString() === 'DeclinedFromInternshipCommitte'

  // This just gets number of days between two dates and removes sunday and saturday from total number. This value does not correspond to real totalDays value in database.
  const totalDays = getWorkDays(startDate, endDate)
  const remainingDays = getInternshipProgress(startDate, endDate, totalDays)
  const progressValue = (remainingDays / totalDays) * 100

  return (
    <div className='relative h-full'>
      {isEditable ? <EditInternship values={internship} /> : null}
      <Link href={`/main/internship/${id}`}>
        <Card className='flex h-full w-full flex-col justify-between'>
          <CardHeader>
            <CardTitle className='text-xs font-bold md:text-base'>
              <p className='flex items-center'>
                <Icons.company className='mr-1 size-4' />
                <span className='mr-2'>{dictionary.company}:</span>
                <span className='font-medium'>{companyName}</span>
              </p>
              <p className='flex items-center'>
                <Icons.employee className='mr-1 size-4' />
                <span className='mr-2'>{dictionary.employee}:</span>
                <span className='font-medium'>{employeeName}</span>
              </p>
            </CardTitle>
          </CardHeader>
          <CardContent>
            <p className='text-sm text-muted-foreground'>{comment}</p>
          </CardContent>
          <CardFooter className='grid grid-cols-1 justify-between gap-4 md:grid-cols-2'>
            <StatusBadge status={statusName} />
            <div className='flex items-center justify-between space-x-2'>
              <span className='text-xs md:text-sm'>{dictionary.progress}:</span>
              <TooltipProvider delayDuration={TOOLTIP_DELAY}>
                <Tooltip>
                  <TooltipTrigger asChild>
                    <Progress
                      value={progressValue > 100 ? 100 : progressValue}
                      className={'h-3 w-full md:h-4'}
                    />
                  </TooltipTrigger>
                  <TooltipContent>
                    <p>
                      {dictionary.startDate}:{' '}
                      {new Date(startDate).toLocaleDateString()},{' '}
                      {dictionary.endDate}:{' '}
                      {new Date(endDate).toLocaleDateString()}
                    </p>
                  </TooltipContent>
                </Tooltip>
              </TooltipProvider>
            </div>
          </CardFooter>
        </Card>
      </Link>
    </div>
  )
}

export default InternshipCard
