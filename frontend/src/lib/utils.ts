import { twentyFourHoursInMs } from '@/config/config'
import axios from 'axios'
import { type ClassValue, clsx } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

type CalendarApiType = {
  start: {
    date: Date
  }
  end: {
    date: Date
  }
}

export async function getPublicHolidays() {
  if (typeof process.env.NEXT_PUBLIC_CALENDAR_API_URL === 'undefined') {
    throw new Error(
      'Environment variable NEXT_PUBLIC_CALENDAR_API_URL is undefined'
    )
  }

  const { data, status } = await axios.get(
    process.env.NEXT_PUBLIC_CALENDAR_API_URL
  )

  if (status === 200) {
    const publicHolidays = extractStartDates(data.items) as Date[]
    const additionalDates = generateDatesFromTodayToNDaysFromNow(9)
    return publicHolidays.concat(additionalDates)
  }

  throw new Error('Fetching public holidays failed!')
}

function extractStartDates(items: any[]) {
  return items.map(item => {
    return new Date(item.start.date)
  })
}

function generateDatesFromTodayToNDaysFromNow(n: number) {
  const dates: Date[] = []
  const today = new Date()
  for (let i = 0; i <= n; i++) {
    const date = new Date(today)
    date.setDate(today.getDate() + i)
    dates.push(date)
  }
  return dates
}

export const getInternshipProgress = (
  start: Date,
  end: Date,
  total: number
): number => {
  const startDate = new Date(start).getTime()
  const endDate = new Date(end).getTime()

  const today = new Date().getTime()
  if (today < startDate) return 0
  const remainingDays = getWorkDays(new Date(), new Date(end));
  const progress = total - remainingDays
  if (progress > total) return total
  return progress
}

export const getTotalDays = (start: Date, end: Date): number => {
  const startDate = new Date(start).getTime()
  const endDate = new Date(end).getTime()
  return (endDate - startDate) / twentyFourHoursInMs
}


export const getWorkDays = (start: Date, end: Date) => {
  const startDate = new Date(start);
  const endDate = new Date(end);
  
  let workDaysCount = 0;

  for (let date = startDate; date <= endDate; date.setDate(date.getDate() + 1)) {
    const dayOfWeek = date.getDay();
    if (dayOfWeek !== 0 && dayOfWeek !== 6) { // 0 is Sunday, 6 is Saturday
      workDaysCount++;
    }
  }

  return workDaysCount;
};
