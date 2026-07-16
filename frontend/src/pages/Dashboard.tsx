import { useEffect, useState } from 'react'
import UploadZone from '../components/UploadZone'
import { meetingService, type MeetingDto } from '../services/meetingService'

export default function Dashboard() {
  const [meetings, setMeetings] = useState<MeetingDto[]>([])
  const [isLoading, setIsLoading] = useState(true)

  const loadMeetings = async () => {
    try {
      const data = await meetingService.getMeetings()
      setMeetings(data)
    } catch {
      // TODO: show toast error
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    loadMeetings()
  }, [])

  return (
    <div>
      <h1 className="text-3xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
      <p className="mt-2 text-gray-600 dark:text-gray-300">
        Upload a meeting recording to get started.
      </p>

      <div className="mt-8 grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
            Upload a meeting
          </h2>
          <UploadZone onUploadComplete={loadMeetings} />
        </div>

        <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
            Recent meetings
          </h2>

          {isLoading ? (
            <p className="text-sm text-gray-500 dark:text-gray-400">Loading...</p>
          ) : meetings.length === 0 ? (
            <p className="text-sm text-gray-500 dark:text-gray-400">
              No meetings yet. Upload one to see it here.
            </p>
          ) : (
            <ul className="space-y-3">
              {meetings.map((meeting) => (
                <li
                  key={meeting.id}
                  className="flex items-center justify-between p-3 rounded-lg bg-gray-50 dark:bg-gray-700/50"
                >
                  <div>
                    <p className="text-sm font-medium text-gray-900 dark:text-white">
                      {meeting.title}
                    </p>
                    <p className="text-xs text-gray-500 dark:text-gray-400">
                      {new Date(meeting.createdAt).toLocaleString()}
                    </p>
                  </div>
                  <StatusBadge status={meeting.status} />
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  )
}

function StatusBadge({ status }: { status: string }) {
  const colors: Record<string, string> = {
    uploading: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/20 dark:text-yellow-300',
    uploaded: 'bg-blue-100 text-blue-700 dark:bg-blue-900/20 dark:text-blue-300',
    processing: 'bg-purple-100 text-purple-700 dark:bg-purple-900/20 dark:text-purple-300',
    completed: 'bg-green-100 text-green-700 dark:bg-green-900/20 dark:text-green-300',
    failed: 'bg-red-100 text-red-700 dark:bg-red-900/20 dark:text-red-300',
  }

  return (
    <span className={`px-2 py-1 rounded-full text-xs font-medium capitalize ${colors[status] || 'bg-gray-100 text-gray-700'}`}>
      {status}
    </span>
  )
}
