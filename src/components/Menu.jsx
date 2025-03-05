import React, { useState, useEffect } from 'react';

export default function Menu({ onSelectCategory }) {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    fetch('https://api.escuelajs.co/api/v1/categories')
      .then((response) => response.json())
      .then((data) => setCategories(data))
      .catch((error) => console.error('Error fetching categories:', error));
  }, []);

  return (
    <nav className="menu">
      <ul>
        {/* Option to show all products */}
        <li onClick={() => onSelectCategory(null)}>All Products</li>
        {/* Dynamic Categories from API */}
        {categories.map((category) => (
          <li key={category.id} onClick={() => onSelectCategory(category.id)}>
            {category.name}
          </li>
        ))}
        {/* You can also include static categories if needed */}
        {/* <li>Смартфони та телефони</li>
        ... */}
      </ul>
    </nav>
  );
}