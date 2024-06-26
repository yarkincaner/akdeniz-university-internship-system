import { getDictionary } from '@/lib/i18n/dictionary'
import { FC, ReactNode } from 'react'
import Icons from '../Icons'
import { cn } from '@/lib/utils'
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger
} from '../ui/tooltip'
import { TOOLTIP_DELAY } from '@/config/config'
import { Status } from '@/types/status'
import { useDictionary } from '../dictionary-provider'

type StatusContentProps = {
  colors: string
  text: string
  children: ReactNode
}

const StatusContent: FC<StatusContentProps> = ({ colors, text, children }) => {
  return (
    <TooltipProvider delayDuration={TOOLTIP_DELAY}>
      <Tooltip>
        <TooltipTrigger asChild>
          <div
            className={cn(
              'flex min-w-0 max-w-fit items-center rounded-full px-3 py-1',
              colors
            )}
          >
            {children}
            <span className='max-w-fit truncate text-xs md:text-sm'>
              {text}
            </span>
          </div>
        </TooltipTrigger>
        <TooltipContent>
          <p>{text}</p>
        </TooltipContent>
      </Tooltip>
    </TooltipProvider>
  )
}

type Props = {
  status: Status
}

const StatusBadge: FC<Props> = ({ status }) => {
  const {
    page: {
      main: {
        internships: {
          internshipCard: { status: dictionary }
        }
      }
    }
  } = useDictionary()

  switch (status.toString()) {
    case 'PendingApprovalFromInternshipCommittee':
      return (
        <StatusContent
          text={dictionary.PendingApprovalFromInternshipCommittee}
          colors='bg-warning text-warning-foreground'
        >
          <Icons.circle className='mr-2 size-4' />
        </StatusContent>
      )
    case 'AcceptedByInternshipCommittee':
      return (
        <StatusContent
          text={dictionary.AcceptedByInternshipCommittee}
          colors='bg-success text-success-foreground'
        >
          <Icons.accepted className='mr-2 size-4' />
        </StatusContent>
      )
    case 'PendingApprovalFromCompany':
      return (
        <StatusContent
          text={dictionary.PendingApprovalFromCompany}
          colors='bg-warning text-warning-foreground'
        >
          <Icons.circle className='mr-2 size-4' />
        </StatusContent>
      )
    case 'AcceptedByCompany':
      return (
        <StatusContent
          text={dictionary.AcceptedByCompany}
          colors='bg-success text-success-foreground'
        >
          <Icons.accepted className='mr-2 size-4' />
        </StatusContent>
      )
    case 'InternshipStarted':
      return (
        <StatusContent
          text={dictionary.InternshipStarted}
          colors='bg-primary text-primary-foreground'
        >
          <Icons.circle className='mr-2 size-4' />
        </StatusContent>
      )
    case 'DeclinedFromInternshipCommitte':
      return (
        <StatusContent
          text={dictionary.DeclinedFromInternshipCommitte}
          colors='bg-destructive text-destructive-foreground'
        >
          <Icons.declined className='mr-2 size-4' />
        </StatusContent>
      )
    case 'DeclinedFromCompany':
      return (
        <StatusContent
          text={dictionary.DeclinedFromCompany}
          colors='bg-destructive text-destructive-foreground'
        >
          <Icons.declined className='mr-2 size-4' />
        </StatusContent>
      )
    default:
      return (
        <StatusContent
          text={dictionary.DeclinedFromCompany}
          colors='bg-primary text-primary-foreground'
        >
          <Icons.info className='mr-2 size-4' />
        </StatusContent>
      )
  }
}

export default StatusBadge
