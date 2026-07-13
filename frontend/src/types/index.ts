export interface User {
  id: string
  email: string
  fullName?: string
  subscriptionTier: string
}

export interface Meeting {
  id: string
  title: string
  status: 'uploading' | 'processing' | 'completed' | 'failed'
  createdAt: string
  durationSeconds?: number
}
