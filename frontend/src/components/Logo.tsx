import { Link } from 'react-router-dom'

interface LogoProps {
  className?: string
}

export default function Logo({ className = '' }: LogoProps) {
  return (
    <Link to="/" className={`flex items-center gap-2 font-bold text-indigo-600 dark:text-indigo-400 ${className}`}>
      <svg
        className="h-8 w-8"
        viewBox="0 0 24 24"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M12 3C7.03 3 3 7.03 3 12V21H12C16.97 21 21 16.97 21 12C21 7.03 16.97 3 12 3ZM12 19H5V12C5 8.13 8.13 5 12 5C15.87 5 19 8.13 19 12C19 15.87 15.87 19 12 19Z"
          fill="currentColor"
        />
        <path
          d="M12 7C9.24 7 7 9.24 7 12H9C9 10.34 10.34 9 12 9C13.66 9 15 10.34 15 12H17C17 9.24 14.76 7 12 7Z"
          fill="currentColor"
        />
      </svg>
      <span className="text-xl tracking-tight">MeetMind</span>
    </Link>
  )
}
