// src/App.jsx
import { useEffect, useState } from "react";
import { Routes, Route, Navigate, useNavigate } from "react-router-dom";
import Navbar from "./components/Navbar";
import LoginPage from "./pages/login/Login";
import ShoppingListsPage from "./pages/shoppinglists/ShoppingLists";
import ShoppingListDetailsPage from "./pages/shoppinglists-details/ShoppingListDetails";
import RegisterPage from "./pages/register/Register";
import PurchaseHistoryPage from "./pages/purchase-history/PurchaseHistory";
import PredictionsPage from "./pages/prediction/Prediction";



function App() {
  const navigate = useNavigate();

  // carrega usuário do localStorage na inicialização
  const [user, setUser] = useState(() => {
    try {
      const stored = localStorage.getItem("pp_user");
      return stored ? JSON.parse(stored) : null;
    } catch {
      return null;
    }
  });

  // sempre que o user mudar, sincroniza com o localStorage
  useEffect(() => {
    if (user) {
      localStorage.setItem("pp_user", JSON.stringify(user));
    } else {
      localStorage.removeItem("pp_user");
    }
  }, [user]);

  function handleLoginSuccess(u) {
    setUser(u);
    navigate("/shopping-lists");
  }

  function handleLogout() {
    setUser(null);
    navigate("/login");
  }

  return (
    <>
      <Navbar user={user} onLogout={handleLogout} />

      <Routes>
        {/* Redireciona raiz para /login ou /shopping-lists dependendo do login */}
        <Route
          path="/"
          element={
            user ? (
              <Navigate to="/shopping-lists" replace />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />

        <Route
          path="/login"
          element={
            user ? (
              <Navigate to="/shopping-lists" replace />
            ) : (
              <LoginPage onLoginSuccess={handleLoginSuccess} />
            )
          }
        />

        <Route
          path="/register"
          element={
            user ? <Navigate to="/shopping-lists" replace /> : <RegisterPage />
          }
        />

        {/* Rotas protegidas */}
        <Route
  path="/predictions"
  element={
    user ? (
      <PredictionsPage userId={user.id} />
    ) : (
      <Navigate to="/login" replace />
    )
  }
/>
        <Route
          path="/shopping-lists"
          element={
            user ? (
              <ShoppingListsPage userId={user.id} />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />
<Route
  path="/purchase-history"
  element={
    user ? (
      <PurchaseHistoryPage userId={user.id} />
    ) : (
      <Navigate to="/login" replace />
    )
  }
/>
        <Route
          path="/shopping-lists/:id"
          element={
            user ? (
              <ShoppingListDetailsPage />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />

        <Route path="*" element={<p style={{ padding: 24 }}>Página não encontrada</p>} />
      </Routes>
    </>
  );
}

export default App;
