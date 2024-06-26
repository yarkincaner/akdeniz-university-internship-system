'use client'

import Icons from '@/components/Icons'
import PDFView from '@/components/PDFView'
import Paginator from '@/components/Paginator'
import Search from '@/components/Search'
import { DataTable } from '@/components/data-table/data-table'
import adminRouteWrapper from '@/components/route-wrappers/AdminRouteWrapper'
import { Button, buttonVariants } from '@/components/ui/button'
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger
} from '@/components/ui/tooltip'
import { TOOLTIP_DELAY } from '@/config/config'
import { useDownloadAsSpreadsheet } from '@/lib/mutations'
import { useGetAllInternships } from '@/lib/queries'
import { BlobProvider } from '@react-pdf/renderer'
import { RowSelectionState } from '@tanstack/react-table'
import { FC, Suspense, useState } from 'react'
import { Columns } from './columns'

type Props = {
  searchParams?: {
    SearchString?: string
    PageNumber?: string
    PageSize?: string
  }
}

const Content: FC<Props> = ({ searchParams }) => {
  const { data, isLoading, error } = useGetAllInternships({
    searchString: searchParams?.SearchString,
    pageNumber: searchParams?.PageNumber,
    pageSize: searchParams?.PageSize
  })

  const [selectedRows, setSelectedRows] = useState<RowSelectionState>({})

  const { mutate: downloadAsSpreadsheet, isLoading: isDownloading } =
    useDownloadAsSpreadsheet()

  if (error) return <h1>Error</h1>
  return (
    <>
      <div className='flex items-center justify-between py-4'>
        <Suspense>
          <Search
            placeholder='Search by name...'
            paramName='SearchString'
            pageParamName='PageNumber'
          />
        </Suspense>
        <div className='flex items-center gap-4'>
          <TooltipProvider delayDuration={TOOLTIP_DELAY}>
            <Tooltip>
              <TooltipTrigger asChild>
                <Button
                  variant={'outline'}
                  size={'icon'}
                  onClick={e => {
                    e.preventDefault()
                    downloadAsSpreadsheet(
                      Object.keys(selectedRows).map(row => {
                        return Number(row)
                      })
                    )
                  }}
                >
                  <Icons.spreadSheet className='size-4' />
                </Button>
              </TooltipTrigger>
              <TooltipContent>
                <p>Download spreadsheet</p>
              </TooltipContent>
            </Tooltip>
          </TooltipProvider>
          {isLoading ? (
            <></>
          ) : (
            <BlobProvider
              document={<PDFView internship={data ? data.data : []} />}
            >
              {({ url, ...rest }) => (
                <TooltipProvider delayDuration={TOOLTIP_DELAY}>
                  <Tooltip>
                    <TooltipTrigger asChild>
                      <a
                        className={buttonVariants({
                          variant: 'outline',
                          size: 'icon'
                        })}
                        href={url!}
                        target='_blank'
                      >
                        <Icons.file className='size-4' />
                      </a>
                    </TooltipTrigger>
                    <TooltipContent>
                      <p>View as PDF</p>
                    </TooltipContent>
                  </Tooltip>
                </TooltipProvider>
              )}
            </BlobProvider>
          )}
          <Suspense>
            <Paginator
              pageNumber={
                searchParams?.PageNumber ? searchParams.PageNumber : '1'
              }
              totalPages={data ? data.totalPages : 1}
              pageParamName='PageNumber'
              isLoading={isLoading}
            />
          </Suspense>
        </div>
      </div>
      <DataTable
        columns={Columns}
        data={data ? data.data : []}
        isLoading={isLoading}
        rowSelection={selectedRows}
        setRowSelection={setSelectedRows}
      />
    </>
  )
}

export default adminRouteWrapper(Content)
