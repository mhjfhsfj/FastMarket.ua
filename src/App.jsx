import { useState } from 'react';
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

  // Toggle modals
  const toggleCartModal = () => setCartModalOpen(prev => !prev);
  const toggleFavoritesModal = () => setFavoritesModalOpen(prev => !prev);

  // Add to cart: if product exists, add one; but later we override click behavior in Items.
  const addToCart = (product) => {
    setCart((prevCart) => {
      const existingIndex = prevCart.findIndex(item => item.id === product.id);
      if (existingIndex !== -1) {
        // If already in cart, you could update quantity if desired.
        // For this requirement, we won't increment—just open the cart modal.
        return prevCart;
      } else {
        return [...prevCart, { ...product, quantity: 1 }];
      }
    });
  };

  // Toggle favorite: add/remove from favorites.
  const toggleFavorite = (product) => {
    setFavorites((prevFavorites) => {
      const isFav = prevFavorites.some(item => item.id === product.id);
      return isFav 
        ? prevFavorites.filter(item => item.id !== product.id)
        : [...prevFavorites, product];
    });
  };

  return (
    <>
      <Header
        cart={cart}
        favorites={favorites}
        toggleCartModal={toggleCartModal}
        toggleFavoritesModal={toggleFavoritesModal}
      />

      {isCartModalOpen && (
        <CartModal
          cart={cart}
          toggleCartModal={toggleCartModal}
          /* other cart functions if needed */
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