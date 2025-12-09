import { useState } from "react";
import "./Login.css";
import { Link } from "react-router-dom";
import api from "../../services/api";

export default function LoginPage({ onLoginSuccess, goToRegister }) {
  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await api.post("/users/login", { name, email });
      const user = response.data;

      onLoginSuccess && onLoginSuccess(user);
      alert(`Logado como: ${user.name}`);
    } catch (err) {
      const msg =
        err?.response?.data ||
        err?.message ||
        "Erro ao fazer login.";
      setError(msg);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="container">
      <h1>Login</h1>

      <form onSubmit={handleSubmit} className="form">
        <label className="label">
          Name
          <input
            type="Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="input"
            required
          />
        </label>
        <label className="label">
          Email
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="input"
            required
          />
        </label>

        {error && <p className="error">{error}</p>}

        <button type="submit" className="button" disabled={loading}>
          {loading ? "Entrando..." : "Entrar"}
        </button>
      </form>

      <Link className="linkButton" to="/register">
        Não tem conta? Registrar
      </Link>
    </div>
  );
}
