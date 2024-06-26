'use client'

import Icons from '@/components/Icons'
import Loader from '@/components/Loader'
import { useDictionary } from '@/components/dictionary-provider'
import { useUploadFile } from '@/lib/mutations'
import { cn } from '@/lib/utils'
import { DocumentTypeName } from '@/types/document'
import { MousePointerSquareDashed } from 'lucide-react'
import { FC, useState } from 'react'
import Dropzone, { FileRejection } from 'react-dropzone'
import { toast } from 'sonner'

type Props = {
  internshipId: number
}

const UploadReport: FC<Props> = ({ internshipId }) => {
  const {
    internship: { report: reportDictionary }
  } = useDictionary()
  const [isDragOver, setIsDragOver] = useState<boolean>(false)
  const [uploadProgress, setUploadProgress] = useState<number>(0)

  const onDropRejected = (rejectedFiles: FileRejection[]) => {
    const [file] = rejectedFiles
    setIsDragOver(false)
    toast.error(reportDictionary.onDropRejected.title, {
      description: `${file.file.type} ${reportDictionary.onDropRejected.description}`
    })
  }

  const onDropAccepted = (acceptedFiles: File[]) => {
    let formData = new FormData()
    formData.append('file', acceptedFiles[0])
    uploadFile(formData)
    setIsDragOver(false)
  }

  const { mutate: uploadFile, isLoading: isUploading } = useUploadFile(
    internshipId,
    DocumentTypeName.InternshipReport
  )

  return (
    <div
      className={cn(
        'relative flex h-full w-full flex-1 flex-col items-center justify-center rounded-xl bg-secondary/50 p-2 ring-1 ring-inset ring-foreground/5',
        {
          'bg-primary/10 ring-primary/25': isDragOver
        }
      )}
    >
      <Dropzone
        onDropRejected={onDropRejected}
        onDropAccepted={onDropAccepted}
        accept={{
          'application/pdf': ['.pdf']
        }}
        onDragEnter={() => setIsDragOver(true)}
        onDragLeave={() => setIsDragOver(false)}
      >
        {({ getRootProps, getInputProps }) => (
          <div
            className='flex size-full flex-1 flex-col items-center justify-center'
            {...getRootProps()}
          >
            <input {...getInputProps()} />
            {isDragOver ? (
              <MousePointerSquareDashed className='mb-2 size-6 text-muted-foreground' />
            ) : isUploading ? (
              <Loader />
            ) : (
              <Icons.file className='mb-2 size-6 text-muted-foreground' />
            )}

            <div className='mb-2 flex flex-col justify-center text-sm text-muted-foreground'>
              {isUploading ? (
                <div className='flex flex-col items-center'>
                  <p>{reportDictionary.uploading}</p>
                </div>
              ) : isDragOver ? (
                <p>{reportDictionary.dropFile}</p>
              ) : (
                <p>{reportDictionary.clickToUpload}</p>
              )}
            </div>

            {isUploading ? null : (
              <p className='text-sm text-muted-foreground'>
                {reportDictionary.supportedTypes}
              </p>
            )}
          </div>
        )}
      </Dropzone>
    </div>
  )
}

export default UploadReport
