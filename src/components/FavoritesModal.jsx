import React from 'react';
import cartIconGray from '../img/cartFavorite.png';
import cartFavoriteGreen from '../img/cartFavoriteGreen.png';
import trashIcon from '../img/Trash.png';


export default function FavoritesModal({
  favorites,
  toggleFavoritesModal,
  toggleFavorite,
  addToCart,
  cart,
  toggleCartModal
}) {
  const isInCart = (product) => cart.some(item => item.id === product.id);

  // Handler for "Add All to Cart" button
  const handleAddAllToCart = () => {
    favorites.forEach(item => {
      if (!isInCart(item)) {
        addToCart(item);
      }
    });
    // After adding all items, close favorites modal and open cart modal
    toggleFavoritesModal();
    toggleCartModal();
  };

  return (
    <div className="favorites-modal-overlay" onClick={toggleFavoritesModal}>
      <div className="favorites-modal" onClick={(e) => e.stopPropagation()}>
        <div className="favorites-modal-header">
          <h2>Обрані товари</h2>
          <button className="favorites-modal-close" onClick={toggleFavoritesModal}>
            ✕
          </button>
        </div>
        {favorites.length === 0 ? (
          <p className="empty-favorites">У вас немає обраних товарів.</p>
        ) : (
          <>
            <ul className="favorites-items-list">
              {favorites.map((item) => (
                <li key={item.id} className="favorites-item">
                  <img
                    className="favorites-item-image"
                    src={item.images[0]}
                    alt={item.title}
                  />
                  <div className="favorites-item-info">
                    <div className="favorites-item-title">{item.title}</div>
                    <div className="favorites-item-price">{item.price} грн</div>
                  </div>
                  <button
                    className="favorites-item-add-to-cart"
                    onClick={() => {
                      if (isInCart(item)) {
                        toggleCartModal();
                        toggleFavoritesModal();
                      } else {
                        addToCart(item);
                      }
                    }}
                  >
                    <img
                      src={isInCart(item) ? cartFavoriteGreen : cartIconGray}
                      alt="Add to cart"
                    />
                  </button>
                  <button
                    className="favorites-item-remove"
                    onClick={() => toggleFavorite(item)}
                  >
                    <img src={trashIcon} alt="Remove from favorites" />
                  </button>
                </li>
              ))}
            </ul>
            <button className="add-all-button" onClick={handleAddAllToCart}>
              Додати все у кошик
            </button>
          </>
        )}
      </div>
    </div>
  );
}