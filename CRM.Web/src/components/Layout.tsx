import { NavLink, Outlet } from "react-router"
import { useAuth } from "@/features/auth/useAuth"
import { Button } from "@/components/ui/button"

const navItems = [
  { to: "/agenti", label: "Agenti" },
  { to: "/aziende-clienti", label: "Aziende Clienti" },
  { to: "/contatti", label: "Contatti" },
  { to: "/ordini", label: "Ordini" },
  { to: "/prodotti", label: "Prodotti" },
  { to: "/log-attivita", label: "Log Attività" },
]

export function Layout() {
  const { auth, logout } = useAuth()

  return (
    <div className="flex min-h-screen">
      <aside className="w-56 border-r p-4 flex flex-col justify-between">
        <nav className="space-y-1">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `block rounded px-3 py-2 text-sm ${
                  isActive ? "bg-primary text-primary-foreground" : "hover:bg-muted"
                }`
              }
            >
              {item.label}
            </NavLink>
          ))}
          {auth?.ruolo === "Admin" && (
            <NavLink
              to="/utenti"
              className={({ isActive }) =>
                `block rounded px-3 py-2 text-sm ${
                  isActive ? "bg-primary text-primary-foreground" : "hover:bg-muted"
                }`
              }
            >
              Utenti
            </NavLink>
          )}
        </nav>

        <div className="space-y-2 border-t pt-4">
          <p className="text-sm text-muted-foreground">{auth?.email}</p>
          <Button variant="outline" className="w-full" onClick={logout}>
            Esci
          </Button>
        </div>
      </aside>

      <main className="flex-1 p-8">
        <Outlet />
      </main>
    </div>
  )
}
