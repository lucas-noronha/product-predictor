import { Link, useLocation } from "react-router-dom";
import "./Navbar.css";

export default function Navbar({ user, onLogout }) {
  const location = useLocation();
  const isLoggedIn = !!user;

  return (
    <header className="navbar">
      <div className="navbar-left">
        <Link
          to={isLoggedIn ? "/shopping-lists" : "/login"}
          className="navbar-logo"
        >
          ProductPrediction
        </Link>
      </div>

      <nav className="navbar-right">
        {!isLoggedIn && (
          <>
            <Link
              to="/login"
              className={
                "navbar-link" +
                (location.pathname === "/login" ? " active" : "")
              }
            >
              Login
            </Link>
            <Link
              to="/register"
              className={
                "navbar-link" +
                (location.pathname === "/register" ? " active" : "")
              }
            >
              Registrar
            </Link>
          </>
        )}

        {isLoggedIn && (
          <>
            <span className="navbar-user">Olá, {user.name}</span>

            <Link
              to="/shopping-lists"
              className={
                "navbar-link" +
                (location.pathname.startsWith("/shopping-lists")
                  ? " active"
                  : "")
              }
            >
              Minhas listas
            </Link>
            <Link
      to="/purchase-history"
      className={
        "navbar-link" +
        (location.pathname.startsWith("/purchase-history") ? " active" : "")
      }
    >
      Histórico
    </Link>
    <Link
      to="/predictions"
      className={
        "navbar-link" +
        (location.pathname.startsWith("/predictions") ? " active" : "")
      }
    >
      Sugestões
    </Link>

            <button className="navbar-button" onClick={onLogout}>
              Sair
            </button>
          </>
        )}
      </nav>
    </header>
  );
}
