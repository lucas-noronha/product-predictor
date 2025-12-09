import api from "../../services/api"; // ou o caminho certo do seu axios configurado
import { useEffect, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import "./ShoppingListDetails.css";

export default function ShoppingListDetailsPage() {
  const { state } = useLocation();
  const { id } = useParams();
  const navigate = useNavigate();

  const shoppingList = state?.list || null;

  const [itemsState, setItemsState] = useState([]);
  const [newItem, setNewItem] = useState("");

  useEffect(() => {
    if (!shoppingList) return;

    const originalItems = Array.isArray(shoppingList.items)
      ? shoppingList.items
      : [];

    setItemsState(
      originalItems.map((name) => ({
        name,
        checked: false,
      }))
    );
  }, [shoppingList]);

  useEffect(() => {
  if (!shoppingList) return;

  // monta um array só com os nomes
  const items = itemsState.map(i => i.name);

  // faz a request
  api.patch(`/ShoppingList/update_items/${id}`, {
    title: shoppingList.title,
    items
  }).catch(err => {
    console.error("Erro ao atualizar lista:", err);
  });

}, [itemsState]);

  function handleToggle(index) {
    setItemsState((prev) =>
      prev.map((item, i) =>
        i === index ? { ...item, checked: !item.checked } : item
      )
    );
  }

  function handleAddItem(e) {
    e.preventDefault();
    const trimmed = newItem.trim();
    if (!trimmed) return;

    setItemsState((prev) => [...prev, { name: trimmed, checked: false }]);
    setNewItem("");
  }

  function handleRemoveItem(index) {
    setItemsState((prev) => prev.filter((_, i) => i !== index));
  }

  async function handleFinishPurchase() {
  const checkedItems = itemsState
    .filter((i) => i.checked)
    .map((i) => i.name);

  if (checkedItems.length === 0) {
    alert("Nenhum item marcado para finalizar a compra.");
    return;
  }

  // pega o usuário corretamente
  const storedUser = localStorage.getItem("pp_user");
  const user = storedUser ? JSON.parse(storedUser) : null;
  const userId = user?.id;

  if (!userId) {
    alert("Usuário não encontrado (pp_user ausente).");
    return;
  }

  try {
    const payload = {
      userId: userId,
      date: new Date().toISOString(),
      items: checkedItems
    };

    await api.post("/purchase", payload);

    alert("Compra registrada com sucesso!");
  } catch (err) {
    console.error("Erro ao registrar compra:", err);
    alert("Erro ao registrar a compra.");
  }
}


  if (!shoppingList) {
    return (
      <div className="shopping-details-container">
        <button className="back-button" onClick={() => navigate("/shopping-lists")}>
          ← Voltar
        </button>
        <p>Lista não encontrada (abra pela tela de listas).</p>
      </div>
    );
  }

  return (
    <div className="shopping-details-container">
      <button className="back-button" onClick={() => navigate("/shopping-lists")}>
        ← Voltar
      </button>

      <h1>{shoppingList.title || "Shopping List"}</h1>

      <form className="add-item-form" onSubmit={handleAddItem}>
        <input
          type="text"
          placeholder="Novo item..."
          value={newItem}
          onChange={(e) => setNewItem(e.target.value)}
          className="add-item-input"
        />
        <button type="submit" className="add-item-button">
          Adicionar
        </button>
      </form>

      <ul className="items-list">
        {itemsState.map((item, index) => (
          <li key={index} className="item-row">
            <label className="item-label">
              <input
                type="checkbox"
                checked={item.checked}
                onChange={() => handleToggle(index)}
              />
              <span className={item.checked ? "item-name checked" : "item-name"}>
                {item.name}
              </span>
            </label>
            <button
              type="button"
              className="remove-item-button"
              onClick={() => handleRemoveItem(index)}
            >
              Remover
            </button>
          </li>
        ))}

        {itemsState.length === 0 && (
          <li className="empty-message">Nenhum item na lista.</li>
        )}
      </ul>

      <button
        type="button"
        className="finish-button"
        onClick={handleFinishPurchase}
      >
        Finalizar compra
      </button>
    </div>
  );
}
