function App() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-br from-indigo-50 to-white dark:from-gray-900 dark:to-gray-800 px-6">
      <h1 className="text-5xl font-bold text-indigo-700 dark:text-indigo-400 mb-4">
        MeetMind AI
      </h1>
      <p className="text-lg text-gray-600 dark:text-gray-300 max-w-xl text-center">
        Smart Meeting Notes — Transcribe, Summarize, Extract Action Items.
      </p>
      <div className="mt-8 p-6 bg-white dark:bg-gray-800 rounded-2xl shadow-lg max-w-md w-full text-center">
        <p className="text-sm text-gray-500 dark:text-gray-400">
          Upload a meeting recording. Get transcripts, summaries, and action items in minutes.
        </p>
        <button
          type="button"
          className="mt-6 px-6 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white rounded-lg font-medium transition-colors cursor-pointer"
        >
          Get Started
        </button>
      </div>
    </div>
  )
}

export default App
