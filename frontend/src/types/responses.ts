export type PagedResponse<T> = {
  pageNumber: number
  pageSize: number
  totalCount: number
  totalPages: number
  data: T[]
}

export type MyResponse<T> = {
  succeeded: boolean
  message: string
  data?: T
}
