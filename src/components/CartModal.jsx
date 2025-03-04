import React from 'react';
import trashIcon from '../img/Trash.png';


export default function CartModal({
  cart,
  toggleCartModal,
  incrementQuantity,
  decrementQuantity,
  removeFromCart,
}) {
  // Calculate the total price of all items in the cart
  const totalPrice = cart.reduce((acc, item) => acc + item.price * item.quantity, 0);

  return (
    <div className="cart-modal-overlay" onClick={toggleCartModal}>
      {/* 
        Stop click propagation on the actual modal so it doesn’t close 
        when clicking inside 
      */}
      <div className="cart-modal" onClick={(e) => e.stopPropagation()}>
        {/* Header */}
        <div className="cart-modal-header">
          <h2>Кошик</h2>
          <button className="cart-modal-close" onClick={toggleCartModal}>
            ✕
          </button>
        </div>

        {/* Cart Items */}
        {cart.length === 0 ? (
          <p className="empty-cart">Ваш кошик порожній.</p>
        ) : (
          <ul className="cart-items-list">
            {cart.map((item) => (
              <li key={item.id} className="cart-item">
                {/* Product Image */}
                <img
                  className="cart-item-image"
                  src={item.images[0]}
                  alt={item.title}
                />

                {/* Title (Removed "Reserved") */}
                <div className="cart-item-info">
                  <div className="cart-item-title">{item.title}</div>
                  {/* If you want an extra code line, you can show item.id or something else */}
                  {/* <div className="cart-item-code">{item.id}</div> */}
                </div>

                {/* Quantity Controls */}
                <div className="cart-item-quantity">
                  <button onClick={() => decrementQuantity(item.id)}>-</button>
                  <span>{item.quantity}</span>
                  <button onClick={() => incrementQuantity(item.id)}>+</button>
                </div>

                {/* Price (item.price * quantity) */}
                <div className="cart-item-price">
                  {item.price * item.quantity} грн
                </div>

                {/* Remove Item (trash icon) */}
                <button
                  className="cart-item-remove"
                  onClick={() => removeFromCart(item.id)}
                >
                  <img src={trashIcon} alt="Remove Item" />
                </button>
              </li>
            ))}
          </ul>
        )}

        {/* Footer with total and checkout button */}
        <div className="cart-modal-footer">
          <div className="cart-total">Разом: {totalPrice} грн</div>
          <button className="checkout-button">Оформити замовлення</button>
        </div>
      </div>
    </div>
  );
}