import React from "react";

export default function Footer() {
  return (
    <footer className="footer">
      <div className="footer-container">
        {/* Консультація */}
        <div className="footer-column">
          <h3>Консультація</h3>
          <p>+38 0** *** ****</p>
          <p>+38 0** *** ****</p>
          <p>fastmarket@gmail.com</p>
        </div>

        {/* Допомога */}
        <div className="footer-column">
          <h3>Допомога</h3>
          <a href="#">Доставка та оплата</a>
          <a href="#">Кредит</a>
          <a href="#">Гарантія</a>
          <a href="#">Повернення товару</a>
          <a href="#">Сервісні центри</a>
        </div>

        {/* Інформація про компанію */}
        <div className="footer-column">
          <h3>Інформація про компанію</h3>
          <a href="#">Про нас</a>
          <a href="#">Умови використання сайту</a>
          <a href="#">Вакансії</a>
          <a href="#">Контакти</a>
          <a href="#">Всі категорії</a>
        </div>

        {/* Покупцям */}
        <div className="footer-column">
          <h3>Покупцям</h3>
          <a href="#">Особистий кабінет</a>
          <a href="#">Зворотній  зв’язок</a>
          <a href="#">Акції</a>
          <a href="#">Новинки</a>
          <a href="#">Відгуки</a>
        </div>

        {/* Месенджери */}
        <div className="footer-column">
          <h3>Месенджери</h3>
          <a href="#">
            <img
              src="https://via.placeholder.com/20x20"
              alt="Telegram"
              className="footer-icon"
            />
            Telegram
          </a>
          <a href="#">
            <img
              src="https://via.placeholder.com/20x20"
              alt="Viber"
              className="footer-icon"
            />
            Viber
          </a>
        </div>
      </div>
    </footer>
  );
}