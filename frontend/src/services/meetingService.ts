import api from './api'
import type { Meeting } from '../types'

export interface MeetingDto {
  id: string
  title: string
  status: string
  createdAt: string
  durationSeconds?: number
  fileSizeBytes?: number
}

export const meetingService = {
  upload: (file: File, onProgress?: (progress: number) => void) => {
    const formData = new FormData()
    formData.append('file', file)

    return api.post<MeetingDto>('/meetings/upload', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
      onUploadProgress: (progressEvent) => {
        if (progressEvent.total && onProgress) {
          const progress = Math.round((progressEvent.loaded * 100) / progressEvent.total)
          onProgress(progress)
        }
      },
    }).then((res) => res.data)
  },

  getMeetings: () =>
    api.get<Meeting[]>('/meetings').then((res) => res.data),
}
