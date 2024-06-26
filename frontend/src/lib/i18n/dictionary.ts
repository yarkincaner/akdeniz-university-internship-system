import type { Locale } from './i18n.config'

const dictionaries = {
  tr: () =>
    import('@/lib/i18n/dictionaries/tr.json').then(module => module.default),
  en: () =>
    import('@/lib/i18n/dictionaries/en.json').then(module => module.default)
}

export const getDictionary = async (locale: Locale) => dictionaries[locale]()
