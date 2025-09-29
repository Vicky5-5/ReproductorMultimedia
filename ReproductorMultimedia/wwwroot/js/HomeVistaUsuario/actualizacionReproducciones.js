$(document).ready(function () {
    // Seleccionamos todos los elementos de audio
    $("audio").on("play", function () {
        //Con el this cogemos el id de la cancion (audio-1, audio-2. depende de su ID)
        const id = parseInt(this.id.split("-")[1], 10); //Dividimos la cadena en dos partes ("audio", "1"). El [1] toma el número y con el Parseint covertimos la cadena "1" en entero (int)

        //Busca el valor oculto que contiene el Token deanti-falsificación. El token es obligatorio para los post que modifican datos
        const token = $('input[name="__RequestVerificationToken"]').val();

        //Lanzamos una solicitud de Ajax (que es un HTTP asíncrona) al servidor sin recargar la página
        $.ajax({
            url: '/Canciones/ActualizarReproducciones', //Es la URl dónde enviamos la solicitud
            type: 'POST', //El tipo de solicitud en el controlador
            //Los datos que enviamos a la petición (el antiForfery y el idCancion)
            data: {
                __RequestVerificationToken: token,
                id: id
            },
            //Si la petición fue un éxito manda una petición exitosa
            success: function (response) {
                //Si es true cambia el texto visible de las reproducciones por int
                if (response.success) {
                    $("#reproducciones-" + response.idCancion).text(response.reproduccionesTotales);
                } else {
                    console.warn("No se pudo actualizar la reproducción:", response.mensaje);
                }
            },
            error: function () {
                console.error("Error al llamar al servidor.");
            }
        });
    });
});