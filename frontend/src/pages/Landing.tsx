import { Link } from 'react-router-dom'
import Logo from '../components/Logo'

export default function Landing() {
  return (
    <div className="min-h-screen bg-white dark:bg-gray-900">
      {/* Navbar */}
      <nav className="border-b border-gray-200 dark:border-gray-800">
        <div className="mx-auto max-w-7xl px-4 lg:px-8">
          <div className="flex h-16 items-center justify-between">
            <Logo />
            <div className="flex items-center gap-4">
              <Link
                to="/login"
                className="hidden sm:inline-flex text-sm font-medium text-gray-700 dark:text-gray-200 hover:text-indigo-600"
              >
                Sign in
              </Link>
              <Link
                to="/register"
                className="rounded-lg bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 transition-colors"
              >
                Get started
              </Link>
            </div>
          </div>
        </div>
      </nav>

      {/* Hero */}
      <section className="mx-auto max-w-7xl px-4 py-20 lg:py-32 lg:px-8 text-center">
        <h1 className="text-4xl font-extrabold tracking-tight text-gray-900 dark:text-white sm:text-6xl">
          Turn meetings into
          <span className="text-indigo-600 dark:text-indigo-400"> action</span>
        </h1>
        <p className="mx-auto mt-6 max-w-2xl text-lg text-gray-600 dark:text-gray-300">
          Upload a meeting recording and get transcripts, AI summaries, extracted action items,
          and searchable insights in minutes.
        </p>
        <div className="mt-10 flex justify-center gap-4">
          <Link
            to="/register"
            className="rounded-xl bg-indigo-600 px-6 py-3 text-base font-semibold text-white hover:bg-indigo-700 transition-colors"
          >
            Start for free
          </Link>
          <Link
            to="/login"
            className="rounded-xl border border-gray-300 dark:border-gray-700 px-6 py-3 text-base font-semibold text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors"
          >
            Sign in
          </Link>
        </div>
      </section>

      {/* Features */}
      <section className="bg-gray-50 dark:bg-gray-800/50 py-20">
        <div className="mx-auto max-w-7xl px-4 lg:px-8">
          <h2 className="text-center text-3xl font-bold text-gray-900 dark:text-white">
            Everything you need after a meeting
          </h2>
          <div className="mt-12 grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3">
            <FeatureCard
              title="AI Transcription"
              description="Whisper-powered transcripts with speaker labels and timestamps."
            />
            <FeatureCard
              title="Smart Summaries"
              description="GPT-4o generates concise summaries and key decisions."
            />
            <FeatureCard
              title="Action Items"
              description="Extracted tasks with owners and deadlines, synced to a Kanban board."
            />
            <FeatureCard
              title="Semantic Search"
              description="Find meetings by meaning, not just keywords, with pgvector."
            />
            <FeatureCard
              title="Team Workspace"
              description="Share meetings, comment, and collaborate with your team."
            />
            <FeatureCard
              title="Export Anywhere"
              description="Export to PDF, Markdown, or copy to your favorite tools."
            />
          </div>
        </div>
      </section>

      {/* Pricing */}
      <section className="py-20">
        <div className="mx-auto max-w-7xl px-4 lg:px-8">
          <h2 className="text-center text-3xl font-bold text-gray-900 dark:text-white">
            Simple pricing
          </h2>
          <div className="mt-12 grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4">
            <PricingCard
              name="Free"
              price="$0"
              description="3 meetings/month · 30 min max · Basic summary"
            />
            <PricingCard
              name="Pro"
              price="$15"
              period="/month"
              description="20 meetings · 2 hours max · Full features"
              highlighted
            />
            <PricingCard
              name="Team"
              price="$49"
              period="/month"
              description="Unlimited · Team workspace · Analytics"
            />
            <PricingCard
              name="Enterprise"
              price="Custom"
              description="SSO · Custom vocabulary · SLA · API"
            />
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t border-gray-200 dark:border-gray-800 py-12">
        <div className="mx-auto max-w-7xl px-4 lg:px-8 text-center text-sm text-gray-500 dark:text-gray-400">
          © {new Date().getFullYear()} MeetMind AI. All rights reserved.
        </div>
      </footer>
    </div>
  )
}

function FeatureCard({ title, description }: { title: string; description: string }) {
  return (
    <div className="rounded-2xl bg-white dark:bg-gray-800 p-6 shadow-sm border border-gray-100 dark:border-gray-700">
      <h3 className="text-lg font-semibold text-gray-900 dark:text-white">{title}</h3>
      <p className="mt-2 text-gray-600 dark:text-gray-300">{description}</p>
    </div>
  )
}

function PricingCard({
  name,
  price,
  period,
  description,
  highlighted,
}: {
  name: string
  price: string
  period?: string
  description: string
  highlighted?: boolean
}) {
  return (
    <div
      className={`rounded-2xl p-6 border ${
        highlighted
          ? 'border-indigo-600 bg-indigo-50 dark:bg-indigo-900/20'
          : 'border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800'
      }`}
    >
      <h3 className="text-lg font-semibold text-gray-900 dark:text-white">{name}</h3>
      <div className="mt-4 flex items-baseline">
        <span className="text-3xl font-bold text-gray-900 dark:text-white">{price}</span>
        {period && <span className="text-gray-500 dark:text-gray-400 ml-1">{period}</span>}
      </div>
      <p className="mt-4 text-sm text-gray-600 dark:text-gray-300">{description}</p>
      <Link
        to="/register"
        className={`mt-6 block w-full rounded-lg px-4 py-2 text-center text-sm font-medium transition-colors ${
          highlighted
            ? 'bg-indigo-600 text-white hover:bg-indigo-700'
            : 'bg-gray-100 dark:bg-gray-700 text-gray-900 dark:text-white hover:bg-gray-200 dark:hover:bg-gray-600'
        }`}
      >
        Get started
      </Link>
    </div>
  )
}
