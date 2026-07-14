import { useAuthStore } from '../stores/authStore'

export default function Dashboard() {
  const { user } = useAuthStore()

  return (
    <div>
      <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
      <p className="mt-2 text-gray-600 dark:text-gray-300">
        Welcome back, {user?.fullName || user?.email}
      </p>

      <div className="mt-8 grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
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

        <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
            Recent meetings
          </h2>
          <p className="text-gray-600 dark:text-gray-400 text-sm">
            Your processed meetings will appear here.
          </p>
        </div>
      </div>
    </div>
  )
}
