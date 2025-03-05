import React from 'react';
import favoriteIcon from '../img/Favorite.png';
import accountIcon from '../img/Account.png';
import cartIcon from '../img/Cart.png';

export default function Header({ cart, favorites, toggleCartModal, toggleFavoritesModal }) {
  return (
    <header className="header">
      <div className="header-content">
        <div className="logo">FastMarket</div>
        <button className="catalog-button" aria-label="Каталог">
          Каталог
        </button>
        <input
          type="text"
          className="search-field"
          placeholder="🔍 Пошук..."
          aria-label="Пошук"
        />
        <div className="icons">
          {/* Favorites Icon with Badge */}
          <button className="icon-button" aria-label="Обране" onClick={toggleFavoritesModal}>
            <img src={favoriteIcon} alt="Обране" />
            {favorites.length > 0 && <span className="badge">{favorites.length}</span>}
          </button>

          {/* Account Icon */}
          <button className="icon-button" aria-label="Акаунт">
            <img src={accountIcon} alt="Акаунт" />
          </button>

          {/* Cart Icon with Badge */}
          <button className="icon-button" aria-label="Кошик" onClick={toggleCartModal}>
            <img src={cartIcon} alt="Кошик" />
            {cart.length > 0 && <span className="badge">{cart.length}</span>}
          </button>
        </div>
      </div>
    </header>
  );
}