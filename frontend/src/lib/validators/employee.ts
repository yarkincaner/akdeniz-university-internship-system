import { z } from 'zod'
import { Locale } from '../i18n/i18n.config'

export const EmployeeSchema = (lang: Locale) => {
  const messages = getErrorMessages(lang)

  return z.object({
    firstName: z.string({
      required_error: messages.firstName
    }),
    lastName: z.string({
      required_error: messages.lastName
    }),
    title: z.string({
      required_error: messages.title
    }),
    email: z
      .string({
        required_error: messages.email.required
      })
      .email({
        message: messages.email.type
      })
      .refine(async e => {
        //TODO: fetch emails from database and check if it includes e.
        return true //for now
      }),
    companyId: z.number()
  })
}

function getErrorMessages(lang: Locale) {
  if (lang === 'en') {
    return {
      firstName: 'first name is required!',
      lastName: 'last name is required!',
      title: 'title is required!',
      email: {
        required: 'e-mail is required!',
        type: 'invalid e-mail address!'
      }
    }
  }

  return {
    firstName: 'bu alan doldurulması zorunludur!',
    lastName: 'bu alan doldurulması zorunludur!',
    title: 'bu alan doldurulması zorunludur!',
    email: {
      required: 'e-posta adresi gereklidir!',
      type: 'geçersiz e-posta adresi!'
    }
  }
}
