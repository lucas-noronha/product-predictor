import { useEffect, useState } from "react";
import api from "../../services/api";
import { addMessagesToPredictions } from "../../utils/predictionMessages";
import "./Prediction.css";

export default function PredictionsPage({ userId }) {
  const [predictions, setPredictions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchPredictions() {
      try {
        setLoading(true);
        setError(null);

        const response = await api.get(`/prediction/history/${userId}`);
const enriched = addMessagesToPredictions(response.data || []);

// Ordena: alertas primeiro
const sorted = enriched.sort((a, b) => {
  const aHasMsg = !!a.message;
  const bHasMsg = !!b.message;

  if (aHasMsg && !bHasMsg) return -1;
  if (!aHasMsg && bHasMsg) return 1;
  return 0;
});

setPredictions(sorted);
        
      } catch (err) {
        console.error("Erro ao carregar previsões:", err);
        setError("Erro ao carregar previsões.");
      } finally {
        setLoading(false);
      }
    }

    if (userId) {
      fetchPredictions();
    }
  }, [userId]);

  return (
    <div className="predictions-container">
      <h1 className="predictions-title">Sugestões de compra</h1>
      <p className="predictions-subtitle">
        Baseadas no seu histórico de compras, a IA estima quais itens podem
        precisar de reposição em breve.
      </p>

      {loading && <p className="predictions-info">Carregando previsões...</p>}

      {error && <p className="predictions-error">{error}</p>}

      {!loading && !error && predictions.length === 0 && (
        <p className="predictions-empty">
          Ainda não há dados suficientes para gerar sugestões.
        </p>
      )}

      {!loading && !error && predictions.length > 0 && (
        <ul className="predictions-list">
          {predictions.map((p, idx) => {
            const hasMessage = !!p.message;
            return (
              <li
                key={idx}
                className={
                  "prediction-card" +
                  (hasMessage ? " prediction-card-highlight" : "")
                }
              >
                <div className="prediction-header">
                  <span className="prediction-item">{p.item}</span>

                  {p.predictedDate && (
                    <span className="prediction-date-tag">
                      Previsto para{" "}
                      {new Date(p.predictedDate).toLocaleDateString("pt-BR")}
                    </span>
                  )}
                </div>

                {hasMessage && (
                  <p className="prediction-message">{p.message}</p>
                )}

                {p.note && (
                  <p className="prediction-note">
                    {p.note}
                  </p>
                )}

                {!hasMessage && (
                  <p className="prediction-subtle">
                    Sem alerta imediato para este item, mas ele faz parte das
                    suas previsões.
                  </p>
                )}
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}
