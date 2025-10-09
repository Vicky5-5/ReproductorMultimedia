# Reproductor Multimedia

Este proyecto es un reproductor multimedia de archivos MP3.
Permite a los usuarios escuchar música localmente, mientras que los administradores pueden subir nuevas canciones al sistema. Es una alternativa sencilla a otras aplicaciones multimedia.

## 🧰 Tecnologías utilizadas

- Back-End: C# con .NET Core.

- Base de datos: SQL Server Management Express.

- Front-End: HTML5, CSS, Bootstrap y JavaScript.
- Librerías externas:

    - MediaElement.js para la reproducción de audio
    - Spotify Web API (OAuth 2.0 + perfil de usuario)
    - PayPal SDK (simulación de pago)
    - Google reCAPTCHA v3 para protección en el login

👥 Tipos de usuario
- Usuario estándar: puede escuchar canciones disponibles en el sistema.

- Administrador: puede subir nuevas canciones, gestionar el catálogo y acceder a funciones avanzadas.

📬 Notificaciones por correo electrónico
La aplicación incluye envío automático de correos en los siguientes casos:

✅ Registro de usuario: se envía un correo de bienvenida confirmando el alta en el sistema.

❌ Solicitud de baja: se envía un correo de confirmación notificando que el usuario ha sido dado de baja.

✨ Funcionalidades destacadas

🎧 Reproducción de archivos MP3 locales con MediaElement.js

👤 Autenticación con Spotify y visualización del perfil del usuario

🔄 Renovación automática del token de acceso a Spotify

💳 Simulación de pago con PayPal

📂 Panel de administración para subir nuevas canciones

🔐 Protección del login con Google reCAPTCHA v3

📧 Envío de correos automáticos en registro y baja de usuarios

## 🚀 Instalación

Habría que clonar el repositorio, además de poner su servidor de SQL propio junto con el nombre de la base de datos que quiere. En el appsetings estaría la configuración para el Paypal, Spotify y Gmail. Por seguridad no está incluido en el repositorio

```bash
git clone https://github.com/Vicky5-5/ReproductorMultimedia.git
cd ReproductorMultimedia
npm install
npm run dev

📄 Licencia
Este proyecto forma parte de mi portfolio personal. Puedes usarlo como referencia educativa o inspiración para tus propios desarrollos.
