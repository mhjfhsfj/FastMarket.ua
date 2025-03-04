import React, { useState, useEffect } from 'react';
import cartIconGray from '../img/Cart Icon.png';
import cartIconGreen from '../img/Cart Icon Green.png';
import favoriteIconGray from '../img/Favorite.png';
import favoriteIconGreen from '../img/FavoriteGreen.png';
import ItemPlaceholder from '../img/Item.png';

export default function Items({ selectedCategory, addToCart, toggleFavorite, favorites, cart, toggleCartModal }) {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    let url = 'https://api.escuelajs.co/api/v1/products';
    if (selectedCategory) {
      url += `?categoryId=${selectedCategory}`;
    }
    fetch(url)
      .then((response) => response.json())
      .then((data) => setProducts(data))
      .catch((error) => console.error('Error fetching products:', error));
  }, [selectedCategory]);

  const isFavorite = (product) => favorites.some(item => item.id === product.id);
  const isInCart = (product) => cart.some(item => item.id === product.id);

  return (
    <div className="items-container">
      {products.length === 0 ? (
        <p>No products found.</p>
      ) : (
        products.map((product) => (
          <div key={product.id} className="item-card">
            {/* Favorite Icon overlapping the image */}
            <div className="favorite-icon" onClick={() => toggleFavorite(product)}>
              <img
                src={isFavorite(product) ? favoriteIconGreen : favoriteIconGray}
                alt="Favorite Icon"
              />
            </div>
            <img
              src={product.images[0] || ItemPlaceholder}
              alt={product.title}
              className="item-image"
            />
            <div className="item-description">{product.title}</div>
            <div className="item-bottom">
              <div className="item-price">{product.price} грн</div>
              <div
                className="cart-icon"
                onClick={() => {
                  if (isInCart(product)) {
                    toggleCartModal();
                  } else {
                    addToCart(product);
                  }
                }}
              >
                <img
                  src={isInCart(product) ? cartIconGreen : cartIconGray}
                  alt="Cart Icon"
                />
              </div>
            </div>
          </div>
        ))
      )}
    </div>
  );
}