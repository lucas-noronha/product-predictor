import { useState } from "react";
import api from "../../services/api";
import "./Register.css";
import { Link, useNavigate } from "react-router-dom";

export default function RegisterPage({ goToLogin }) {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setSuccess(null);
    setLoading(true);

    try {
      const response = await api.post("/users/register", { name, email });
      setSuccess(response.data || "Usuário registrado com sucesso.");
      
      setTimeout(() => {
        navigate("/login");
      }, 800);

    } catch (err) {
      const msg =
        err?.response?.data ||
        err?.message ||
        "Erro ao registrar usuário.";
      setError(msg);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="container">
      <h1>Registrar</h1>

      <form onSubmit={handleSubmit} className="form">
        <label className="label">
          Nome
          <input
            type="text"
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
        {success && <p className="success">{success}</p>}

        <button type="submit" className="button" disabled={loading}>
          {loading ? "Registrando..." : "Registrar"}
        </button>
      </form>

      <Link className="linkButton" to="/login">
        Já tem conta? Fazer login
      </Link>
    </div>
  );
}
