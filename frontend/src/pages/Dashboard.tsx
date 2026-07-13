import { useAuthStore } from '../stores/authStore'

export default function Dashboard() {
  const { user, logout } = useAuthStore()

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 p-8">
      <div className="max-w-4xl mx-auto">
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
            <p className="text-gray-600 dark:text-gray-300 mt-1">
              Welcome back, {user?.fullName || user?.email}
            </p>
          </div>
          <button
            onClick={logout}
            className="px-4 py-2 rounded-lg bg-red-600 hover:bg-red-700 text-white font-medium transition-colors"
          >
            Sign out
          </button>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="bg-white dark:bg-gray-800 rounded-2xl shadow p-6">
            <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
              Upload a meeting
            </h2>
            <p className="text-gray-600 dark:text-gray-400 text-sm">
              Drag and drop your audio file here to get started.
            </p>
            <div className="mt-4 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-xl p-8 text-center">
              <span className="text-gray-500 dark:text-gray-400">Upload zone coming soon</span>
            </div>
          </div>

          <div className="bg-white dark:bg-gray-800 rounded-2xl shadow p-6">
            <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
              Recent meetings
            </h2>
            <p className="text-gray-600 dark:text-gray-400 text-sm">
              Your processed meetings will appear here.
            </p>
          </div>
        </div>
      </div>
    </div>
  )
}
