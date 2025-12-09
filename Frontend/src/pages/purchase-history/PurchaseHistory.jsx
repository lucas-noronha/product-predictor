import { useEffect, useState } from "react";
import "./PurchaseHistory.css";
import api from "../../services/api";

export default function PurchaseHistoryPage({ userId }) {
  const [purchases, setPurchases] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchPurchases() {
      try {
        setLoading(true);
        setError(null);

        const response = await api.get(`/purchase/${userId}`);

        if (response.status === 204) {
          // NoContent
          setPurchases([]);
        } else {
          setPurchases(response.data || []);
        }
      } catch (err) {
        console.error("Erro ao carregar histórico de compras:", err);
        setError("Erro ao carregar histórico de compras.");
      } finally {
        setLoading(false);
      }
    }

    if (userId) {
      fetchPurchases();
    }
  }, [userId]);

  function formatDate(dateStr) {
    if (!dateStr) return "-";
    const date = new Date(dateStr);
    return date.toLocaleString("pt-BR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  }

  return (
    <div className="purchase-history-container">
      <h1 className="purchase-history-title">Histórico de compras</h1>
      <p className="purchase-history-subtitle">
        Aqui você vê as compras registradas ao finalizar uma lista.
      </p>

      {loading && <p className="purchase-history-info">Carregando...</p>}

      {error && <p className="purchase-history-error">{error}</p>}

      {!loading && !error && purchases.length === 0 && (
        <p className="purchase-history-empty">
          Você ainda não tem compras registradas.
        </p>
      )}

      {!loading && !error && purchases.length > 0 && (
        <ul className="purchase-list">
          {purchases
            .slice()
            .sort(
              (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
            )
            .map((purchase) => (
              <li key={purchase.id} className="purchase-card">
                <div className="purchase-card-header">
                  <span className="purchase-date">
                    {formatDate(purchase.date)}
                  </span>
                  <span className="purchase-id">
                    ID: {purchase.id.substring(0, 8)}...
                  </span>
                </div>

                <div className="purchase-items">
                  <h3>Itens comprados ({purchase.items.length}):</h3>
                  {purchase.items.length === 0 ? (
                    <p className="purchase-no-items">
                      Nenhum item registrado nesta compra.
                    </p>
                  ) : (
                    <ul>
                      {purchase.items.map((item, idx) => (
                        <li key={idx}>{item}</li>
                      ))}
                    </ul>
                  )}
                </div>
              </li>
            ))}
        </ul>
      )}
    </div>
  );
}
