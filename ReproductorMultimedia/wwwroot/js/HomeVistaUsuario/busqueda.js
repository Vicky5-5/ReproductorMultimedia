// Funcionalidad de búsqueda de forma dinámica (solo busca por nombre de la canción)
document.addEventListener("DOMContentLoaded", function () {
    const inputBusqueda = document.getElementById("barraBusqueda");
    const filas = document.querySelectorAll("table.table tbody tr");

    inputBusqueda.addEventListener("input", function () {
        const filtro = inputBusqueda.value.toLowerCase();

        filas.forEach(fila => {
            const titulo = fila.querySelector("td:nth-child(3)").textContent.toLowerCase();

            if (titulo.includes(filtro)) {
                fila.style.display = "";
            } else {
                fila.style.display = "none";
            }
        });
    });
});
