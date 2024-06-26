import { z } from 'zod'
import { Locale } from '../i18n/i18n.config'

export const InternshipSchema = (lang: Locale) => {
  const messages = getErrorMessages(lang)

  return z.object({
    companyId: z.number({
      required_error: messages.companyId
    }),
    employeeId: z.number({
      required_error: messages.employeeId
    }),
    startDate: z.date({
      required_error: messages.startDate
    }),
    endDate: z.date({
      required_error: messages.endDate
    }),
    totalDays: z.coerce
      .number({
        required_error: messages.totalDays
      })
      .min(1)
      .max(60),
    insuranceType: z.enum(['school', 'company'])
  })
}

export const EditInternshipSchema = (lang: Locale) => {
  const messages = getErrorMessages(lang)

  return z.object({
    employeeId: z.number({
      required_error: messages.employeeId
    }),
    startDate: z.date({
      required_error: messages.startDate
    }),
    endDate: z.date({
      required_error: messages.endDate
    }),
    totalDays: z.coerce
      .number({
        required_error: messages.totalDays
      })
      .min(1)
      .max(60)
  })
}

function getErrorMessages(lang: Locale) {
  if (lang === 'en') {
    return {
      companyId: 'You must select a company!',
      employeeId: 'You must select an employee!',
      startDate: 'A starting date is required.',
      endDate: 'An ending date is required.',
      totalDays: 'Total days is required.'
    }
  }

  return {
    companyId: 'Şirket seçmeniz gerekmektedir!',
    employeeId: 'İşveren seçmeniz gerekmektedir!',
    startDate: 'Başlangıç günü girilmedi!',
    endDate: 'Bitiş günü girilmedi!',
    totalDays: 'Toplam gün girilmedi!'
  }
}
