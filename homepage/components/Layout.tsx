import Link from 'next/link'
import Image from 'next/image'
import logo from '../public/images/logo4-2.png'

export default function Layout({ children }: { children: React.ReactNode } ) {
  const whereIAm = process.env.NEXT_PUBLIC_WHEREIAM  
  const clientsUrl = process.env.NEXT_PUBLIC_CLIENTS_URL 

  return (
    <div>
      <nav className="navbar navbar-expand-lg navbar-dark tint-90">
        <div className="container">
          <Link href="/" className="navbar-brand">
            <Image src={logo} alt="Jericalla Mobile Logo" width={165} height={50} className="d-inline-block align-text-top" />
          </Link>
          <div className="collapse navbar-collapse">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <Link href="/" className="nav-link">Inicio</Link>
              </li>
              <li className="nav-item">
                <Link href="/plans" className="nav-link">Planes</Link>
              </li>
              <li className="nav-item">
                <Link href="/coverage" className="nav-link">Cobertura</Link>
              </li>
            </ul>
          </div>
          <a className="btn btn-outline-primary" href={`${clientsUrl}`}>Mi Cuenta</a>
        </div>
      </nav>

      <main className="container py-4">{children}</main>

      <footer className="bg-light text-center py-3 mt-auto">
        © 2025 Jericalla Mobile.
        <p>
          <span className="badge rounded-pill bg-success fixed-bottom">Hosted @ {whereIAm}</span>
        </p>
      </footer>
    </div>
  )
}
