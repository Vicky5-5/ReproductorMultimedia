// Para añadir canciones a una lista personalizada
document.addEventListener("DOMContentLoaded", function () {
    // Buscamos el formulario por el id
    const form = document.getElementById("formCrearLista");

    // Comprobamos que existe el formulario
    if (form) {
        // Añadimos el evento submit al formulario
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const nombreLista = document.getElementById("nombreLista").value;
            const select = document.getElementById("cancionesSeleccionadas");
            const idsCanciones = Array.from(select.selectedOptions).map(opt => parseInt(opt.value));

            // Enviamos los datos al servidor
            fetch('/VistaUsuario/CrearLista', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    nombreLista: nombreLista,
                    idsCanciones: idsCanciones
                })
            })
                // Se procesa la respuesta
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        alert(data.mensaje);
                        const modal = bootstrap.Modal.getInstance(document.getElementById('crearListaReproduccion')); // Llamamos al modal
                        modal.hide();
                    } else {
                        alert(data.mensaje);
                    }
                })
                .catch(error => {
                    console.error("Error al crear la lista:", error);
                });
        });
    }
});
