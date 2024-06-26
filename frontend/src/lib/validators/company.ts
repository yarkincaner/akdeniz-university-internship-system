import { isValidPhoneNumber } from 'react-phone-number-input'
import { z } from 'zod'
import { Locale } from '../i18n/i18n.config'

const taxRegex = new RegExp(/^\+?[0-9]{6,}$/)

export const CompanySchema = (lang: Locale) => {
  const messages = getErrorMessages(lang)

  return z.object({
    name: z.string({
      required_error: messages.name
    }),
    serviceArea: z.string({
      required_error: messages.serviceArea
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
    website: z
      .string({
        required_error: messages.website.required
      })
      .url({
        message: messages.website.type
      }),
    phone: z
      .string({
        required_error: messages.phone.required
      })
      .refine(isValidPhoneNumber, {
        message: messages.phone.type
      }),
    taxNumber: z
      .string({
        required_error: messages.taxNumber.required
      })
      .regex(taxRegex, {
        message: messages.taxNumber.type
      }),
    address: z.string({
      required_error: messages.address
    })
  })
}

function getErrorMessages(lang: Locale) {
  if (lang === 'en') {
    return {
      name: 'name is required!',
      serviceArea: 'service area is required!',
      email: {
        required: 'e-mail is required!',
        type: 'must be a valid e-mail address!'
      },
      website: {
        required: 'web address is required!',
        type: 'must be a valid url!'
      },
      phone: {
        required: 'phone number is required!',
        type: 'invalid phone number!'
      },
      taxNumber: {
        required: 'tax number is required!',
        type: 'invalid tax number!'
      },
      address: 'address is required!'
    }
  }

  return {
    name: 'şirket ismi gerekli!',
    serviceArea: 'üretim/hizmet alanı gerekli!',
    email: {
      required: 'e-posta adresi gerekli!',
      type: 'geçerli bir e-posta adresi giriniz!'
    },
    website: {
      required: 'web adresi gerekli!',
      type: 'geçerli bir url giriniz!'
    },
    phone: {
      required: 'telefon numarası gerekli!',
      type: 'geçerli bir telefon numarası giriniz!'
    },
    taxNumber: {
      required: 'vergi no giriniz!',
      type: 'geçerli bir vergi no giriniz!'
    },
    address: 'adres gerekli!'
  }
}
