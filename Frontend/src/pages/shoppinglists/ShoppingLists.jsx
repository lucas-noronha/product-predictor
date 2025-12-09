import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";
import "./ShoppingLists.css";

export default function ShoppingListsPage({ userId }) {
  const [lists, setLists] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [noContent, setNoContent] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (!userId) return;
    fetchLists();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [userId]);

  async function fetchLists() {
    setLoading(true);
    setError(null);
    setNoContent(false);

    try {
      const response = await api.get(`/ShoppingList/${userId}`);

      if (response.status === 204) {
        setLists([]);
        setNoContent(true);
        return;
      }

      setLists(response.data || []);
    } catch (err) {
      console.error(err);
      const msg =
        err?.response?.data ||
        err?.message ||
        "Erro ao carregar listas de compras.";
      setError(msg);
    } finally {
      setLoading(false);
    }
  }

  function handleOpenList(list) {
    navigate(`/shopping-lists/${list.id}`, { state: { list } });
  }

  return (
    <div className="shopping-lists-container">
      <h1>Minhas Shopping Lists</h1>

      {loading && <p>Carregando...</p>}
      {error && <p className="error">{error}</p>}
      {noContent && <p>Nenhuma lista encontrada.</p>}

      <div className="shopping-lists-grid">
        {lists.map((list) => (
          <div
            key={list.id}
            className="shopping-list-card"
            onClick={() => handleOpenList(list)}
          >
            <h2>{list.title || "Lista sem título"}</h2>
            <p>
              Itens: {Array.isArray(list.items) ? list.items.length : "-"}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}
