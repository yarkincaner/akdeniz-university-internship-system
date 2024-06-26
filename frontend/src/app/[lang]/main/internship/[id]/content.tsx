'use client'

import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { FC } from 'react'
import Report from './report'
import Tally from './tally'
import { useDictionary } from '@/components/dictionary-provider'
import History from './history'

type Props = {
  internshipId: number
}

const Content: FC<Props> = ({ internshipId }) => {
  const {
    internship: { report, tabs }
  } = useDictionary()

  // this will be enabled after internships end 
  const documentLoad = () => {
    return (
      <Tabs defaultValue='report' className='col-span-3'>
        <TabsList className='mx-auto grid w-[250px] grid-cols-2 md:w-[400px]'>
          <TabsTrigger value='report'>{tabs.report}</TabsTrigger>
          <TabsTrigger value='tally'>{tabs.tally}</TabsTrigger>
        </TabsList>
        <TabsContent value='report'>
          <Report dictionary={report} internshipId={internshipId} />
        </TabsContent>
        <TabsContent value='tally'>
          <Tally />
        </TabsContent>
      </Tabs>
    );
  }


  return (
    <div className='w-full grid grid-cols-1 xl:grid-cols-12'>
      <div className='col-span-12 w-full'>
        <History internshipId={internshipId} />
      </div>
    </div>
  )
}

export default Content
