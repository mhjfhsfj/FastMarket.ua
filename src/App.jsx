import React, { useState } from 'react';
import './App.css';

import Header from "./components/Header";
import Footer from "./components/Footer";
import Menu from "./components/Menu";
import Items from "./components/Items";
import CartModal from "./components/CartModal";
import FavoritesModal from "./components/FavoritesModal";



function App() {
  const [selectedCategory, setSelectedCategory] = useState(null);

  // CART STATE & MODAL
  const [cart, setCart] = useState([]);
  const [isCartModalOpen, setCartModalOpen] = useState(false);

  // FAVORITES STATE & MODAL
  const [favorites, setFavorites] = useState([]);
  const [isFavoritesModalOpen, setFavoritesModalOpen] = useState(false);

  // ACCOUNT (LOGIN) MODAL STATE
  const [isAccountModalOpen, setAccountModalOpen] = useState(false);

  // Toggle modal functions
  const toggleCartModal = () => setCartModalOpen(prev => !prev);
  const toggleFavoritesModal = () => setFavoritesModalOpen(prev => !prev);
  const toggleAccountModal = () => setAccountModalOpen(prev => !prev);

  // Functions to increment, decrement, and remove items in the cart
  const incrementQuantity = (productId) => {
    setCart(prevCart =>
      prevCart.map(item =>
        item.id === productId ? { ...item, quantity: item.quantity + 1 } : item
      )
    );
  };

  const decrementQuantity = (productId) => {
    setCart(prevCart =>
      prevCart.map(item => {
        if (item.id === productId && item.quantity > 1) {
          return { ...item, quantity: item.quantity - 1 };
        }
        return item;
      })
    );
  };

  const removeFromCart = (productId) => {
    setCart(prevCart => prevCart.filter(item => item.id !== productId));
  };

  // Add to cart: if product is already in cart, do nothing.
  const addToCart = (product) => {
    setCart(prevCart => {
      if (prevCart.some(item => item.id === product.id)) {
        return prevCart;
      } else {
        return [...prevCart, { ...product, quantity: 1 }];
      }
    });
  };

  // Toggle favorite: add or remove product from favorites.
  const toggleFavorite = (product) => {
    setFavorites(prevFavorites => {
      if (prevFavorites.some(item => item.id === product.id)) {
        return prevFavorites.filter(item => item.id !== product.id);
      } else {
        return [...prevFavorites, product];
      }
    });
  };

  return (
    <>
      <Header
        cart={cart}
        favorites={favorites}
        toggleCartModal={toggleCartModal}
        toggleFavoritesModal={toggleFavoritesModal}
        toggleAccountModal={toggleAccountModal}
      />

      {isCartModalOpen && (
        <CartModal
          cart={cart}
          toggleCartModal={toggleCartModal}
          incrementQuantity={incrementQuantity}
          decrementQuantity={decrementQuantity}
          removeFromCart={removeFromCart}
        />
      )}

      {isFavoritesModalOpen && (
        <FavoritesModal
          favorites={favorites}
          toggleFavoritesModal={toggleFavoritesModal}
          toggleFavorite={toggleFavorite}
          addToCart={addToCart}
          cart={cart}
          toggleCartModal={toggleCartModal}
        />
      )}

      {isAccountModalOpen && (
        <AccountModal toggleAccountModal={toggleAccountModal} />
      )}

      <div className="main-content">
        <Menu onSelectCategory={setSelectedCategory} />
        <Items
          selectedCategory={selectedCategory}
          addToCart={addToCart}
          toggleFavorite={toggleFavorite}
          favorites={favorites}
          cart={cart}
          toggleCartModal={toggleCartModal}
        />
      </div>
      <Footer />
    </>
  );
}

export default App;