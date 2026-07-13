import { useEffect } from 'react'
import { authService } from '../services/authService'
import { useAuthStore } from '../stores/authStore'

export function useAuthInit() {
  const { setAuth, logout, setLoading } = useAuthStore()

  useEffect(() => {
    const init = async () => {
      const token = localStorage.getItem('accessToken')
      if (!token) {
        setLoading(false)
        return
      }

      try {
        const user = await authService.me()
        setAuth(user, token)
      } catch {
        logout()
      }
    }

    init()
  }, [setAuth, logout, setLoading])
}
