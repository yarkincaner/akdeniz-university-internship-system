import { FC } from 'react'
import UploadReport from './uploadReport'
import { getDictionary } from '@/lib/i18n/dictionary'

type ReportProps = {
  dictionary: Awaited<ReturnType<typeof getDictionary>>['internship']['report']
  internshipId: number
}

const Report: FC<ReportProps> = ({ dictionary, internshipId }) => {
  return (
    <div className='flex w-full flex-col items-center'>
      <div className='grid w-full grid-cols-2'>
        <div className='flex flex-1 flex-col rounded bg-secondary/50 p-4 shadow'>
          <h4 className='mb-4 text-2xl font-semibold'>
            {dictionary.reportLabel}
          </h4>
          <UploadReport internshipId={internshipId} />
        </div>
        <div className='flex flex-1 flex-col rounded bg-secondary/50 p-4 shadow'>
          <h4 className='mb-4 text-2xl font-semibold'>
            {dictionary.originalityReportLabel}
          </h4>
          <iframe className='aspect-[0.7/1]'></iframe>
        </div>
      </div>
    </div>
  )
}

export default Report
