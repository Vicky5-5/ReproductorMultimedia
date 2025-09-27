// Inicializa MediaElement.js y gestiona la animación del disco + actualización de reproducciones
document.addEventListener('DOMContentLoaded', function () {
    // Inicializa los reproductores MediaElement.js
    const players = document.querySelectorAll('audio');
    players.forEach(function (player) {
        new MediaElementPlayer(player, {
            features: ['playpause', 'progress', 'current', 'duration', 'volume'],
            audioVolume: 'horizontal'
        });
    });

    // Animación de discos y conteo de reproducciones
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : '';

    const actualizarReproduccionesUrl = '/Canciones/ActualizarReproducciones'; // URL actualizada (usa Url.Action en Razor si necesitas)

    document.querySelectorAll('audio').forEach(audio => {
        const id = audio.id.split('-')[1];
        const disco = document.getElementById('disco-' + id);

        if (!disco) return;

        let reproduccionContada = false;

        audio.addEventListener('playing', () => {
            disco.classList.add('girando');

            if (!reproduccionContada) {
                reproduccionContada = true;

                const formData = new FormData();
                formData.append('__RequestVerificationToken', token);
                formData.append('id', id);

                fetch(actualizarReproduccionesUrl, {
                    method: 'POST',
                    body: formData,
                    credentials: 'same-origin'
                })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            const contador = document.querySelector(`#reproducciones-${data.idCancion}`);
                            if (contador) {
                                contador.textContent = data.reproduccionesTotales;
                            }
                        } else {
                            console.error('❌ Error en el servidor:', data.mensaje);
                        }
                    })
                    .catch(err => {
                        console.error('❌ Error en fetch:', err);
                    });
            }
        });

        audio.addEventListener('pause', () => {
            disco.classList.remove('girando');
        });

        audio.addEventListener('ended', () => {
            disco.classList.remove('girando');
            reproduccionContada = false; // Permitir contar una nueva reproducción si se vuelve a reproducir desde el principio
        });
    });
});
