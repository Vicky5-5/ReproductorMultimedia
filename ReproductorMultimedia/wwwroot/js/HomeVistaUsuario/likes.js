// Función global para alternar likes
function toggleLike(button) {
    if (button.disabled) return; // Evita clics múltiples

    button.disabled = true;

    const idCancion = parseInt(button.getAttribute('data-idcancion'));
    const contador = document.getElementById(`contadorLikes-${idCancion}`);

    fetch('/VistaUsuario/LikeAlternar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(idCancion)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                button.classList.toggle('like-activo', data.dioLike);
                if (contador) {
                    contador.textContent = data.likesTotales;
                }
            } else {
                alert(data.mensaje);
            }
        })
        .catch(error => {
            console.error("Error al alternar like:", error);
        })
        .finally(() => {
            button.disabled = false; // Reactiva el botón
        });
}